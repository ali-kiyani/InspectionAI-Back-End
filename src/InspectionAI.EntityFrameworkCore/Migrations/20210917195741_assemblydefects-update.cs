using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InspectionAI.Migrations
{
    public partial class assemblydefectsupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DetectionTime",
                table: "AssemblyDefects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "StageId",
                table: "AssemblyDefects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyDefects_StageId",
                table: "AssemblyDefects",
                column: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssemblyDefects_Stage_StageId",
                table: "AssemblyDefects",
                column: "StageId",
                principalTable: "Stage",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssemblyDefects_Stage_StageId",
                table: "AssemblyDefects");

            migrationBuilder.DropIndex(
                name: "IX_AssemblyDefects_StageId",
                table: "AssemblyDefects");

            migrationBuilder.DropColumn(
                name: "DetectionTime",
                table: "AssemblyDefects");

            migrationBuilder.DropColumn(
                name: "StageId",
                table: "AssemblyDefects");
        }
    }
}
