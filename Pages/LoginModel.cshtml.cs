using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using CoreFixWeb.Data;

namespace CoreFixWeb.Pages

{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginModel(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public string Correo { get; set; }

        [BindProperty]
        public string Contraseña { get; set; }

        public string MensajeError { get; set; }

        public async Task<IActionResult> OnPost()
        {
            var usuario = _context.Usuarios.
            FirstOrDefault(u => u.Correo == Correo && u.Contraseña == Contraseña);

            if (usuario == null)
            {
                MensajeError = "Correo o contraseña incorrectos.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim("Nombre", usuario.Nombre ?? "Usuario"),
                new Claim("ID_usuario", usuario.ID_usuario.ToString()),
                new Claim(ClaimTypes.Role, usuario.Puesto ?? "Usuario")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync
            (CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (usuario.Puesto == "Empleado")
            {
                return RedirectToPage("/Empleado/Reportes");
            }
            else if (usuario.Puesto == "Supervisor")
            {
                return RedirectToPage("/Supervisor/Reportes");
            }
            else if (usuario.Puesto == "Ingeniero")
            {
                return RedirectToPage("/Ingeniero/Reportes");
            }
            if (usuario.Puesto == "Técnico")
            {
                return RedirectToPage("/Tecnico/Reportes");
            }

            return RedirectToPage("/Index");
        }
    }
}