using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreFixWeb.Data;

namespace CoreFixWeb.Pages.Tecnico
{
    [Authorize(Roles = "Técnico")]
    public class ReportesModel : PageModel
    {
        private readonly AppDbContext _context;

        public ReportesModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Reporte> Reportes { get; set; } = new();

        public async Task OnGetAsync()
        {
            var tecnicoId = int.Parse(User.Claims.First(c => c.Type == "ID_usuario").Value);

            Reportes = await _context.Reportes
                .Include(r => r.Equipo)
                .Include(r => r.EstadoReporte)
                .Include(r => r.Evidencias)
                .Where(r => r.ID_tecnico_asignado == tecnicoId)
                .OrderByDescending(r => r.Fecha_reporte)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostValidar(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);
            if (reporte == null || reporte.ID_estado_reporte != 4)
            {
                TempData["MensajeError"] = "No se puede validar este reporte";
                return RedirectToPage();
            }

            var idTecnico = int.Parse(User.Claims.First(c => c.Type == "ID_usuario").Value);
            reporte.ID_estado_reporte = 5;
            reporte.ID_tecnico_validador = idTecnico;
            _context.Reportes.Update(reporte);
            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = "Reporte validado";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostIniciarProceso(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);
            if (reporte == null || reporte.ID_estado_reporte != 3)
            {
                TempData["MensajeError"] = "No se puede iniciar este reporte";
                return RedirectToPage();
            }

            reporte.ID_estado_reporte = 4;
            _context.Reportes.Update(reporte);
            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = "Reporte marcado como 'En Proceso'";
            return RedirectToPage();
        }
    }
}