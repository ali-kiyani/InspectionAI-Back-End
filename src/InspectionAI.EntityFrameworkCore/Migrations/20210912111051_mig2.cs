using Microsoft.EntityFrameworkCore.Migrations;

namespace InspectionAI.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyDetections_AssemblyLines_AssemblyLineId",
                table: "AssemblyDetections");

            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyDetections_Detections_DetectionId",
                table: "AssemblyDetections");

            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyDetections_Products_ProductId",
                table: "AssemblyDetections");

            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyLines_Products_ProductId",
                table: "AssemblyLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Detections",
                table: "Detections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssemblyLines",
                table: "AssemblyLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssemblyDetections",
                table: "AssemblyDetections");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "Detections",
                newName: "Detection");

            migrationBuilder.RenameTable(
                name: "AssemblyLines",
                newName: "AssemblyLine");

            migrationBuilder.RenameTable(
                name: "AssemblyDetections",
                newName: "AssemblyDetection");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyLines_ProductId",
                table: "AssemblyLine",
                newName: "IX_AssemblyLine_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyDetections_ProductId",
                table: "AssemblyDetection",
                newName: "IX_AssemblyDetection_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyDetections_DetectionId",
                table: "AssemblyDetection",
                newName: "IX_AssemblyDetection_DetectionId");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyDetections_AssemblyLineId",
                table: "AssemblyDetection",
                newName: "IX_AssemblyDetection_AssemblyLineId");

            migrationBuilder.AddColumn<float>(
                name: "Confidence",
                table: "Detection",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Detection",
                table: "Detection",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssemblyLine",
                table: "AssemblyLine",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssemblyDetection",
                table: "AssemblyDetection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyDetection_AssemblyLine_AssemblyLineId",
                table: "AssemblyDetection",
                column: "AssemblyLineId",
                principalTable: "AssemblyLine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyDetection_Detection_DetectionId",
                table: "AssemblyDetection",
                column: "DetectionId",
                principalTable: "Detection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyDetection_Product_ProductId",
                table: "AssemblyDetection",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyLine_Product_ProductId",
                table: "AssemblyLine",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyDetection_AssemblyLine_AssemblyLineId",
                table: "AssemblyDetection");

            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyDetection_Detection_DetectionId",
                table: "AssemblyDetection");

            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyDetection_Product_ProductId",
                table: "AssemblyDetection");

            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyLine_Product_ProductId",
                table: "AssemblyLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Detection",
                table: "Detection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssemblyLine",
                table: "AssemblyLine");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssemblyDetection",
                table: "AssemblyDetection");

            migrationBuilder.DropColumn(
                name: "Confidence",
                table: "Detection");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Detection",
                newName: "Detections");

            migrationBuilder.RenameTable(
                name: "AssemblyLine",
                newName: "AssemblyLines");

            migrationBuilder.RenameTable(
                name: "AssemblyDetection",
                newName: "AssemblyDetections");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyLine_ProductId",
                table: "AssemblyLines",
                newName: "IX_AssemblyLines_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyDetection_ProductId",
                table: "AssemblyDetections",
                newName: "IX_AssemblyDetections_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyDetection_DetectionId",
                table: "AssemblyDetections",
                newName: "IX_AssemblyDetections_DetectionId");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyDetection_AssemblyLineId",
                table: "AssemblyDetections",
                newName: "IX_AssemblyDetections_AssemblyLineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Detections",
                table: "Detections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssemblyLines",
                table: "AssemblyLines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssemblyDetections",
                table: "AssemblyDetections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyDetections_AssemblyLines_AssemblyLineId",
                table: "AssemblyDetections",
                column: "AssemblyLineId",
                principalTable: "AssemblyLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyDetections_Detections_DetectionId",
                table: "AssemblyDetections",
                column: "DetectionId",
                principalTable: "Detections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyDetections_Products_ProductId",
                table: "AssemblyDetections",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyLines_Products_ProductId",
                table: "AssemblyLines",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
