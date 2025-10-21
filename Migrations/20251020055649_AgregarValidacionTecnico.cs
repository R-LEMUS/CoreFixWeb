using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFixWeb.Migrations
{
    /// <inheritdoc />
    public partial class AgregarValidacionTecnico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reportes_Usuarios_TecnicoAsignadoID_usuario",
                table: "Reportes");

            migrationBuilder.RenameColumn(
                name: "TecnicoAsignadoID_usuario",
                table: "Reportes",
                newName: "TecnicoValidadorID_usuario");

            migrationBuilder.RenameIndex(
                name: "IX_Reportes_TecnicoAsignadoID_usuario",
                table: "Reportes",
                newName: "IX_Reportes_TecnicoValidadorID_usuario");

            migrationBuilder.AddColumn<int>(
                name: "ID_tecnico_validador",
                table: "Reportes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_ID_tecnico_asignado",
                table: "Reportes",
                column: "ID_tecnico_asignado");

            migrationBuilder.AddForeignKey(
                name: "FK_Reportes_Usuarios_ID_tecnico_asignado",
                table: "Reportes",
                column: "ID_tecnico_asignado",
                principalTable: "Usuarios",
                principalColumn: "ID_usuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reportes_Usuarios_TecnicoValidadorID_usuario",
                table: "Reportes",
                column: "TecnicoValidadorID_usuario",
                principalTable: "Usuarios",
                principalColumn: "ID_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reportes_Usuarios_ID_tecnico_asignado",
                table: "Reportes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reportes_Usuarios_TecnicoValidadorID_usuario",
                table: "Reportes");

            migrationBuilder.DropIndex(
                name: "IX_Reportes_ID_tecnico_asignado",
                table: "Reportes");

            migrationBuilder.DropColumn(
                name: "ID_tecnico_validador",
                table: "Reportes");

            migrationBuilder.RenameColumn(
                name: "TecnicoValidadorID_usuario",
                table: "Reportes",
                newName: "TecnicoAsignadoID_usuario");

            migrationBuilder.RenameIndex(
                name: "IX_Reportes_TecnicoValidadorID_usuario",
                table: "Reportes",
                newName: "IX_Reportes_TecnicoAsignadoID_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Reportes_Usuarios_TecnicoAsignadoID_usuario",
                table: "Reportes",
                column: "TecnicoAsignadoID_usuario",
                principalTable: "Usuarios",
                principalColumn: "ID_usuario");
        }
    }
}
