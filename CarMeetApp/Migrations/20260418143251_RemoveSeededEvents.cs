using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace CarMeetApp.Migrations
{
    public partial class RemoveSeededEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CreatedAt", "Date", "Description", "Location", "Organizer", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 18, 14, 29, 31, 187, DateTimeKind.Utc).AddTicks(9961), new DateTime(2026, 4, 21, 18, 0, 0, 0, DateTimeKind.Local), "Casual evening meet for all builds: imports, muscle, and classics.", "Downtown Garage, Austin", "Turbo Club", "Sunset Street Meet" },
                    { 2, new DateTime(2026, 4, 18, 14, 29, 31, 188, DateTimeKind.Utc).AddTicks(149), new DateTime(2026, 4, 25, 9, 0, 0, 0, DateTimeKind.Local), "Morning cruise with photo stops and coffee at the summit.", "Blue Ridge Scenic Point", "Apex Riders", "Mountain Drive Meetup" },
                    { 3, new DateTime(2026, 4, 18, 14, 29, 31, 188, DateTimeKind.Utc).AddTicks(152), new DateTime(2026, 5, 2, 20, 0, 0, 0, DateTimeKind.Local), "Night showcase featuring custom lighting and audio builds.", "Riverside Plaza, Seattle", "NightShift Garage", "Neon Night Showcase" }
                });
        }
    }
}
