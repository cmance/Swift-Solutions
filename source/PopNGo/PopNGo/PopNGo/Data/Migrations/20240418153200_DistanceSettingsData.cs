using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopNGo.Data.Migrations
{
    /// <inheritdoc />
    public partial class DistanceSettingsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DistanceUnit",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistanceUnit",
                table: "AspNetUsers");
        }
    }
}
