using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFixWeb.Migrations
{
    /// <inheritdoc />
    public partial class ModeloActualizado2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID_supervisor_validador",
                table: "Reportes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_ID_supervisor_validador",
                table: "Reportes",
                column: "ID_supervisor_validador");

            migrationBuilder.AddForeignKey(
                name: "FK_Reportes_Usuarios_ID_supervisor_validador",
                table: "Reportes",
                column: "ID_supervisor_validador",
                principalTable: "Usuarios",
                principalColumn: "ID_usuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reportes_Usuarios_ID_supervisor_validador",
                table: "Reportes");

            migrationBuilder.DropIndex(
                name: "IX_Reportes_ID_supervisor_validador",
                table: "Reportes");

            migrationBuilder.DropColumn(
                name: "ID_supervisor_validador",
                table: "Reportes");
        }
    }
}
