using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class RolesPresentacion : IRolesPresentacion
    {
        private IComunicaciones IComunicaciones;

        public RolesPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Roles Actualizar(Roles entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Roles>(
                "Roles/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Roles Borrar(Roles entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Roles>(
                "Roles/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Roles> Buscar(Roles entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Roles>>(
                "Roles/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Roles> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Roles>>(
                "Roles/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Roles Insertar(Roles entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Roles>(
                "Roles/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Roles entidad)
        {
            return string.IsNullOrEmpty(entidad.nombre);
        }
    }
}
