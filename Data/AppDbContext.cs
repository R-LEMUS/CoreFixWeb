using Microsoft.EntityFrameworkCore;
using System;

namespace CoreFixWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Estado_reporte> EstadosReportes { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
        public DbSet<Evidencia> Evidencias { get; set; }
        public DbSet<Mantenimiento> Mantenimientos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reportes)
                .HasForeignKey(r => r.ID_usuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.Equipo)
                .WithMany(e => e.Reportes)
                .HasForeignKey(r => r.ID_equipo);

            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.EstadoReporte)
                .WithMany(er => er.Reportes)
                .HasForeignKey(r => r.ID_estado_reporte);

            modelBuilder.Entity<Evidencia>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Evidencias)
                .HasForeignKey(e => e.ID_usuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Evidencia>()
                .HasOne(e => e.Reporte)
                .WithMany(r => r.Evidencias)
                .HasForeignKey(e => e.ID_reporte)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Mantenimiento>()
                .HasOne(m => m.Reporte)
                .WithMany(r => r.Mantenimientos)
                .HasForeignKey(m => m.ID_reporte)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}