using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SNACKS.Migrations
{
    public partial class reporte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reporte",
                columns: table => new
                {
                    IdReporte = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(nullable: true),
                    TipoReporte = table.Column<int>(nullable: false),
                    Flag = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reporte", x => x.IdReporte);
                });

            migrationBuilder.CreateTable(
                name: "ItemReporte",
                columns: table => new
                {
                    IdItemReporte = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdReporte = table.Column<int>(nullable: true),
                    Nombre = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemReporte", x => x.IdItemReporte);
                    table.ForeignKey(
                        name: "FK_ItemReporte_Reporte_IdReporte",
                        column: x => x.IdReporte,
                        principalTable: "Reporte",
                        principalColumn: "IdReporte",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemReporte_IdReporte",
                table: "ItemReporte",
                column: "IdReporte");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemReporte");

            migrationBuilder.DropTable(
                name: "Reporte");
        }
    }
}
