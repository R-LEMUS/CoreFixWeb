using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreFixWeb.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CoreFixWeb.Pages.Empleado
{
    [Authorize(Roles = "Empleado")]
    public class ReportesModel : PageModel
    {
        private readonly AppDbContext _context;
        public List<Reporte> Reportes { get; set; }

        public ReportesModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            var idUsuarioClaim = User.Claims.FirstOrDefault(c => c.Type == "ID_usuario")?.Value;
            if (string.IsNullOrEmpty(idUsuarioClaim) || !int.TryParse(idUsuarioClaim, out int idEmpleado))
            {
                Reportes = new List<Reporte>();
                return;
            }

            Reportes = await _context.Reportes
                .Where(r => r.ID_usuario == idEmpleado &&
                (r.ID_estado_reporte == 1 || r.ID_estado_reporte == 7))
                .Include(r => r.EstadoReporte)
                .Include(r => r.Equipo)
                .Include(r => r.Evidencias)
                .ToListAsync();
        }

        public IActionResult OnPostEliminar(int id)
        {
            var reporte = _context.Reportes
                                .Include(r => r.Evidencias)
                                .FirstOrDefault(r => r.ID_reporte == id);

            if (reporte == null)
            {
                return NotFound();
            }

            if (reporte.Evidencias != null)
            {
                _context.Evidencias.RemoveRange(reporte.Evidencias);
            }

            _context.Reportes.Remove(reporte);
            _context.SaveChanges();

            TempData["MensajeExito"] = "Reporte eliminado";

            return RedirectToPage();
        }
        
        public async Task<IActionResult> OnPostReenviar(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);
            if (reporte == null || reporte.ID_estado_reporte != 7)
            {
                TempData["MensajeError"] = "Solo se pueden reenviar reportes rechazados";
                return RedirectToPage();
            }

            var idEmpleado = int.Parse(User.Claims.First(c => c.Type == "ID_usuario").Value);
            if (reporte.ID_usuario != idEmpleado)
                return Forbid();

            reporte.ID_estado_reporte = 1;
            _context.Reportes.Update(reporte);
            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = "Reporte reenviado";

            return RedirectToPage();
        }
    }
}