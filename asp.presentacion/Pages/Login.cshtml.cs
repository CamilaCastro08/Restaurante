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
        private readonly IRolesPresentacion rolesPres;

        public LoginModel(IUsuariosPresentacion usuarios, IPermisosPresentacion permisosPres, IRolesPresentacion rolesPres)
        {
            this.usuarios = usuarios;
            this.permisosPres = permisosPres;
            this.rolesPres = rolesPres;
        }

        [BindProperty] public string? NombreUsuario { get; set; }
        [BindProperty] public string? Contrasena { get; set; }
        public string? Mensaje { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(NombreUsuario) || string.IsNullOrWhiteSpace(Contrasena))
            {
                Mensaje = "Ingrese usuario y contraseña";
                return Page();
            }
            try
            {
                var usuario = usuarios.Login(new Usuarios { nombreUsuario = NombreUsuario, contraseña = Contrasena });
                var permisos = permisosPres.Consultar();
                var nombres = permisos.Where(x => x.rol == usuario.rol).Select(x => x.nombre);

                var rolObj = rolesPres.Consultar().FirstOrDefault(r => r.id == usuario.rol);
                var nombreRol = rolObj?.nombre ?? "Mesero";

                HttpContext.Session.Clear();
                HttpContext.Session.SetString("usuario", usuario.id.ToString());
                HttpContext.Session.SetString("nombre", usuario.nombreUsuario!);
                HttpContext.Session.SetString("rol", nombreRol);
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