using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace CarMeetApp.Services;

public class FirebaseAuthService
{
    private const string ApiKey = "AIzaSyBt25oA9MWvz_S-L_bM9fmZ0xinYiyq05w";
    private const string SignInUrl = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword";

    private static readonly HttpClient Client = new()
    {
        Timeout = TimeSpan.FromSeconds(15)
    };

    public async Task<FirebaseLoginResult> SignInAsync(string email, string password)
    {
        try
        {
            var request = new FirebaseSignInRequest
            {
                Email = email,
                Password = password,
                ReturnSecureToken = true
            };

            using var response = await Client.PostAsJsonAsync($"{SignInUrl}?key={ApiKey}", request);
            if (response.IsSuccessStatusCode)
            {
                var success = await response.Content.ReadFromJsonAsync<FirebaseSignInResponse>();
                if (success is null || string.IsNullOrWhiteSpace(success.Email))
                {
                    return FirebaseLoginResult.Fail("Firebase returned an empty response.");
                }

                return FirebaseLoginResult.Ok(success.Email, success.IdToken ?? string.Empty);
            }

            var error = await response.Content.ReadFromJsonAsync<FirebaseErrorResponse>();
            var message = MapFirebaseError(error?.Error?.Message);
            return FirebaseLoginResult.Fail(message);
        }
        catch (Exception)
        {
            return FirebaseLoginResult.Fail("Could not connect to Firebase. Check internet and try again.");
        }
    }

    private static string MapFirebaseError(string? code)
    {
        return code switch
        {
            "INVALID_LOGIN_CREDENTIALS" => "Invalid email or password.",
            "EMAIL_NOT_FOUND" => "Email was not found in Firebase Auth.",
            "INVALID_PASSWORD" => "Invalid email or password.",
            "USER_DISABLED" => "This Firebase user is disabled.",
            "TOO_MANY_ATTEMPTS_TRY_LATER" => "Too many attempts. Try again later.",
            _ => $"Login failed: {code ?? "Unknown Firebase error"}"
        };
    }
}

public class FirebaseLoginResult
{
    public bool IsSuccess { get; private init; }
    public string Email { get; private init; } = string.Empty;
    public string IdToken { get; private init; } = string.Empty;
    public string ErrorMessage { get; private init; } = string.Empty;

    public static FirebaseLoginResult Ok(string email, string idToken) =>
        new() { IsSuccess = true, Email = email, IdToken = idToken };

    public static FirebaseLoginResult Fail(string message) =>
        new() { IsSuccess = false, ErrorMessage = message };
}

internal class FirebaseSignInRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("returnSecureToken")]
    public bool ReturnSecureToken { get; set; }
}

internal class FirebaseSignInResponse
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("idToken")]
    public string? IdToken { get; set; }
}

internal class FirebaseErrorResponse
{
    [JsonPropertyName("error")]
    public FirebaseErrorBody? Error { get; set; }
}

internal class FirebaseErrorBody
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
