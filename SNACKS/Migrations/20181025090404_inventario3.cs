using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class inventario3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Producto_IdCategoria",
                table: "Producto");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdCategoria",
                table: "Producto",
                column: "IdCategoria",
                unique: false,
                filter: "[IdCategoria] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Producto_IdCategoria",
                table: "Producto");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdCategoria",
                table: "Producto",
                column: "IdCategoria");
        }
    }
}
