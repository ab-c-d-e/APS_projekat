using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheScientistAPI.Migrations
{
    public partial class M4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_ScientificPapers_PaperId",
                table: "Sections");

            migrationBuilder.AlterColumn<int>(
                name: "PaperId",
                table: "Sections",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_ScientificPapers_PaperId",
                table: "Sections",
                column: "PaperId",
                principalTable: "ScientificPapers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_ScientificPapers_PaperId",
                table: "Sections");

            migrationBuilder.AlterColumn<int>(
                name: "PaperId",
                table: "Sections",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_ScientificPapers_PaperId",
                table: "Sections",
                column: "PaperId",
                principalTable: "ScientificPapers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
