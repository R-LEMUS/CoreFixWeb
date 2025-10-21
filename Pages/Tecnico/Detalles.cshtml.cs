using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreFixWeb.Data;

namespace CoreFixWeb.Pages.Tecnico
{
    [Authorize(Roles = "Técnico")]
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
                .Include(r => r.Evidencias)
                    .ThenInclude(e => e.Usuario)
                .FirstOrDefaultAsync(r => r.ID_reporte == id);

            if (Reporte == null)
                return NotFound();

            return Page();
        }
    }
}