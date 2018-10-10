using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class _11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngresoInsumo");

            migrationBuilder.AlterColumn<int>(
                name: "TipoDocumento",
                table: "Persona",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TipoDocumento",
                table: "Persona",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "IngresoInsumo",
                columns: table => new
                {
                    IdIngresoInsumo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    IdEmpleado = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngresoInsumo", x => x.IdIngresoInsumo);
                    table.ForeignKey(
                        name: "FK_IngresoInsumo_Persona_IdEmpleado",
                        column: x => x.IdEmpleado,
                        principalTable: "Persona",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngresoInsumo_IdEmpleado",
                table: "IngresoInsumo",
                column: "IdEmpleado");
        }
    }
}
