using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFixWeb.Migrations
{
    /// <inheritdoc />
    public partial class ChangesApllied : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID_tecnico_asignado",
                table: "Reportes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TecnicoAsignadoID_usuario",
                table: "Reportes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_TecnicoAsignadoID_usuario",
                table: "Reportes",
                column: "TecnicoAsignadoID_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Reportes_Usuarios_TecnicoAsignadoID_usuario",
                table: "Reportes",
                column: "TecnicoAsignadoID_usuario",
                principalTable: "Usuarios",
                principalColumn: "ID_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reportes_Usuarios_TecnicoAsignadoID_usuario",
                table: "Reportes");

            migrationBuilder.DropIndex(
                name: "IX_Reportes_TecnicoAsignadoID_usuario",
                table: "Reportes");

            migrationBuilder.DropColumn(
                name: "ID_tecnico_asignado",
                table: "Reportes");

            migrationBuilder.DropColumn(
                name: "TecnicoAsignadoID_usuario",
                table: "Reportes");
        }
    }
}
