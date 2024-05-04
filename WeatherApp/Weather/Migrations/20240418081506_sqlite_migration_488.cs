using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Migrations
{
    /// <inheritdoc />
    public partial class sqlite_migration_488 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "MyWeatherForecasts",
                newName: "AcquireDateTime");

            migrationBuilder.AddColumn<string>(
                name: "CustomName",
                table: "SavedLocations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcquireDateTime",
                table: "MyWeatherInfos",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e3e33137-0374-4ebb-9c31-d6220850dee5", "AQAAAAIAAYagAAAAEABUIAK1eAazMvKsp87o5NAOGUfpF7TMyo5Hj2l3xLe6CBo1XMv3vYY2fqs4XBNr1Q==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomName",
                table: "SavedLocations");

            migrationBuilder.DropColumn(
                name: "AcquireDateTime",
                table: "MyWeatherInfos");

            migrationBuilder.RenameColumn(
                name: "AcquireDateTime",
                table: "MyWeatherForecasts",
                newName: "DateTime");

            migrationBuilder.AddColumn<double>(
                name: "LocationName",
                table: "Locations",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4184f9c5-190a-45be-b0c6-484e0e3e3ba4", "AQAAAAIAAYagAAAAEPFlON7cwWKRKlhF14lB2rZ1pR20zGBDycA9sABOyZuXw4g6JvrqeonRDp/F1AJjSA==" });
        }
    }
}
