using Microsoft.EntityFrameworkCore.Migrations;

namespace InspectionAI.Migrations
{
    public partial class mig9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyDefects_Detection_DetectionId",
                table: "AssemblyDefects");

            migrationBuilder.DropTable(
                name: "Detection");

            migrationBuilder.RenameColumn(
                name: "DetectionId",
                table: "AssemblyDefects",
                newName: "DefectId");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyDefects_DetectionId",
                table: "AssemblyDefects",
                newName: "IX_AssemblyDefects_DefectId");

            migrationBuilder.AddColumn<float>(
                name: "Confidence",
                table: "AssemblyDefects",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AssemblyDefects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyDefects_Defects_DefectId",
                table: "AssemblyDefects",
                column: "DefectId",
                principalTable: "Defects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyDefects_Defects_DefectId",
                table: "AssemblyDefects");

            migrationBuilder.DropColumn(
                name: "Confidence",
                table: "AssemblyDefects");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AssemblyDefects");

            migrationBuilder.RenameColumn(
                name: "DefectId",
                table: "AssemblyDefects",
                newName: "DetectionId");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyDefects_DefectId",
                table: "AssemblyDefects",
                newName: "IX_AssemblyDefects_DetectionId");

            migrationBuilder.CreateTable(
                name: "Detection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Confidence = table.Column<float>(type: "real", nullable: false),
                    DefectId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Detection_Defects_DefectId",
                        column: x => x.DefectId,
                        principalTable: "Defects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Detection_DefectId",
                table: "Detection",
                column: "DefectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyDefects_Detection_DetectionId",
                table: "AssemblyDefects",
                column: "DetectionId",
                principalTable: "Detection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
