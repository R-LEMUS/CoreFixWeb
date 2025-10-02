using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CoreFixWeb.Data
{
    public class Usuario
    {
        [Key]
        public int ID_usuario { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string? Contraseña { get; set; }
        public string? Puesto { get; set; }

        // Relación con Reporte
        public ICollection<Reporte>? Reportes { get; set; }

        // Relación con Evidencia
        public ICollection<Evidencia>? Evidencias { get; set; }
    }

    public class Equipo
    {
        [Key]
        public int ID_equipo { get; set; }
        public string? Nombre { get; set; }
        public string? Ubicacion { get; set; }
        public string? Descripcion { get; set; }
        public string? Numero_serie { get; set; }
        public string? Modelo { get; set; }
        public string? Marca { get; set; }
        public string? Fabricante { get; set; }
        public DateTime? Fecha_fabricacion { get; set; }
        public DateTime? Fecha_adquisicion { get; set; }
        public string? Area_Produccion { get; set; }
        public string? Dimensiones { get; set; }
        public int? Peso { get; set; }

        // Relación con Reporte
        public ICollection<Reporte>? Reportes { get; set; }

        // Relación con Mantenimiento
        public ICollection<Mantenimiento>? Mantenimientos { get; set; }
    }

    public class Estado_reporte
    {
        [Key]
        public int ID_estado_reporte { get; set; }
        public string? Estado { get; set; }

        // Relación con Reporte
        public ICollection<Reporte>? Reportes { get; set; }
    }

    public class Reporte
    {
        [Key]
        public int ID_reporte { get; set; }
        public int ID_usuario { get; set; }
        public int ID_equipo { get; set; }
        public int ID_estado_reporte { get; set; }
        public string? Descripcion { get; set; }
        public DateTime Fecha_reporte { get; set; }
        public DateTime? Fecha_cierre { get; set; }

        public int Numero_Reporte { get; set; }

        // Relaciones
        public Usuario? Usuario { get; set; }
        public Equipo? Equipo { get; set; }
        public Estado_reporte? EstadoReporte { get; set; }
        public int? ID_tecnico_asignado { get; set; }
        public Usuario? TecnicoAsignado { get; set; }

        public int? ID_supervisor_validador { get; set; }
        public Usuario? SupervisorValidador { get; set; }

        // Relación con Evidencia
        public ICollection<Evidencia>? Evidencias { get; set; }
        public ICollection<Mantenimiento>? Mantenimientos { get; set; }
    }

    public class Evidencia
    {
        [Key]
        public int ID_evidencia { get; set; }
        public int ID_reporte { get; set; }
        public int ID_usuario { get; set; }
        public string? Descripcion { get; set; }
        public string? Ruta { get; set; }
        public DateTime Fecha_subida { get; set; }

        public int Numero_Evidencia { get; set; }

        // Relaciones
        public Reporte? Reporte { get; set; }
        public Usuario? Usuario { get; set; }
    }

    public class Mantenimiento
    {
        [Key]
        public int ID_mantenimiento { get; set; }
        public int ID_equipo { get; set; }
        public int? ID_reporte { get; set; }
        public string? Tipo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime Fecha_mantenimiento { get; set; }
        public DateTime? Proximo_mantenimiento { get; set; }

        // Relaciones
        public Equipo? Equipo { get; set; }
        public Reporte? Reporte { get; set; }
    }
}