using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class UsuariosPresentacion : IUsuariosPresentacion
    {
        private IComunicaciones IComunicaciones;

        public UsuariosPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Usuarios Actualizar(Usuarios entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Usuarios>(
                "Usuarios/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Usuarios Borrar(Usuarios entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Usuarios>(
                "Usuarios/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Usuarios> Buscar(Usuarios entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Usuarios>>(
                "Usuarios/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Usuarios> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Usuarios>>(
                "Usuarios/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Usuarios Insertar(Usuarios entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            if (string.IsNullOrEmpty(entidad.contraseña))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Usuarios>(
                "Usuarios/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Usuarios Login(Usuarios entidad)
        {
            var task = this.IComunicaciones.Ejecutar<Usuarios>(
                "Usuarios/Login",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Usuarios entidad)
        {
            return string.IsNullOrEmpty(entidad.nombreUsuario) ||
                string.IsNullOrEmpty(entidad.correo) ||
                entidad.rol == null || entidad.rol == 0;
        }
    }
}
