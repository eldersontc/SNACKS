using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class inventario8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Persona_IdPersona",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Producto_IdCategoria",
                table: "Producto");

            migrationBuilder.AlterColumn<int>(
                name: "IdPersona",
                table: "Usuario",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdCategoria",
                table: "Producto",
                column: "IdCategoria");

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

            migrationBuilder.DropIndex(
                name: "IX_Producto_IdCategoria",
                table: "Producto");

            migrationBuilder.AlterColumn<int>(
                name: "IdPersona",
                table: "Usuario",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdCategoria",
                table: "Producto",
                column: "IdCategoria",
                unique: true,
                filter: "[IdCategoria] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Persona_IdPersona",
                table: "Usuario",
                column: "IdPersona",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
