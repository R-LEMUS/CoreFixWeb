using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreFixWeb.Data;
using Microsoft.VisualBasic;

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

        public async Task OnGetAsync()
        {

            Reportes = await _context.Reportes
                .Include(r => r.Usuario)
                .Include(r => r.Equipo)
                .Include(r => r.EstadoReporte)
                .Include(r => r.Evidencias)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostValidar(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);
            if (reporte == null || reporte.ID_estado_reporte != 1)
                return BadRequest("No se puede validar.");

            var idSupervisor = int.Parse(User.Claims.First(c => c.Type == "ID_usuario").Value);

            reporte.ID_estado_reporte = 2;
            reporte.ID_supervisor_validador = idSupervisor;
            _context.Reportes.Update(reporte);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        [BindProperty]
        public string MotivoRechazo { get; set; } = "";

        public async Task<IActionResult> OnPostRechazar(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);
            if (reporte == null || reporte.ID_estado_reporte != 1)
                return BadRequest("El reporte no se puede rechazar.");

            reporte.ID_estado_reporte = 7;
            _context.Reportes.Update(reporte);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminar(int id)
        {
            var reporte = await _context.Reportes
                .Include(r => r.Evidencias)
                .FirstOrDefaultAsync(r => r.ID_reporte == id);

            if (reporte == null)
                return NotFound();

            if (reporte.ID_estado_reporte != 5 && reporte.ID_estado_reporte != 6)
                return BadRequest("Solo se pueden eliminar reportes completados.");

            if (reporte.Evidencias != null)
            {
                foreach(var evidencia in reporte.Evidencias)
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

            return RedirectToPage();
        }
    }
}