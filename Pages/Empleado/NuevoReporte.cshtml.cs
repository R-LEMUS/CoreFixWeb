using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreFixWeb.Data;

namespace CoreFixWeb.Pages.Empleado
{
    [Authorize(Roles = "Empleado")]
    public class NuevoReporteModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public Reporte Reporte { get; set; }
        public List<SelectListItem> Equipos { get; set; }

        public List<SelectListItem> Prioridades { get; } = new()
        {
            new SelectListItem("Baja", "Baja"),
            new SelectListItem("Media", "Media"),
            new SelectListItem("Alta", "Alta"),
            new SelectListItem("Critica", "Critica")
        };

        public NuevoReporteModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Equipos = _context.Equipos
                              .Select(e => new SelectListItem
                              {
                                  Value = e.ID_equipo.ToString(),
                                  Text = e.Nombre
                              })
                              .ToList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                OnGet();
                return Page();
            }

            var idEmpleado = int.Parse(User.Claims.First(c => c.Type == "ID_usuario").Value);
            Reporte.ID_usuario = idEmpleado;
            Reporte.ID_estado_reporte = 1;
            Reporte.Fecha_reporte = DateTime.Now;

            var ultimoNumero = _context.Reportes
                    .Where(r => r.ID_usuario == idEmpleado)
                    .OrderByDescending(r => r.Numero_Reporte)
                    .Select(r => r.Numero_Reporte)
                    .FirstOrDefault();

            Reporte.ID_usuario = idEmpleado;
            Reporte.ID_estado_reporte = 1;
            Reporte.Numero_Reporte = ultimoNumero + 1;
            Reporte.Fecha_reporte = DateTime.Now;

            _context.Reportes.Add(Reporte);
            _context.SaveChanges();

            return RedirectToPage("/Empleado/Reportes");
        }
    }
}