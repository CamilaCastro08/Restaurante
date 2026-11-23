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

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(NombreUsuario) || string.IsNullOrWhiteSpace(Contrasena))
            {
                Mensaje = "Ingrese usuario y contraseña";
                return Page();
            }

            try
            {
                var usuario = await Task.Run(() =>
                    usuarios.Login(new Usuarios { nombreUsuario = NombreUsuario, contraseña = Contrasena }));

                var permisos = await Task.Run(() => permisosPres.Consultar());
                var nombres = permisos.Where(x => x.rol == usuario.rol).Select(x => x.nombre);

                HttpContext.Session.Clear();
                HttpContext.Session.SetString("usuario", usuario.id.ToString());
                HttpContext.Session.SetString("nombre", usuario.nombreUsuario!);
                HttpContext.Session.SetString("permisos", string.Join(",", nombres));

                return RedirectToPage("/Index");
            }
            catch (HttpRequestException)
            {
                Mensaje = "No se pudo conectar con el servidor de servicios (asp.servicios). Verifique que esté iniciado.";
                return Page();
            }
            catch (TaskCanceledException)
            {
                Mensaje = "El servidor tardó demasiado en responder.";
                return Page();
            }
            catch (Exception ex)
            {
                Mensaje = string.IsNullOrWhiteSpace(ex.Message) ? "Usuario o contraseña incorrectos" : ex.Message;
                return Page();
            }
        }
    }
}