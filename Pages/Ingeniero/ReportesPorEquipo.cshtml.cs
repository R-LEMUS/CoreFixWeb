using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreFixWeb.Data;

namespace CoreFixWeb.Pages.Ingeniero
{
    [Authorize(Roles = "Ingeniero")]
    public class ReportesPorEquipoModel : PageModel
    {
        private readonly AppDbContext _context;

        public ReportesPorEquipoModel(AppDbContext context)
        {
            _context = context;
        }

        public Equipo Equipo { get; set; } = new();
        public List<Reporte> Reportes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo == null)
                return NotFound();

            Equipo = equipo;
            Reportes = await _context.Reportes
                .Include(r => r.Usuario)
                .Include(r => r.EstadoReporte)
                .Include(r => r.Evidencias)
                .Include(r => r.TecnicoAsignado)
                .Where(r => r.ID_equipo == id)
                .OrderByDescending(r => r.Fecha_reporte)
                .ToListAsync();

            return Page();
        }
    }
}