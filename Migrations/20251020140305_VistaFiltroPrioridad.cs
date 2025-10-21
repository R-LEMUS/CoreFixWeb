using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFixWeb.Migrations
{
    /// <inheritdoc />
    public partial class VistaFiltroPrioridad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prioridad",
                table: "Estado_reporte",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prioridad",
                table: "Estado_reporte");
        }
    }
}
