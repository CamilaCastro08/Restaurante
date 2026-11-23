using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp.presentacion.Nucleo
{
    public abstract class PaginaProtegida : PageModel
    {
        protected abstract string Permiso { get; }
        public string RolActual => HttpContext.Session.GetString("rol") ?? "";

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                context.Result = RedirectToPage("/Login");
                return;
            }
            var permisos = (HttpContext.Session.GetString("permisos") ?? "").Split(',');
            if (Permiso != "" && !permisos.Contains(Permiso))
            {
                TempData["Mensaje"] = "No tiene permiso para acceder a este módulo.";
                context.Result = RedirectToPage("/Index");
            }
        }
    }
}