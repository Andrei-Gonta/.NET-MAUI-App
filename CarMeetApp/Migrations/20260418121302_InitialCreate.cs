using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarMeetApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Organizer = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    SelectedBrand = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SelectedModel = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SelectedGeneration = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    HorsepowerHp = table.Column<int>(type: "INTEGER", nullable: true),
                    EngineSizeLiters = table.Column<double>(type: "REAL", nullable: true),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EventItemId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Events_EventItemId",
                        column: x => x.EventItemId,
                        principalTable: "Events",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EventUsers",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    SignedUpAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CarBrand = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CarModel = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CarGeneration = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CarHorsepowerHp = table.Column<int>(type: "INTEGER", nullable: true),
                    CarEngineSizeLiters = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventUsers", x => new { x.EventId, x.UserId });
                    table.ForeignKey(
                        name: "FK_EventUsers_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CreatedAt", "Date", "Description", "Location", "Organizer", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 18, 12, 13, 2, 386, DateTimeKind.Utc).AddTicks(8171), new DateTime(2026, 4, 21, 18, 0, 0, 0, DateTimeKind.Local), "Casual evening meet for all builds: imports, muscle, and classics.", "Downtown Garage, Austin", "Turbo Club", "Sunset Street Meet" },
                    { 2, new DateTime(2026, 4, 18, 12, 13, 2, 386, DateTimeKind.Utc).AddTicks(8354), new DateTime(2026, 4, 25, 9, 0, 0, 0, DateTimeKind.Local), "Morning cruise with photo stops and coffee at the summit.", "Blue Ridge Scenic Point", "Apex Riders", "Mountain Drive Meetup" },
                    { 3, new DateTime(2026, 4, 18, 12, 13, 2, 386, DateTimeKind.Utc).AddTicks(8356), new DateTime(2026, 5, 2, 20, 0, 0, 0, DateTimeKind.Local), "Night showcase featuring custom lighting and audio builds.", "Riverside Plaza, Seattle", "NightShift Garage", "Neon Night Showcase" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventUsers_UserId",
                table: "EventUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EventItemId",
                table: "Users",
                column: "EventItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventUsers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
