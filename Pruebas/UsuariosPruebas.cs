using Libreria_restaurante.Entidades;
using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;

namespace pruebas
{
    [TestClass]
    public class UsuariosPruebas
    {
        private TestDbContext conexion;
        private IUsuariosAplicacion iAplicacion;
        private Usuarios? entidad = null;

        public UsuariosPruebas()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new UsuariosAplicacion(this.conexion);
        }

        [TestMethod]
        public void Execute()
        {
            Insertar();
            Consultar();
            Buscar();
            Login();
            Actualizar();
            Borrar();
        }

        private int ContarAuditorias(string accion)
        {
            return this.conexion.Auditorias!.Count(x => x.tabla == "Usuarios" && x.accion == accion);
        }

        private void Reiniciar()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new UsuariosAplicacion(this.conexion);
        }

        private void Insertar()
        {
            int antes = ContarAuditorias("Insertar");
            this.entidad = this.iAplicacion.Insertar(new Usuarios()
            {
                nombreUsuario = "usuario.prueba",
                correo = "prueba@correo.com",
                contraseña = "Clave123*",
                activo = true,
                rol = 1
            });
            Reiniciar();
            if (this.entidad.id == 0)
                throw new Exception("No se guardo");
            if (this.entidad.contraseña != null)
                throw new Exception("Devolvio la contraseña");
            if (ContarAuditorias("Insertar") != antes + 1)
                throw new Exception("No se guardo la auditoria");

            try
            {
                this.iAplicacion.Insertar(new Usuarios()
                {
                    nombreUsuario = "usuario.prueba",
                    correo = "otro@correo.com",
                    contraseña = "Clave123*",
                    activo = true,
                    rol = 1
                });
                throw new Exception("Usuario repetido permitido");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Usuario repetido permitido")
                    throw;
            }
            Reiniciar();
        }

        private void Consultar()
        {
            var lista = this.iAplicacion.Consultar();
            Reiniciar();
            if (lista.Count <= 0)
                throw new Exception("Lista vacia");
            foreach (var item in lista)
                if (item.contraseña != null)
                    throw new Exception("Devolvio la contraseña");
        }

        private void Buscar()
        {
            var lista = this.iAplicacion.Buscar(new Usuarios() { nombreUsuario = this.entidad!.nombreUsuario });
            Reiniciar();
            if (lista.Count <= 0)
                throw new Exception("No encontro");
        }

        private void Login()
        {
            var usuario = this.iAplicacion.Login(new Usuarios() { nombreUsuario = "usuario.prueba", contraseña = "Clave123*" });
            Reiniciar();
            if (usuario.id != this.entidad!.id)
                throw new Exception("Login incorrecto");

            try
            {
                this.iAplicacion.Login(new Usuarios() { nombreUsuario = "usuario.prueba", contraseña = "Incorrecta1*" });
                throw new Exception("Login incorrecto permitido");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Login incorrecto permitido")
                    throw;
            }
            Reiniciar();
        }

        private void Actualizar()
        {
            int antes = ContarAuditorias("Actualizar");
            this.entidad!.correo = "actualizado@correo.com";
            this.entidad.contraseña = "";
            this.iAplicacion.Actualizar(this.entidad);
            Reiniciar();
            if (ContarAuditorias("Actualizar") != antes + 1)
                throw new Exception("No se guardo la auditoria");

            var usuario = this.iAplicacion.Login(new Usuarios() { nombreUsuario = "usuario.prueba", contraseña = "Clave123*" });
            Reiniciar();
            if (usuario.id != this.entidad.id)
                throw new Exception("Login incorrecto");
        }

        private void Borrar()
        {
            int antes = ContarAuditorias("Borrar");
            this.iAplicacion.Borrar(this.entidad!);
            Reiniciar();
            if (ContarAuditorias("Borrar") != antes + 1)
                throw new Exception("No se guardo la auditoria");
        }
    }
}
