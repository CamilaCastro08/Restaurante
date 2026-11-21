using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp.presentacion.Nucleo
{
    public abstract class PaginaProtegida : PageModel
    {
        protected abstract string Permiso { get; }

        public string NombreActual => HttpContext.Session.GetString("nombre") ?? "";
        public string CorreoActual => HttpContext.Session.GetString("correo") ?? "";
        public string RolActual => HttpContext.Session.GetString("rol") ?? "";
        public bool Autenticado => !string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"));

        public bool EsAdministrador => RolActual.Equals("Administrador", StringComparison.OrdinalIgnoreCase);
        public bool EsMesero => RolActual.Equals("Mesero", StringComparison.OrdinalIgnoreCase);

        public string[] PermisosActuales =>
            (HttpContext.Session.GetString("permisos") ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

        public bool TienePermiso(string permiso) =>
            EsAdministrador || PermisosActuales.Contains(permiso);

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (!Autenticado || (!EsAdministrador && !EsMesero))
            {
                HttpContext.Session.Clear();
                context.Result = RedirectToPage("/Login");
                return;
            }

            if (!string.IsNullOrEmpty(Permiso) && !TienePermiso(Permiso))
            {
                TempData["Mensaje"] = "No tiene permiso para acceder a este módulo.";
                context.Result = RedirectToPage("/Index");
            }
        }
    }
}