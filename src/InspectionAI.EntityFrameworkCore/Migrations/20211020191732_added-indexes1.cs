using Microsoft.EntityFrameworkCore.Migrations;

namespace InspectionAI.Migrations
{
    public partial class addedindexes1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stage",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Product",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Defects",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stage_Name_Cost",
                table: "Stage",
                columns: new[] { "Name", "Cost" });

            migrationBuilder.CreateIndex(
                name: "IX_Product_Name",
                table: "Product",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_Name",
                table: "Defects",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyDetection_DetectionTime_DefectsCount",
                table: "AssemblyDetection",
                columns: new[] { "DetectionTime", "DefectsCount" });

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyDefects_DetectionTime_Confidence",
                table: "AssemblyDefects",
                columns: new[] { "DetectionTime", "Confidence" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stage_Name_Cost",
                table: "Stage");

            migrationBuilder.DropIndex(
                name: "IX_Product_Name",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Defects_Name",
                table: "Defects");

            migrationBuilder.DropIndex(
                name: "IX_AssemblyDetection_DetectionTime_DefectsCount",
                table: "AssemblyDetection");

            migrationBuilder.DropIndex(
                name: "IX_AssemblyDefects_DetectionTime_Confidence",
                table: "AssemblyDefects");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stage",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Defects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
