using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class zonaventa2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdVendedor",
                table: "Persona",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persona_IdVendedor",
                table: "Persona",
                column: "IdVendedor");

            migrationBuilder.AddForeignKey(
                name: "FK_Persona_Persona_IdVendedor",
                table: "Persona",
                column: "IdVendedor",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persona_Persona_IdVendedor",
                table: "Persona");

            migrationBuilder.DropIndex(
                name: "IX_Persona_IdVendedor",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "IdVendedor",
                table: "Persona");
        }
    }
}
