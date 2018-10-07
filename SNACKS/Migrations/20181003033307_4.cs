using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendedorIdVendedor",
                table: "Cliente",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Vendedor",
                columns: table => new
                {
                    IdVendedor = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombres = table.Column<string>(nullable: true),
                    Apellidos = table.Column<string>(nullable: true),
                    TipoDocumento = table.Column<string>(nullable: true),
                    NumeroDocumento = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendedor", x => x.IdVendedor);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_VendedorIdVendedor",
                table: "Cliente",
                column: "VendedorIdVendedor");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Vendedor_VendedorIdVendedor",
                table: "Cliente",
                column: "VendedorIdVendedor",
                principalTable: "Vendedor",
                principalColumn: "IdVendedor",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Vendedor_VendedorIdVendedor",
                table: "Cliente");

            migrationBuilder.DropTable(
                name: "Vendedor");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_VendedorIdVendedor",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "VendedorIdVendedor",
                table: "Cliente");
        }
    }
}
