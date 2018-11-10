using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class cajas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdAlmacen",
                table: "SalidaProducto",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdAlmacen",
                table: "SalidaInsumo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdAlmacen",
                table: "InventarioProducto",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdAlmacen",
                table: "InventarioInsumo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdAlmacen",
                table: "IngresoProducto",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdAlmacen",
                table: "IngresoInsumo",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Almacen",
                columns: table => new
                {
                    IdAlmacen = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Almacen", x => x.IdAlmacen);
                });

            migrationBuilder.CreateTable(
                name: "Caja",
                columns: table => new
                {
                    IdCaja = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true),
                    Saldo = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caja", x => x.IdCaja);
                });

            migrationBuilder.CreateTable(
                name: "MovimientoCaja",
                columns: table => new
                {
                    IdMovimientoCaja = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdCaja = table.Column<int>(nullable: true),
                    TipoMovimiento = table.Column<string>(nullable: true),
                    IdCajaOrigen = table.Column<int>(nullable: true),
                    IdPedido = table.Column<int>(nullable: true),
                    IdIngresoInsumo = table.Column<int>(nullable: true),
                    Glosa = table.Column<string>(nullable: true),
                    Importe = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoCaja", x => x.IdMovimientoCaja);
                    table.ForeignKey(
                        name: "FK_MovimientoCaja_Caja_IdCaja",
                        column: x => x.IdCaja,
                        principalTable: "Caja",
                        principalColumn: "IdCaja",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalidaProducto_IdAlmacen",
                table: "SalidaProducto",
                column: "IdAlmacen");

            migrationBuilder.CreateIndex(
                name: "IX_SalidaInsumo_IdAlmacen",
                table: "SalidaInsumo",
                column: "IdAlmacen");

            migrationBuilder.CreateIndex(
                name: "IX_IngresoProducto_IdAlmacen",
                table: "IngresoProducto",
                column: "IdAlmacen");

            migrationBuilder.CreateIndex(
                name: "IX_IngresoInsumo_IdAlmacen",
                table: "IngresoInsumo",
                column: "IdAlmacen");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoCaja_IdCaja",
                table: "MovimientoCaja",
                column: "IdCaja");

            migrationBuilder.AddForeignKey(
                name: "FK_IngresoInsumo_Almacen_IdAlmacen",
                table: "IngresoInsumo",
                column: "IdAlmacen",
                principalTable: "Almacen",
                principalColumn: "IdAlmacen",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IngresoProducto_Almacen_IdAlmacen",
                table: "IngresoProducto",
                column: "IdAlmacen",
                principalTable: "Almacen",
                principalColumn: "IdAlmacen",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalidaInsumo_Almacen_IdAlmacen",
                table: "SalidaInsumo",
                column: "IdAlmacen",
                principalTable: "Almacen",
                principalColumn: "IdAlmacen",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalidaProducto_Almacen_IdAlmacen",
                table: "SalidaProducto",
                column: "IdAlmacen",
                principalTable: "Almacen",
                principalColumn: "IdAlmacen",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngresoInsumo_Almacen_IdAlmacen",
                table: "IngresoInsumo");

            migrationBuilder.DropForeignKey(
                name: "FK_IngresoProducto_Almacen_IdAlmacen",
                table: "IngresoProducto");

            migrationBuilder.DropForeignKey(
                name: "FK_SalidaInsumo_Almacen_IdAlmacen",
                table: "SalidaInsumo");

            migrationBuilder.DropForeignKey(
                name: "FK_SalidaProducto_Almacen_IdAlmacen",
                table: "SalidaProducto");

            migrationBuilder.DropTable(
                name: "Almacen");

            migrationBuilder.DropTable(
                name: "MovimientoCaja");

            migrationBuilder.DropTable(
                name: "Caja");

            migrationBuilder.DropIndex(
                name: "IX_SalidaProducto_IdAlmacen",
                table: "SalidaProducto");

            migrationBuilder.DropIndex(
                name: "IX_SalidaInsumo_IdAlmacen",
                table: "SalidaInsumo");

            migrationBuilder.DropIndex(
                name: "IX_IngresoProducto_IdAlmacen",
                table: "IngresoProducto");

            migrationBuilder.DropIndex(
                name: "IX_IngresoInsumo_IdAlmacen",
                table: "IngresoInsumo");

            migrationBuilder.DropColumn(
                name: "IdAlmacen",
                table: "SalidaProducto");

            migrationBuilder.DropColumn(
                name: "IdAlmacen",
                table: "SalidaInsumo");

            migrationBuilder.DropColumn(
                name: "IdAlmacen",
                table: "InventarioProducto");

            migrationBuilder.DropColumn(
                name: "IdAlmacen",
                table: "InventarioInsumo");

            migrationBuilder.DropColumn(
                name: "IdAlmacen",
                table: "IngresoProducto");

            migrationBuilder.DropColumn(
                name: "IdAlmacen",
                table: "IngresoInsumo");
        }
    }
}
