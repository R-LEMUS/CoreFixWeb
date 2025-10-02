using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFixWeb.Migrations
{
    /// <inheritdoc />
    public partial class ModeloActualizado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reportes_EstadosReportes_ID_estado_reporte",
                table: "Reportes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstadosReportes",
                table: "EstadosReportes");

            migrationBuilder.RenameTable(
                name: "EstadosReportes",
                newName: "Estado_reporte");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estado_reporte",
                table: "Estado_reporte",
                column: "ID_estado_reporte");

            migrationBuilder.AddForeignKey(
                name: "FK_Reportes_Estado_reporte_ID_estado_reporte",
                table: "Reportes",
                column: "ID_estado_reporte",
                principalTable: "Estado_reporte",
                principalColumn: "ID_estado_reporte",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reportes_Estado_reporte_ID_estado_reporte",
                table: "Reportes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estado_reporte",
                table: "Estado_reporte");

            migrationBuilder.RenameTable(
                name: "Estado_reporte",
                newName: "EstadosReportes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstadosReportes",
                table: "EstadosReportes",
                column: "ID_estado_reporte");

            migrationBuilder.AddForeignKey(
                name: "FK_Reportes_EstadosReportes_ID_estado_reporte",
                table: "Reportes",
                column: "ID_estado_reporte",
                principalTable: "EstadosReportes",
                principalColumn: "ID_estado_reporte",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
