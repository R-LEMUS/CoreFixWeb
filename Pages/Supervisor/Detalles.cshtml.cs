using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreFixWeb.Data;

namespace CoreFixWeb.Pages.Supervisor
{
    [Authorize(Roles = "Supervisor")]
    public class DetallesModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetallesModel(AppDbContext context)
        {
            _context = context;
        }

        public Reporte Reporte { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Reporte = await _context.Reportes
                .Include(r => r.Usuario)
                .Include(r => r.Equipo)
                .Include(r => r.EstadoReporte)
                .Include(r => r.TecnicoAsignado)
                .Include(r => r.Evidencias)
                    .ThenInclude(e => e.Usuario)
                .FirstOrDefaultAsync(r => r.ID_reporte == id);

            if (Reporte == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostArchivarAsync(int id)
        {
            var reporte = await _context.Reportes
                .Include(r => r.Usuario)
                .Include(r => r.Equipo)
                .FirstOrDefaultAsync(r => r.ID_reporte == id);

            if (reporte == null)
                return NotFound();

            if (reporte.ID_estado_reporte != 5)
            {
                TempData["Error"] = "Solo se pueden archivar reportes 'Solucionados'";
                return RedirectToPage();
            }

            bool yaArchivado = await _context.Archivados
                .AnyAsync(a => a.ID_reporte == id && a.ID_usuario == int.Parse(User.FindFirst("ID_usuario").Value));

            if (!yaArchivado)
            {
                var archivo = new Archivado
                {
                    ID_reporte = id,
                    ID_usuario = int.Parse(User.FindFirst("ID_usuario").Value),
                    Fecha_archivado = DateTime.Now
                };

                _context.Archivados.Add(archivo);
                await _context.SaveChangesAsync();
            }

            TempData["Mensaje"] = "Reporte archivado correctamente.";
            return RedirectToPage("/Supervisor/Reportes");
        }
    }
}