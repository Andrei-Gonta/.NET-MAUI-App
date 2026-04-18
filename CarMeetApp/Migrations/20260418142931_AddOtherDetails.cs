using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarMeetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherDetails",
                table: "Users",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OtherDetails",
                table: "EventUsers",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 29, 31, 187, DateTimeKind.Utc).AddTicks(9961));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 29, 31, 188, DateTimeKind.Utc).AddTicks(149));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 29, 31, 188, DateTimeKind.Utc).AddTicks(152));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherDetails",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OtherDetails",
                table: "EventUsers");

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
    }
}
