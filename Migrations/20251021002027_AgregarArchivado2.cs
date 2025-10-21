using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFixWeb.Migrations
{
    /// <inheritdoc />
    public partial class AgregarArchivado2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Archivados_Reportes_ReporteID_reporte",
                table: "Archivados");

            migrationBuilder.DropForeignKey(
                name: "FK_Archivados_Usuarios_UsuarioID_usuario",
                table: "Archivados");

            migrationBuilder.DropIndex(
                name: "IX_Archivados_ReporteID_reporte",
                table: "Archivados");

            migrationBuilder.DropIndex(
                name: "IX_Archivados_UsuarioID_usuario",
                table: "Archivados");

            migrationBuilder.DropColumn(
                name: "ReporteID_reporte",
                table: "Archivados");

            migrationBuilder.DropColumn(
                name: "UsuarioID_usuario",
                table: "Archivados");

            migrationBuilder.CreateIndex(
                name: "IX_Archivados_ID_reporte",
                table: "Archivados",
                column: "ID_reporte");

            migrationBuilder.CreateIndex(
                name: "IX_Archivados_ID_usuario",
                table: "Archivados",
                column: "ID_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Archivados_Reportes_ID_reporte",
                table: "Archivados",
                column: "ID_reporte",
                principalTable: "Reportes",
                principalColumn: "ID_reporte",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Archivados_Usuarios_ID_usuario",
                table: "Archivados",
                column: "ID_usuario",
                principalTable: "Usuarios",
                principalColumn: "ID_usuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Archivados_Reportes_ID_reporte",
                table: "Archivados");

            migrationBuilder.DropForeignKey(
                name: "FK_Archivados_Usuarios_ID_usuario",
                table: "Archivados");

            migrationBuilder.DropIndex(
                name: "IX_Archivados_ID_reporte",
                table: "Archivados");

            migrationBuilder.DropIndex(
                name: "IX_Archivados_ID_usuario",
                table: "Archivados");

            migrationBuilder.AddColumn<int>(
                name: "ReporteID_reporte",
                table: "Archivados",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioID_usuario",
                table: "Archivados",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Archivados_ReporteID_reporte",
                table: "Archivados",
                column: "ReporteID_reporte");

            migrationBuilder.CreateIndex(
                name: "IX_Archivados_UsuarioID_usuario",
                table: "Archivados",
                column: "UsuarioID_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Archivados_Reportes_ReporteID_reporte",
                table: "Archivados",
                column: "ReporteID_reporte",
                principalTable: "Reportes",
                principalColumn: "ID_reporte");

            migrationBuilder.AddForeignKey(
                name: "FK_Archivados_Usuarios_UsuarioID_usuario",
                table: "Archivados",
                column: "UsuarioID_usuario",
                principalTable: "Usuarios",
                principalColumn: "ID_usuario");
        }
    }
}
