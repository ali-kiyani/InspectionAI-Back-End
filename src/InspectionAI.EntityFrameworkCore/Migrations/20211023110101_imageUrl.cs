using Microsoft.EntityFrameworkCore.Migrations;

namespace InspectionAI.Migrations
{
    public partial class imageUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AssemblyDefects");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AssemblyDetection",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AssemblyDetection");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AssemblyDefects",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
