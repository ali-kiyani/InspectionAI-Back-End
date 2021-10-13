using Microsoft.EntityFrameworkCore.Migrations;

namespace InspectionAI.Migrations
{
    public partial class mig8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyLine_Stage_ProductId",
                table: "AssemblyLine");

            migrationBuilder.DropForeignKey(
                name: "FK_Defects_Stage_StageId",
                table: "Defects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDefects_Defects_DefectId",
                table: "ProductDefects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDefects_Stage_StageId",
                table: "ProductDefects");

            migrationBuilder.DropIndex(
                name: "IX_Defects_StageId",
                table: "Defects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDefects",
                table: "ProductDefects");

            migrationBuilder.DropColumn(
                name: "StageId",
                table: "Defects");

            migrationBuilder.RenameTable(
                name: "ProductDefects",
                newName: "StageDefects");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "AssemblyLine",
                newName: "StageId");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyLine_ProductId",
                table: "AssemblyLine",
                newName: "IX_AssemblyLine_StageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDefects_StageId",
                table: "StageDefects",
                newName: "IX_StageDefects_StageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDefects_DefectId",
                table: "StageDefects",
                newName: "IX_StageDefects_DefectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StageDefects",
                table: "StageDefects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyLine_Stage_StageId",
                table: "AssemblyLine",
                column: "StageId",
                principalTable: "Stage",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_StageDefects_Defects_DefectId",
                table: "StageDefects",
                column: "DefectId",
                principalTable: "Defects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StageDefects_Stage_StageId",
                table: "StageDefects",
                column: "StageId",
                principalTable: "Stage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyLine_Stage_StageId",
                table: "AssemblyLine");

            migrationBuilder.DropForeignKey(
                name: "FK_StageDefects_Defects_DefectId",
                table: "StageDefects");

            migrationBuilder.DropForeignKey(
                name: "FK_StageDefects_Stage_StageId",
                table: "StageDefects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StageDefects",
                table: "StageDefects");

            migrationBuilder.RenameTable(
                name: "StageDefects",
                newName: "ProductDefects");

            migrationBuilder.RenameColumn(
                name: "StageId",
                table: "AssemblyLine",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AssemblyLine_StageId",
                table: "AssemblyLine",
                newName: "IX_AssemblyLine_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_StageDefects_StageId",
                table: "ProductDefects",
                newName: "IX_ProductDefects_StageId");

            migrationBuilder.RenameIndex(
                name: "IX_StageDefects_DefectId",
                table: "ProductDefects",
                newName: "IX_ProductDefects_DefectId");

            migrationBuilder.AddColumn<int>(
                name: "StageId",
                table: "Defects",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDefects",
                table: "ProductDefects",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_StageId",
                table: "Defects",
                column: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyLine_Stage_ProductId",
                table: "AssemblyLine",
                column: "ProductId",
                principalTable: "Stage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_Stage_StageId",
                table: "Defects",
                column: "StageId",
                principalTable: "Stage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDefects_Defects_DefectId",
                table: "ProductDefects",
                column: "DefectId",
                principalTable: "Defects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDefects_Stage_StageId",
                table: "ProductDefects",
                column: "StageId",
                principalTable: "Stage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
