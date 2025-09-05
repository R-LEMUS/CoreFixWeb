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

namespace CoreFixWeb.Pages
{
    [Authorize]
    public class ReportesModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportesModel(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Reporte> Reportes { get; set; } = new List<Reporte>();
        [BindProperty]
        public Reporte NuevoReporte { get; set; } = new Reporte();

        public List<SelectListItem> Equipos { get; set; }
        public List<SelectListItem> Estados { get; set; }
        public List<SelectListItem> TecnicosDisponibles { get; set; }

        public int ID_usuario_logueado { get; set; }
        public string RolUsuario { get; set; }

        public void OnGet()
        {
            var userClaims = _httpContextAccessor.HttpContext.User;
            ID_usuario_logueado = int.Parse(userClaims.FindFirst("ID_usuario")?.Value ?? "0");
            RolUsuario = userClaims.FindFirst(ClaimTypes.Role)?.Value ?? "Empleado";

            CargarCombos();

            var query = _context.Reportes
                .Include(r => r.Usuario)
                .Include(r => r.Equipo)
                .Include(r => r.EstadoReporte)
                .Include(r => r.TecnicoAsignado)
                .AsQueryable();

            if (RolUsuario == "Empleado")
            {
                query = query.Where(r => r.ID_usuario == ID_usuario_logueado);
            }
            else if (RolUsuario == "Supervisor")
            {
                query = query.Where(r => r.ID_estado_reporte == 1);
            }
            else if (RolUsuario == "Ingeniero")
            {
                query = query.Where(r => r.ID_estado_reporte == 2);
            }
            else if (RolUsuario == "Técnico")
            {
                query = query.Where(r => r.ID_tecnico_asignado == ID_usuario_logueado);
            }

            Reportes = query.ToList();
        }

        public IActionResult OnPost()
        {
            var userClaims = _httpContextAccessor.HttpContext.User;
            ID_usuario_logueado = int.Parse(userClaims.FindFirst("ID_usuario")?.Value ?? "0");
            RolUsuario = userClaims.FindFirst(ClaimTypes.Role)?.Value ?? "Empleado";

            if (RolUsuario != "Empleado") return Forbid();

            var reporte = new Reporte
            {
                ID_usuario = ID_usuario_logueado,
                ID_equipo = NuevoReporte.ID_equipo,
                ID_estado_reporte = 1,
                Descripcion = NuevoReporte.Descripcion,
                Fecha_reporte = DateTime.Now
            };

            _context.Reportes.Add(reporte);
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostEliminar(int id)
        {
            var reporte = _context.Reportes.FirstOrDefault(r => r.ID_reporte == id);
            if (reporte != null)
            {
                _context.Reportes.Remove(reporte);
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostValidar(int id)
        {
            var userClaims = _httpContextAccessor.HttpContext.User;
            RolUsuario = userClaims.FindFirst(ClaimTypes.Role)?.Value ?? "Empleado";

            var reporte = _context.Reportes
                .Include(r => r.EstadoReporte)
                .FirstOrDefault(r => r.ID_reporte == id);

            if (reporte == null || RolUsuario != "Supervisor") return Forbid();

            reporte.ID_estado_reporte = 2;
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostAsignar(int id, int idTecnico)
        {
            var userClaims = _httpContextAccessor.HttpContext.User;
            RolUsuario = userClaims.FindFirst(ClaimTypes.Role)?.Value ?? "Empleado";

            var reporte = _context.Reportes
                .Include(r => r.TecnicoAsignado)
                .FirstOrDefault(r => r.ID_reporte == id);

            if (reporte == null || RolUsuario != "Ingeniero") return Forbid();

            reporte.ID_tecnico_asignado = idTecnico;
            reporte.ID_estado_reporte = 3;

            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostActualizarEstado(int id, int nuevoEstado)
        {
            var userClaims = _httpContextAccessor.HttpContext.User;
            ID_usuario_logueado = int.Parse(userClaims.FindFirst("ID_usuario")?.Value ?? "0");
            RolUsuario = userClaims.FindFirst(ClaimTypes.Role)?.Value ?? "Empleado";

            var reporte = _context.Reportes
                .Include(r => r.EstadoReporte)
                .FirstOrDefault(r => r.ID_reporte == id);

            if (reporte == null || RolUsuario != "Técnico" || reporte.ID_tecnico_asignado != ID_usuario_logueado)
                return Forbid();

            reporte.ID_estado_reporte = nuevoEstado;
            _context.SaveChanges();

            return RedirectToPage();
        }

        private void CargarCombos()
        {
            Equipos = _context.Equipos.Select(e => new SelectListItem
            {
                Value = e.ID_equipo.ToString(),
                Text = e.Nombre
            }).ToList();

            Estados = _context.EstadosReportes.Select(er => new SelectListItem
            {
                Value = er.ID_estado_reporte.ToString(),
                Text = er.Estado
            }).ToList();

            TecnicosDisponibles = _context.Usuarios
                .Where(u => u.Puesto == "Técnico")
                .Select(t => new SelectListItem
                {
                    Value = t.ID_usuario.ToString(),
                    Text = t.Nombre
                })
                .ToList();
        }
    }
}