using asp.presentacion.Nucleo;

namespace asp.presentacion.Pages
{
    public class IndexModel : PaginaProtegida
    {
        protected override string Permiso => "";

        public void OnGet()
        {
        }
    }
}
