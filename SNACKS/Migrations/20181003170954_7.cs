using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Empleado_IdVendedor",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Empleado");

            migrationBuilder.RenameColumn(
                name: "IdVendedor",
                table: "Cliente",
                newName: "IdEmpleado");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_IdVendedor",
                table: "Cliente",
                newName: "IX_Cliente_IdEmpleado");

            migrationBuilder.AddColumn<int>(
                name: "IdEmpleado",
                table: "Pedido",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Factor",
                table: "ItemPedido",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdProducto",
                table: "ItemPedido",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdUnidad",
                table: "ItemPedido",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoEmpleado",
                table: "Empleado",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "IngresoInsumo",
                columns: table => new
                {
                    IdIngresoInsumo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdEmpleado = table.Column<int>(nullable: true),
                    FechaCreacion = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngresoInsumo", x => x.IdIngresoInsumo);
                    table.ForeignKey(
                        name: "FK_IngresoInsumo_Empleado_IdEmpleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Empleado",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    IdProducto = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true),
                    EsInsumo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.IdProducto);
                });

            migrationBuilder.CreateTable(
                name: "Unidad",
                columns: table => new
                {
                    IdUnidad = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true),
                    Abreviacion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidad", x => x.IdUnidad);
                });

            migrationBuilder.CreateTable(
                name: "ItemProducto",
                columns: table => new
                {
                    IdItemProducto = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdProducto = table.Column<int>(nullable: true),
                    IdUnidad = table.Column<int>(nullable: true),
                    Factor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemProducto", x => x.IdItemProducto);
                    table.ForeignKey(
                        name: "FK_ItemProducto_Producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemProducto_Unidad_IdUnidad",
                        column: x => x.IdUnidad,
                        principalTable: "Unidad",
                        principalColumn: "IdUnidad",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_IdEmpleado",
                table: "Pedido",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPedido_IdProducto",
                table: "ItemPedido",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPedido_IdUnidad",
                table: "ItemPedido",
                column: "IdUnidad");

            migrationBuilder.CreateIndex(
                name: "IX_IngresoInsumo_IdEmpleado",
                table: "IngresoInsumo",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_ItemProducto_IdProducto",
                table: "ItemProducto",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ItemProducto_IdUnidad",
                table: "ItemProducto",
                column: "IdUnidad");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Empleado_IdEmpleado",
                table: "Cliente",
                column: "IdEmpleado",
                principalTable: "Empleado",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedido_Producto_IdProducto",
                table: "ItemPedido",
                column: "IdProducto",
                principalTable: "Producto",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedido_Unidad_IdUnidad",
                table: "ItemPedido",
                column: "IdUnidad",
                principalTable: "Unidad",
                principalColumn: "IdUnidad",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Empleado_IdEmpleado",
                table: "Pedido",
                column: "IdEmpleado",
                principalTable: "Empleado",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Empleado_IdEmpleado",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedido_Producto_IdProducto",
                table: "ItemPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedido_Unidad_IdUnidad",
                table: "ItemPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Empleado_IdEmpleado",
                table: "Pedido");

            migrationBuilder.DropTable(
                name: "IngresoInsumo");

            migrationBuilder.DropTable(
                name: "ItemProducto");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Unidad");

            migrationBuilder.DropIndex(
                name: "IX_Pedido_IdEmpleado",
                table: "Pedido");

            migrationBuilder.DropIndex(
                name: "IX_ItemPedido_IdProducto",
                table: "ItemPedido");

            migrationBuilder.DropIndex(
                name: "IX_ItemPedido_IdUnidad",
                table: "ItemPedido");

            migrationBuilder.DropColumn(
                name: "IdEmpleado",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "Factor",
                table: "ItemPedido");

            migrationBuilder.DropColumn(
                name: "IdProducto",
                table: "ItemPedido");

            migrationBuilder.DropColumn(
                name: "IdUnidad",
                table: "ItemPedido");

            migrationBuilder.DropColumn(
                name: "TipoEmpleado",
                table: "Empleado");

            migrationBuilder.RenameColumn(
                name: "IdEmpleado",
                table: "Cliente",
                newName: "IdVendedor");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_IdEmpleado",
                table: "Cliente",
                newName: "IX_Cliente_IdVendedor");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Empleado",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Empleado_IdVendedor",
                table: "Cliente",
                column: "IdVendedor",
                principalTable: "Empleado",
                principalColumn: "IdEmpleado",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
