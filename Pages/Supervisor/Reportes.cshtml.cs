using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreFixWeb.Data;

namespace CoreFixWeb.Pages.Supervisor
{
    [Authorize(Roles = "Supervisor")]
    public class ReportesModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ReportesModel(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public List<Reporte> Reportes { get; set; } = new();
        public List<Estado_reporte> Estados { get; set; } = new();
        public List<string> Prioridades { get; set; } = new() { "Critica", "Alta", "Media", "Baja" };

        [BindProperty(SupportsGet = true)]
        public int? EstadoFiltro { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? PrioridadFiltro { get; set; }

        [BindProperty(SupportsGet = true)]
        public string OrdenarPor { get; set; } = "FechaDesc";

        [BindProperty(SupportsGet = true)]
        public string Busqueda { get; set; } = "";

        [BindProperty(SupportsGet = true)] public bool VerArchivados { get; set; } = false;


        public async Task OnGetAsync()
        {
            var query = _context.Reportes
                .Include(r => r.Usuario)
                .Include(r => r.Equipo)
                .Include(r => r.EstadoReporte)
                .Include(r => r.Evidencias)
                .AsQueryable();

            if (!string.IsNullOrEmpty(Busqueda))
            {
                query = query.Where(r =>
                    r.Descripcion.Contains(Busqueda) ||
                    r.Numero_Reporte.ToString() == Busqueda);
            }
            
            if (VerArchivados)
            {
                var usuarioId = int.Parse(User.FindFirst("ID_usuario").Value);
                var archivadosIds = await _context.Archivados
                    .Where(a => a.ID_usuario == usuarioId)
                    .Select(a => a.ID_reporte)
                    .ToListAsync();

                query = query.Where(r => archivadosIds.Contains(r.ID_reporte));
            }
            else
            {
                var usuarioId = int.Parse(User.FindFirst("ID_usuario").Value);
                var archivadosIds = await _context.Archivados
                    .Where(a => a.ID_usuario == usuarioId)
                    .Select(a => a.ID_reporte)
                    .ToListAsync();

                query = query.Where(r => !archivadosIds.Contains(r.ID_reporte));
            }

            if (EstadoFiltro.HasValue)
            {
                query = query.Where(r => r.ID_estado_reporte == EstadoFiltro.Value);
            }

            if (!string.IsNullOrEmpty(PrioridadFiltro))
            {
                query = query.Where(r => r.Prioridad == PrioridadFiltro);
            }

            query = OrdenarPor switch
            {
                "FolioAsc" => query.OrderBy(r => r.Numero_Reporte),
                "FolioDesc" => query.OrderByDescending(r => r.Numero_Reporte),
                "FechaAsc" => query.OrderBy(r => r.Fecha_reporte),
                _ => query.OrderByDescending(r => r.Fecha_reporte)
            };

            Reportes = await query.ToListAsync();
            Estados = await _context.EstadosReportes.ToListAsync();
        }

        public async Task<IActionResult> OnPostValidar(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);
            if (reporte == null || reporte.ID_estado_reporte != 1)
            {
                TempData["MensajeError"] = "No se puede validar este reporte";
                return RedirectToPage();
            }

            var idSupervisor = int.Parse(User.Claims.First(c => c.Type == "ID_usuario").Value);
            reporte.ID_estado_reporte = 2;
            reporte.ID_supervisor_validador = idSupervisor;
            _context.Reportes.Update(reporte);
            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = "Reporte validado";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRechazar(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);
            if (reporte == null || reporte.ID_estado_reporte != 1)
            {
                TempData["MensajeError"] = "No se puede rechazar este reporte";
                return RedirectToPage();
            }

            reporte.ID_estado_reporte = 7;
            _context.Reportes.Update(reporte);
            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = "Reporte rechazado";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminar(int id)
        {
            var reporte = await _context.Reportes
                .Include(r => r.Evidencias)
                .FirstOrDefaultAsync(r => r.ID_reporte == id);

            if (reporte == null)
            {
                TempData["MensajeError"] = "Reporte no encontrado";
                return RedirectToPage();
            }

            if (reporte.ID_estado_reporte != 5 && reporte.ID_estado_reporte != 6)
            {
                TempData["MensajeError"] = "Solo se pueden eliminar reportes completados";
                return RedirectToPage();
            }

            var archivados = _context.Archivados.Where(a => a.ID_reporte == id);
            _context.Archivados.RemoveRange(archivados);

            if (reporte.Evidencias != null)
            {
                foreach (var evidencia in reporte.Evidencias)
                {
                    var rutaFisica = Path.Combine(_env.WebRootPath, evidencia.Ruta.TrimStart('/'));
                    if (System.IO.File.Exists(rutaFisica))
                    {
                        System.IO.File.Delete(rutaFisica);
                    }
                }
                _context.Evidencias.RemoveRange(reporte.Evidencias);
            }

            _context.Reportes.Remove(reporte);
            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = "Reporte eliminado";
            return RedirectToPage();
        }
    }
}