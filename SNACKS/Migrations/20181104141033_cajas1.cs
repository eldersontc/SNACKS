using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class cajas1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCaja",
                table: "IngresoInsumo",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IngresoInsumo_IdCaja",
                table: "IngresoInsumo",
                column: "IdCaja");

            migrationBuilder.AddForeignKey(
                name: "FK_IngresoInsumo_Caja_IdCaja",
                table: "IngresoInsumo",
                column: "IdCaja",
                principalTable: "Caja",
                principalColumn: "IdCaja",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngresoInsumo_Caja_IdCaja",
                table: "IngresoInsumo");

            migrationBuilder.DropIndex(
                name: "IX_IngresoInsumo_IdCaja",
                table: "IngresoInsumo");

            migrationBuilder.DropColumn(
                name: "IdCaja",
                table: "IngresoInsumo");
        }
    }
}
