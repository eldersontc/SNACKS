using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Vendedor_VendedorIdVendedor",
                table: "Cliente");

            migrationBuilder.RenameColumn(
                name: "VendedorIdVendedor",
                table: "Cliente",
                newName: "IdVendedor");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_VendedorIdVendedor",
                table: "Cliente",
                newName: "IX_Cliente_IdVendedor");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Vendedor_IdVendedor",
                table: "Cliente",
                column: "IdVendedor",
                principalTable: "Vendedor",
                principalColumn: "IdVendedor",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Vendedor_IdVendedor",
                table: "Cliente");

            migrationBuilder.RenameColumn(
                name: "IdVendedor",
                table: "Cliente",
                newName: "VendedorIdVendedor");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_IdVendedor",
                table: "Cliente",
                newName: "IX_Cliente_VendedorIdVendedor");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Vendedor_VendedorIdVendedor",
                table: "Cliente",
                column: "VendedorIdVendedor",
                principalTable: "Vendedor",
                principalColumn: "IdVendedor",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
