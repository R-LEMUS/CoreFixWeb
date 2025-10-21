using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFixWeb.Migrations
{
    /// <inheritdoc />
    public partial class AgregarArchivado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Archivados",
                columns: table => new
                {
                    ID_archivado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_reporte = table.Column<int>(type: "int", nullable: false),
                    ID_usuario = table.Column<int>(type: "int", nullable: false),
                    Fecha_archivado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReporteID_reporte = table.Column<int>(type: "int", nullable: true),
                    UsuarioID_usuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archivados", x => x.ID_archivado);
                    table.ForeignKey(
                        name: "FK_Archivados_Reportes_ReporteID_reporte",
                        column: x => x.ReporteID_reporte,
                        principalTable: "Reportes",
                        principalColumn: "ID_reporte");
                    table.ForeignKey(
                        name: "FK_Archivados_Usuarios_UsuarioID_usuario",
                        column: x => x.UsuarioID_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "ID_usuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Archivados_ReporteID_reporte",
                table: "Archivados",
                column: "ReporteID_reporte");

            migrationBuilder.CreateIndex(
                name: "IX_Archivados_UsuarioID_usuario",
                table: "Archivados",
                column: "UsuarioID_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Archivados");
        }
    }
}
