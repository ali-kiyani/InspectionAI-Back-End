using Microsoft.EntityFrameworkCore.Migrations;

namespace InspectionAI.Migrations
{
    public partial class defectsCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefectsCount",
                table: "AssemblyDetection",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefectsCount",
                table: "AssemblyDetection");
        }
    }
}
