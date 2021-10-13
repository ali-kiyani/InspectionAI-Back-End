using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InspectionAI.Migrations
{
    public partial class mig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Detections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssemblyLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblyLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssemblyLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AssemblyDetections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssemblyLineId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DetectionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DetectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblyDetections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssemblyDetections_AssemblyLines_AssemblyLineId",
                        column: x => x.AssemblyLineId,
                        principalTable: "AssemblyLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AssemblyDetections_Detections_DetectionId",
                        column: x => x.DetectionId,
                        principalTable: "Detections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssemblyDetections_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyDetections_AssemblyLineId",
                table: "AssemblyDetections",
                column: "AssemblyLineId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyDetections_DetectionId",
                table: "AssemblyDetections",
                column: "DetectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyDetections_ProductId",
                table: "AssemblyDetections",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyLines_ProductId",
                table: "AssemblyLines",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssemblyDetections");

            migrationBuilder.DropTable(
                name: "AssemblyLines");

            migrationBuilder.DropTable(
                name: "Detections");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
