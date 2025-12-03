using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreFixWeb.Data;

namespace CoreFixWeb.Pages.Ingeniero
{
    [Authorize(Roles = "Ingeniero")]
    public class DetallesEquipoModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetallesEquipoModel(AppDbContext context)
        {
            _context = context;
        }

        public Equipo Equipo { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Equipo = await _context.Equipos
                .Include(e => e.Reportes)
                    .ThenInclude(r => r.EstadoReporte)
                .FirstOrDefaultAsync(e => e.ID_equipo == id);

            if (Equipo == null)
                return NotFound();

            return Page();
        }
    }
}