using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFixWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    ID_equipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numero_serie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fabricante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha_fabricacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Fecha_adquisicion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Area_Produccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dimensiones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Peso = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.ID_equipo);
                });

            migrationBuilder.CreateTable(
                name: "EstadosReportes",
                columns: table => new
                {
                    ID_estado_reporte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosReportes", x => x.ID_estado_reporte);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    ID_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Puesto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.ID_usuario);
                });

            migrationBuilder.CreateTable(
                name: "Reportes",
                columns: table => new
                {
                    ID_reporte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_usuario = table.Column<int>(type: "int", nullable: false),
                    ID_equipo = table.Column<int>(type: "int", nullable: false),
                    ID_estado_reporte = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha_reporte = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fecha_cierre = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reportes", x => x.ID_reporte);
                    table.ForeignKey(
                        name: "FK_Reportes_Equipos_ID_equipo",
                        column: x => x.ID_equipo,
                        principalTable: "Equipos",
                        principalColumn: "ID_equipo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reportes_EstadosReportes_ID_estado_reporte",
                        column: x => x.ID_estado_reporte,
                        principalTable: "EstadosReportes",
                        principalColumn: "ID_estado_reporte",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reportes_Usuarios_ID_usuario",
                        column: x => x.ID_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "ID_usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Evidencias",
                columns: table => new
                {
                    ID_evidencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_reporte = table.Column<int>(type: "int", nullable: false),
                    ID_usuario = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Fecha_subida = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidencias", x => x.ID_evidencia);
                    table.ForeignKey(
                        name: "FK_Evidencias_Reportes_ID_reporte",
                        column: x => x.ID_reporte,
                        principalTable: "Reportes",
                        principalColumn: "ID_reporte",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evidencias_Usuarios_ID_usuario",
                        column: x => x.ID_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "ID_usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mantenimientos",
                columns: table => new
                {
                    ID_mantenimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_equipo = table.Column<int>(type: "int", nullable: false),
                    ID_reporte = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha_mantenimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Proximo_mantenimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EquipoID_equipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantenimientos", x => x.ID_mantenimiento);
                    table.ForeignKey(
                        name: "FK_Mantenimientos_Equipos_EquipoID_equipo",
                        column: x => x.EquipoID_equipo,
                        principalTable: "Equipos",
                        principalColumn: "ID_equipo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mantenimientos_Reportes_ID_reporte",
                        column: x => x.ID_reporte,
                        principalTable: "Reportes",
                        principalColumn: "ID_reporte",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evidencias_ID_reporte",
                table: "Evidencias",
                column: "ID_reporte");

            migrationBuilder.CreateIndex(
                name: "IX_Evidencias_ID_usuario",
                table: "Evidencias",
                column: "ID_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_EquipoID_equipo",
                table: "Mantenimientos",
                column: "EquipoID_equipo");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_ID_reporte",
                table: "Mantenimientos",
                column: "ID_reporte");

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_ID_equipo",
                table: "Reportes",
                column: "ID_equipo");

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_ID_estado_reporte",
                table: "Reportes",
                column: "ID_estado_reporte");

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_ID_usuario",
                table: "Reportes",
                column: "ID_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evidencias");

            migrationBuilder.DropTable(
                name: "Mantenimientos");

            migrationBuilder.DropTable(
                name: "Reportes");

            migrationBuilder.DropTable(
                name: "Equipos");

            migrationBuilder.DropTable(
                name: "EstadosReportes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
