using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreFixWeb.Data;

namespace CoreFixWeb.Pages.Empleado
{
    [Authorize(Roles = "Empleado")]
    public class SubirEvidenciasModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SubirEvidenciasModel(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public IFormFile Archivo { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Archivo == null || Archivo.Length == 0)
            {
                ModelState.AddModelError("", "Selecciona un archivo.");
                return Page();
            }

            var carpeta = Path.Combine(_env.WebRootPath, "evidencias");
            Directory.CreateDirectory(carpeta);

            var nombre = $"{Guid.NewGuid()}{Path.GetExtension(Archivo.FileName)}";
            var ruta = Path.Combine(carpeta, nombre);

            using (var stream = new FileStream(ruta, FileMode.Create))
            {
                await Archivo.CopyToAsync(stream);
            }

            var ultimoNumero = await _context.Evidencias
                .Where(e => e.ID_reporte == Id)
                .OrderByDescending(e => e.Numero_Evidencia)
                .Select(e => e.Numero_Evidencia)
                .FirstOrDefaultAsync();
            
            var evidencia = new Evidencia
            {
                ID_reporte = Id,
                ID_usuario = int.Parse(User.Claims.First(c => c.Type == "ID_usuario").Value),
                Ruta = $"/evidencias/{nombre}",
                Fecha_subida = DateTime.Now,
                Numero_Evidencia = ultimoNumero + 1
            };

            _context.Evidencias.Add(evidencia);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Empleado/Reportes");
        }
    }
}