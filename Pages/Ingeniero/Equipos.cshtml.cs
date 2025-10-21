using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreFixWeb.Data;

namespace CoreFixWeb.Pages.Ingeniero
{
    [Authorize(Roles = "Ingeniero")]

    public class EquiposModel : PageModel
    {

        private readonly AppDbContext _context;
        public EquiposModel(AppDbContext context)
        {
            _context = context;
        }
        public List<EquipoConReportes> Equipos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var equiposConReportes = await _context.Equipos
                .Include(e => e.Reportes)
                    .ThenInclude(r => r.EstadoReporte)
                .ToListAsync();

            Equipos = equiposConReportes.Select(e => new EquipoConReportes
            {
                Equipo = e,
                TotalReportes = e.Reportes?.Count ?? 0,
                UltimoReporte = e.Reportes?.OrderByDescending(r => r.Fecha_reporte).FirstOrDefault()
            }).ToList();

            return Page();
        }

        public class EquipoConReportes
        {
            public Equipo Equipo { get; set; } = new();
            public int TotalReportes { get; set; }
            public Reporte? UltimoReporte { get; set; }
        }
    }
}