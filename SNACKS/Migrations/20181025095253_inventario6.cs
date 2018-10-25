using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class inventario6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Persona_PersonaIdPersona",
                table: "Usuario");

            migrationBuilder.RenameColumn(
                name: "PersonaIdPersona",
                table: "Usuario",
                newName: "IdPersona");

            migrationBuilder.RenameIndex(
                name: "IX_Usuario_PersonaIdPersona",
                table: "Usuario",
                newName: "IX_Usuario_IdPersona");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Persona_IdPersona",
                table: "Usuario",
                column: "IdPersona",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Persona_IdPersona",
                table: "Usuario");

            migrationBuilder.RenameColumn(
                name: "IdPersona",
                table: "Usuario",
                newName: "PersonaIdPersona");

            migrationBuilder.RenameIndex(
                name: "IX_Usuario_IdPersona",
                table: "Usuario",
                newName: "IX_Usuario_PersonaIdPersona");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Persona_PersonaIdPersona",
                table: "Usuario",
                column: "PersonaIdPersona",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
