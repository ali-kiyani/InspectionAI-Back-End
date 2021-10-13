using Microsoft.EntityFrameworkCore.Migrations;

namespace InspectionAI.Migrations
{
    public partial class productadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "StageDefects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Stage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "AssemblyLine",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "AssemblyDetection",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_StageDefects_ProductId",
                table: "StageDefects",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Stage_ProductId",
                table: "Stage",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyLine_ProductId",
                table: "AssemblyLine",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyDetection_ProductId",
                table: "AssemblyDetection",
                column: "ProductId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Stage_Product_ProductId",
                table: "Stage",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_StageDefects_Product_ProductId",
                table: "StageDefects",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyDetection_Product_ProductId",
                table: "AssemblyDetection");

            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyLine_Product_ProductId",
                table: "AssemblyLine");

            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Product_ProductId",
                table: "Stage");

            migrationBuilder.DropForeignKey(
                name: "FK_StageDefects_Product_ProductId",
                table: "StageDefects");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropIndex(
                name: "IX_StageDefects_ProductId",
                table: "StageDefects");

            migrationBuilder.DropIndex(
                name: "IX_Stage_ProductId",
                table: "Stage");

            migrationBuilder.DropIndex(
                name: "IX_AssemblyLine_ProductId",
                table: "AssemblyLine");

            migrationBuilder.DropIndex(
                name: "IX_AssemblyDetection_ProductId",
                table: "AssemblyDetection");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "StageDefects");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Stage");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "AssemblyLine");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "AssemblyDetection");
        }
    }
}
