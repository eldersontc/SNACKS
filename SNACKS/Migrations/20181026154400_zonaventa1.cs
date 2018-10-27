using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class zonaventa1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdZonaVenta",
                table: "Persona",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ZonaVenta",
                columns: table => new
                {
                    IdZonaVenta = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZonaVenta", x => x.IdZonaVenta);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persona_IdZonaVenta",
                table: "Persona",
                column: "IdZonaVenta");

            migrationBuilder.AddForeignKey(
                name: "FK_Persona_ZonaVenta_IdZonaVenta",
                table: "Persona",
                column: "IdZonaVenta",
                principalTable: "ZonaVenta",
                principalColumn: "IdZonaVenta",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persona_ZonaVenta_IdZonaVenta",
                table: "Persona");

            migrationBuilder.DropTable(
                name: "ZonaVenta");

            migrationBuilder.DropIndex(
                name: "IX_Persona_IdZonaVenta",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "IdZonaVenta",
                table: "Persona");
        }
    }
}
