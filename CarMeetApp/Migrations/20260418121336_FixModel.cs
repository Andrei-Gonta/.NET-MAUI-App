using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarMeetApp.Migrations
{
    public partial class FixModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 12, 13, 35, 610, DateTimeKind.Utc).AddTicks(6985));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 12, 13, 35, 610, DateTimeKind.Utc).AddTicks(7163));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 12, 13, 35, 610, DateTimeKind.Utc).AddTicks(7166));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 12, 13, 2, 386, DateTimeKind.Utc).AddTicks(8171));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 12, 13, 2, 386, DateTimeKind.Utc).AddTicks(8354));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 12, 13, 2, 386, DateTimeKind.Utc).AddTicks(8356));
        }
    }
}
