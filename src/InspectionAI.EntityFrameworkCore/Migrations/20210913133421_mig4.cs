using Microsoft.EntityFrameworkCore.Migrations;

namespace InspectionAI.Migrations
{
    public partial class mig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyDetection_Detection_DetectionId",
                table: "AssemblyDetection");

            migrationBuilder.DropIndex(
                name: "IX_AssemblyDetection_DetectionId",
                table: "AssemblyDetection");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Detection");

            migrationBuilder.DropColumn(
                name: "DetectionId",
                table: "AssemblyDetection");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Detection",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<int>(
                name: "DefectId",
                table: "Detection",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AssemblyDefects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssemblyDetectionId = table.Column<int>(type: "int", nullable: false),
                    DetectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblyDefects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssemblyDefects_AssemblyDetection_AssemblyDetectionId",
                        column: x => x.AssemblyDetectionId,
                        principalTable: "AssemblyDetection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssemblyDefects_Detection_DetectionId",
                        column: x => x.DetectionId,
                        principalTable: "Detection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Defects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Defects_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Detection_DefectId",
                table: "Detection",
                column: "DefectId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyDefects_AssemblyDetectionId",
                table: "AssemblyDefects",
                column: "AssemblyDetectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyDefects_DetectionId",
                table: "AssemblyDefects",
                column: "DetectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_ProductId",
                table: "Defects",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Detection_Defects_DefectId",
                table: "Detection",
                column: "DefectId",
                principalTable: "Defects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Detection_Defects_DefectId",
                table: "Detection");

            migrationBuilder.DropTable(
                name: "AssemblyDefects");

            migrationBuilder.DropTable(
                name: "Defects");

            migrationBuilder.DropIndex(
                name: "IX_Detection_DefectId",
                table: "Detection");

            migrationBuilder.DropColumn(
                name: "DefectId",
                table: "Detection");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Detection",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Detection",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DetectionId",
                table: "AssemblyDetection",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyDetection_DetectionId",
                table: "AssemblyDetection",
                column: "DetectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyDetection_Detection_DetectionId",
                table: "AssemblyDetection",
                column: "DetectionId",
                principalTable: "Detection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
