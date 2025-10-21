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
        public DbSet<Archivado> Archivados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reportes)
                .HasForeignKey(r => r.ID_usuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.TecnicoAsignado)
                .WithMany()
                .HasForeignKey(r => r.ID_tecnico_asignado)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reporte>()
                .Property(r => r.ID_reporte)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Reporte>()
                .Property(r => r.Numero_Reporte)
                .IsRequired();

            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.Equipo)
                .WithMany(e => e.Reportes)
                .HasForeignKey(r => r.ID_equipo);

            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.EstadoReporte)
                .WithMany(er => er.Reportes)
                .HasForeignKey(r => r.ID_estado_reporte);

            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.SupervisorValidador)
                .WithMany()
                .HasForeignKey(r => r.ID_supervisor_validador)
                .OnDelete(DeleteBehavior.Restrict);

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

            modelBuilder.Entity<Evidencia>()
                .Property(e => e.ID_evidencia)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Evidencia>()
                .Property(e => e.Numero_Evidencia)
                .IsRequired();

            modelBuilder.Entity<Mantenimiento>()
                .HasOne(m => m.Reporte)
                .WithMany(r => r.Mantenimientos)
                .HasForeignKey(m => m.ID_reporte)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Estado_reporte>()
                .ToTable("Estado_reporte");

            modelBuilder.Entity<Archivado>()
                .HasOne(a => a.Reporte)
                .WithMany()
                .HasForeignKey(a => a.ID_reporte)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Archivado>()
                .HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.ID_usuario)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}