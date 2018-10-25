using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class inventario9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Factor",
                table: "InventarioInsumo");

            migrationBuilder.DropColumn(
                name: "IdUnidad",
                table: "InventarioInsumo");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Pedido",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEntrega",
                table: "Pedido",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPago",
                table: "Pedido",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPropuesta",
                table: "Pedido",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Pago",
                table: "Pedido",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Pedido",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "ItemPedido",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "InventarioProducto",
                columns: table => new
                {
                    IdInventarioProducto = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdProducto = table.Column<int>(nullable: false),
                    Stock = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventarioProducto", x => x.IdInventarioProducto);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventarioProducto");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "FechaEntrega",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "FechaPago",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "FechaPropuesta",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "Pago",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "ItemPedido");

            migrationBuilder.AddColumn<int>(
                name: "Factor",
                table: "InventarioInsumo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUnidad",
                table: "InventarioInsumo",
                nullable: false,
                defaultValue: 0);
        }
    }
}
