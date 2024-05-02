using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopNGo.Data.Migrations
{
    /// <inheritdoc />
    public partial class WeatherSettingsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TemperatureUnit",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false);
            
            migrationBuilder.AddColumn<bool>(
                name: "MeasurementUnit",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemperatureUnit",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MeasurementUnit",
                table: "AspNetUsers");
        }
    }
}
