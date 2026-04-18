using System.Text.Json;

namespace CarMeetApp.Services;

public class CarApiService
{
    private const string BaseUrl = "https://www.carapi.app/api";
    private static readonly HttpClient Client = new()
    {
        Timeout = TimeSpan.FromSeconds(12)
    };

    public async Task<List<string>> GetBrandsAsync()
    {
        var fallback = GetFallbackBrands();

        try
        {
            using var response = await Client.GetAsync($"{BaseUrl}/makes");
            if (!response.IsSuccessStatusCode)
            {
                return fallback;
            }

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);
            var list = new List<string>();

            if (doc.RootElement.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in data.EnumerateArray())
                {
                    if (item.TryGetProperty("name", out var nameProp))
                    {
                        var name = nameProp.GetString();
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            list.Add(name);
                        }
                    }
                }
            }

            return list.Count > 0 ? list.OrderBy(x => x).ToList() : fallback;
        }
        catch
        {
            return fallback;
        }
    }

    public async Task<List<string>> GetModelsAsync(string brand)
    {
        var fallback = GetFallbackModels(brand);

        try
        {
            var uri = $"{BaseUrl}/models?make={Uri.EscapeDataString(brand)}";
            using var response = await Client.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                return fallback;
            }

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);
            var list = new List<string>();

            if (doc.RootElement.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in data.EnumerateArray())
                {
                    if (item.TryGetProperty("name", out var nameProp))
                    {
                        var name = nameProp.GetString();
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            list.Add(name);
                        }
                    }
                }
            }

            return list.Count > 0 ? list.Distinct().OrderBy(x => x).ToList() : fallback;
        }
        catch
        {
            return fallback;
        }
    }

    public async Task<List<CarGenerationOption>> GetGenerationsAsync(string brand, string model)
    {
        var fallback = GetFallbackGenerations(brand, model);
        var year = DateTime.UtcNow.Year;

        try
        {
            var uri = $"{BaseUrl}/trims?make={Uri.EscapeDataString(brand)}&model={Uri.EscapeDataString(model)}&year={year}&verbose=yes";
            using var response = await Client.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                return fallback;
            }

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);
            var list = new List<CarGenerationOption>();

            if (doc.RootElement.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in data.EnumerateArray())
                {
                    var generation = GetString(item, "trim")
                        ?? GetString(item, "submodel")
                        ?? GetString(item, "description")
                        ?? "Standard";

                    int? hp = null;
                    double? engineL = null;

                    if (item.TryGetProperty("engine", out var engineObj) && engineObj.ValueKind == JsonValueKind.Object)
                    {
                        hp = GetInt(engineObj, "horsepower_hp");
                        engineL = GetDouble(engineObj, "size");
                    }
                    else
                    {
                        hp = GetInt(item, "horsepower_hp");
                        engineL = GetDouble(item, "size");
                    }

                    list.Add(new CarGenerationOption(generation, hp, engineL));
                }
            }

            if (list.Count == 0)
            {
                return fallback;
            }

            return list
                .GroupBy(x => x.Generation)
                .Select(g => g.First())
                .OrderBy(x => x.Generation)
                .ToList();
        }
        catch
        {
            return fallback;
        }
    }

    private static string? GetString(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String
            ? prop.GetString()
            : null;
    }

    private static int? GetInt(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var prop))
        {
            return null;
        }

        if (prop.ValueKind == JsonValueKind.Number && prop.TryGetInt32(out var intVal))
        {
            return intVal;
        }

        if (prop.ValueKind == JsonValueKind.String && int.TryParse(prop.GetString(), out var parsed))
        {
            return parsed;
        }

        return null;
    }

    private static double? GetDouble(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var prop))
        {
            return null;
        }

        if (prop.ValueKind == JsonValueKind.Number && prop.TryGetDouble(out var val))
        {
            return val;
        }

        if (prop.ValueKind == JsonValueKind.String && double.TryParse(prop.GetString(), out var parsed))
        {
            return parsed;
        }

        return null;
    }

    private static List<string> GetFallbackBrands() => ["Toyota", "Honda", "BMW"];

    private static List<string> GetFallbackModels(string brand) => brand switch
    {
        "Toyota" => ["Corolla", "Camry", "GR Supra"],
        "Honda" => ["Civic", "Accord", "CR-V"],
        "BMW" => ["M3", "M4", "330i"],
        _ => ["Unknown Model"]
    };

    private static List<CarGenerationOption> GetFallbackGenerations(string brand, string model)
    {
        var key = $"{brand}|{model}";
        return key switch
        {
            "Toyota|Corolla" =>
            [
                new CarGenerationOption("E210", 169, 2.0),
                new CarGenerationOption("E170", 132, 1.8)
            ],
            "Honda|Civic" =>
            [
                new CarGenerationOption("11th Gen", 180, 1.5),
                new CarGenerationOption("10th Gen", 174, 1.5)
            ],
            "BMW|M3" =>
            [
                new CarGenerationOption("G80", 473, 3.0),
                new CarGenerationOption("F80", 425, 3.0)
            ],
            _ =>
            [
                new CarGenerationOption("Standard", null, null)
            ]
        };
    }
}

public record CarGenerationOption(string Generation, int? HorsepowerHp, double? EngineSizeLiters)
{
    public override string ToString() => Generation;
}
