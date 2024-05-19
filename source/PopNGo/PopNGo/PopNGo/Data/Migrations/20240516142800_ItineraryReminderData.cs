using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopNGo.Data.Migrations
{
    /// <inheritdoc />
    public partial class ItineraryReminderData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ItineraryReminderTime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItineraryReminderTime",
                table: "AspNetUsers");
        }
    }
}
