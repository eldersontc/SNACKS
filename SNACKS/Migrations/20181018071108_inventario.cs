using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class inventario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCategoria",
                table: "Producto",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comentario",
                table: "Pedido",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "IngresoInsumo",
                columns: table => new
                {
                    IdIngresoInsumo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(nullable: true),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    Comentario = table.Column<string>(nullable: true),
                    Costo = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngresoInsumo", x => x.IdIngresoInsumo);
                    table.ForeignKey(
                        name: "FK_IngresoInsumo_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IngresoProducto",
                columns: table => new
                {
                    IdIngresoProducto = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(nullable: true),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    Comentario = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngresoProducto", x => x.IdIngresoProducto);
                    table.ForeignKey(
                        name: "FK_IngresoProducto_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalidaInsumo",
                columns: table => new
                {
                    IdSalidaInsumo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(nullable: true),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    Comentario = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalidaInsumo", x => x.IdSalidaInsumo);
                    table.ForeignKey(
                        name: "FK_SalidaInsumo_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalidaProducto",
                columns: table => new
                {
                    IdSalidaProducto = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(nullable: true),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    Comentario = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalidaProducto", x => x.IdSalidaProducto);
                    table.ForeignKey(
                        name: "FK_SalidaProducto_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemIngresoInsumo",
                columns: table => new
                {
                    IdItemIngresoInsumo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdIngresoInsumo = table.Column<int>(nullable: true),
                    IdProducto = table.Column<int>(nullable: true),
                    IdUnidad = table.Column<int>(nullable: true),
                    Factor = table.Column<int>(nullable: false),
                    Cantidad = table.Column<int>(nullable: false),
                    Costo = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemIngresoInsumo", x => x.IdItemIngresoInsumo);
                    table.ForeignKey(
                        name: "FK_ItemIngresoInsumo_IngresoInsumo_IdIngresoInsumo",
                        column: x => x.IdIngresoInsumo,
                        principalTable: "IngresoInsumo",
                        principalColumn: "IdIngresoInsumo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemIngresoInsumo_Producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemIngresoInsumo_Unidad_IdUnidad",
                        column: x => x.IdUnidad,
                        principalTable: "Unidad",
                        principalColumn: "IdUnidad",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemIngresoProducto",
                columns: table => new
                {
                    IdItemIngresoProducto = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdIngresoProducto = table.Column<int>(nullable: true),
                    IdProducto = table.Column<int>(nullable: true),
                    IdUnidad = table.Column<int>(nullable: true),
                    Factor = table.Column<int>(nullable: false),
                    Cantidad = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemIngresoProducto", x => x.IdItemIngresoProducto);
                    table.ForeignKey(
                        name: "FK_ItemIngresoProducto_IngresoProducto_IdIngresoProducto",
                        column: x => x.IdIngresoProducto,
                        principalTable: "IngresoProducto",
                        principalColumn: "IdIngresoProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemIngresoProducto_Producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemIngresoProducto_Unidad_IdUnidad",
                        column: x => x.IdUnidad,
                        principalTable: "Unidad",
                        principalColumn: "IdUnidad",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemSalidaInsumo",
                columns: table => new
                {
                    IdItemSalidaInsumo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdSalidaInsumo = table.Column<int>(nullable: true),
                    IdProducto = table.Column<int>(nullable: true),
                    IdUnidad = table.Column<int>(nullable: true),
                    Factor = table.Column<int>(nullable: false),
                    Cantidad = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSalidaInsumo", x => x.IdItemSalidaInsumo);
                    table.ForeignKey(
                        name: "FK_ItemSalidaInsumo_Producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemSalidaInsumo_SalidaInsumo_IdSalidaInsumo",
                        column: x => x.IdSalidaInsumo,
                        principalTable: "SalidaInsumo",
                        principalColumn: "IdSalidaInsumo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemSalidaInsumo_Unidad_IdUnidad",
                        column: x => x.IdUnidad,
                        principalTable: "Unidad",
                        principalColumn: "IdUnidad",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemSalidaProducto",
                columns: table => new
                {
                    IdItemSalidaProducto = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdSalidaProducto = table.Column<int>(nullable: true),
                    IdProducto = table.Column<int>(nullable: true),
                    IdUnidad = table.Column<int>(nullable: true),
                    Factor = table.Column<int>(nullable: false),
                    Cantidad = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSalidaProducto", x => x.IdItemSalidaProducto);
                    table.ForeignKey(
                        name: "FK_ItemSalidaProducto_Producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemSalidaProducto_SalidaProducto_IdSalidaProducto",
                        column: x => x.IdSalidaProducto,
                        principalTable: "SalidaProducto",
                        principalColumn: "IdSalidaProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemSalidaProducto_Unidad_IdUnidad",
                        column: x => x.IdUnidad,
                        principalTable: "Unidad",
                        principalColumn: "IdUnidad",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdCategoria",
                table: "Producto",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_IngresoInsumo_IdUsuario",
                table: "IngresoInsumo",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_IngresoProducto_IdUsuario",
                table: "IngresoProducto",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ItemIngresoInsumo_IdIngresoInsumo",
                table: "ItemIngresoInsumo",
                column: "IdIngresoInsumo");

            migrationBuilder.CreateIndex(
                name: "IX_ItemIngresoInsumo_IdProducto",
                table: "ItemIngresoInsumo",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ItemIngresoInsumo_IdUnidad",
                table: "ItemIngresoInsumo",
                column: "IdUnidad");

            migrationBuilder.CreateIndex(
                name: "IX_ItemIngresoProducto_IdIngresoProducto",
                table: "ItemIngresoProducto",
                column: "IdIngresoProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ItemIngresoProducto_IdProducto",
                table: "ItemIngresoProducto",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ItemIngresoProducto_IdUnidad",
                table: "ItemIngresoProducto",
                column: "IdUnidad");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSalidaInsumo_IdProducto",
                table: "ItemSalidaInsumo",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSalidaInsumo_IdSalidaInsumo",
                table: "ItemSalidaInsumo",
                column: "IdSalidaInsumo");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSalidaInsumo_IdUnidad",
                table: "ItemSalidaInsumo",
                column: "IdUnidad");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSalidaProducto_IdProducto",
                table: "ItemSalidaProducto",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSalidaProducto_IdSalidaProducto",
                table: "ItemSalidaProducto",
                column: "IdSalidaProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSalidaProducto_IdUnidad",
                table: "ItemSalidaProducto",
                column: "IdUnidad");

            migrationBuilder.CreateIndex(
                name: "IX_SalidaInsumo_IdUsuario",
                table: "SalidaInsumo",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_SalidaProducto_IdUsuario",
                table: "SalidaProducto",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Categoria_IdCategoria",
                table: "Producto",
                column: "IdCategoria",
                principalTable: "Categoria",
                principalColumn: "IdCategoria",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Categoria_IdCategoria",
                table: "Producto");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "ItemIngresoInsumo");

            migrationBuilder.DropTable(
                name: "ItemIngresoProducto");

            migrationBuilder.DropTable(
                name: "ItemSalidaInsumo");

            migrationBuilder.DropTable(
                name: "ItemSalidaProducto");

            migrationBuilder.DropTable(
                name: "IngresoInsumo");

            migrationBuilder.DropTable(
                name: "IngresoProducto");

            migrationBuilder.DropTable(
                name: "SalidaInsumo");

            migrationBuilder.DropTable(
                name: "SalidaProducto");

            migrationBuilder.DropIndex(
                name: "IX_Producto_IdCategoria",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "IdCategoria",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "Comentario",
                table: "Pedido");
        }
    }
}
