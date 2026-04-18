using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarMeetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherDetailsToEventUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CreatedAt", "Date", "Description", "Location", "Organizer", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 18, 14, 50, 29, 239, DateTimeKind.Utc).AddTicks(3063), new DateTime(2026, 4, 21, 18, 0, 0, 0, DateTimeKind.Local), "Casual evening meet for all builds: imports, muscle, and classics.", "Downtown Garage, Austin", "Turbo Club", "Sunset Street Meet" },
                    { 2, new DateTime(2026, 4, 18, 14, 50, 29, 239, DateTimeKind.Utc).AddTicks(3237), new DateTime(2026, 4, 25, 9, 0, 0, 0, DateTimeKind.Local), "Morning cruise with photo stops and coffee at the summit.", "Blue Ridge Scenic Point", "Apex Riders", "Mountain Drive Meetup" },
                    { 3, new DateTime(2026, 4, 18, 14, 50, 29, 239, DateTimeKind.Utc).AddTicks(3240), new DateTime(2026, 5, 2, 20, 0, 0, 0, DateTimeKind.Local), "Night showcase featuring custom lighting and audio builds.", "Riverside Plaza, Seattle", "NightShift Garage", "Neon Night Showcase" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
