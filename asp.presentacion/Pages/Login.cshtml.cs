using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp.presentacion.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUsuariosPresentacion usuarios;
        private readonly IPermisosPresentacion permisosPres;

        public LoginModel(IUsuariosPresentacion usuarios, IPermisosPresentacion permisosPres)
        {
            this.usuarios = usuarios;
            this.permisosPres = permisosPres;
        }

        [BindProperty] public string? NombreUsuario { get; set; }
        [BindProperty] public string? Contrasena { get; set; }
        public string? Mensaje { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrWhiteSpace(NombreUsuario) || string.IsNullOrWhiteSpace(Contrasena))
            {
                Mensaje = "Ingrese usuario y contraseña";
                return Page();
            }
            try
            {
                var usuario = await usuarios.Login(new Usuarios { nombreUsuario = NombreUsuario, contraseña = Contrasena });
                var permisos = await permisosPres.Consultar();
                var nombres = permisos.Where(x => x.rol == usuario.rol).Select(x => x.nombre);
                HttpContext.Session.Clear();
                HttpContext.Session.SetString("usuario", usuario.id.ToString());
                HttpContext.Session.SetString("nombre", usuario.nombreUsuario!);
                HttpContext.Session.SetString("permisos", string.Join(",", nombres));
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                return Page();
            }
        }
    }
}
