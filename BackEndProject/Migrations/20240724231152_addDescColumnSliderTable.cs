using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEndProject.Migrations
{
    /// <inheritdoc />
    public partial class addDescColumnSliderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sliders");
        }
    }
}
