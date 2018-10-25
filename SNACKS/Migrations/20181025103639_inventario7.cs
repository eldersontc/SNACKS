using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class inventario7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Persona_IdPersona",
                table: "Usuario");

            migrationBuilder.AlterColumn<int>(
                name: "IdPersona",
                table: "Usuario",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Persona_IdPersona",
                table: "Usuario",
                column: "IdPersona",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Persona_IdPersona",
                table: "Usuario");

            migrationBuilder.AlterColumn<int>(
                name: "IdPersona",
                table: "Usuario",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Persona_IdPersona",
                table: "Usuario",
                column: "IdPersona",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
