using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp.presentacion.Nucleo
{
    public abstract class PaginaProtegida : PageModel
    {
        protected abstract string Permiso { get; }

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
                TempData["Mensaje"] = "No tiene permiso";
                context.Result = RedirectToPage("/Index");
            }
        }
    }
}
