using Microsoft.EntityFrameworkCore.Migrations;

namespace InspectionAI.Migrations
{
    public partial class mig7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyDetection_Product_ProductId",
                table: "AssemblyDetection");

            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyLine_Product_ProductId",
                table: "AssemblyLine");

            migrationBuilder.DropForeignKey(
                name: "FK_Defects_Product_ProductId",
                table: "Defects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDefects_Product_ProductId",
                table: "ProductDefects");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductDefects",
                newName: "StageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDefects_ProductId",
                table: "ProductDefects",
                newName: "IX_ProductDefects_StageId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Defects",
                newName: "StageId");

            migrationBuilder.RenameIndex(
                name: "IX_Defects_ProductId",
                table: "Defects",
                newName: "IX_Defects_StageId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "AssemblyDetection",
                newName: "StageId");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyDetection_ProductId",
                table: "AssemblyDetection",
                newName: "IX_AssemblyDetection_StageId");

            migrationBuilder.CreateTable(
                name: "Stage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stage", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyDetection_Stage_StageId",
                table: "AssemblyDetection",
                column: "StageId",
                principalTable: "Stage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyLine_Stage_StageId",
                table: "AssemblyLine",
                column: "ProductId",
                principalTable: "Stage",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_Stage_StageId",
                table: "Defects",
                column: "StageId",
                principalTable: "Stage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDefects_Stage_StageId",
                table: "ProductDefects",
                column: "StageId",
                principalTable: "Stage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyDetection_Stage_StageId",
                table: "AssemblyDetection");

            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyLine_Stage_StageId",
                table: "AssemblyLine");

            migrationBuilder.DropForeignKey(
                name: "FK_Defects_Stage_StageId",
                table: "Defects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDefects_Stage_StageId",
                table: "ProductDefects");

            migrationBuilder.DropTable(
                name: "Stage");

            migrationBuilder.RenameColumn(
                name: "StageId",
                table: "ProductDefects",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDefects_StageId",
                table: "ProductDefects",
                newName: "IX_ProductDefects_ProductId");

            migrationBuilder.RenameColumn(
                name: "StageId",
                table: "Defects",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Defects_StageId",
                table: "Defects",
                newName: "IX_Defects_ProductId");

            migrationBuilder.RenameColumn(
                name: "StageId",
                table: "AssemblyDetection",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyDetection_StageId",
                table: "AssemblyDetection",
                newName: "IX_AssemblyDetection_ProductId");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_Product_ProductId",
                table: "Defects",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDefects_Product_ProductId",
                table: "ProductDefects",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
