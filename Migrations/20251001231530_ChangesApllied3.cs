using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFixWeb.Migrations
{
    /// <inheritdoc />
    public partial class ChangesApllied3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "Evidencias");

            migrationBuilder.AddColumn<string>(
                name: "Ruta",
                table: "Evidencias",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ruta",
                table: "Evidencias");

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen",
                table: "Evidencias",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
