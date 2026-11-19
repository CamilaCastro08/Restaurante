using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class PermisosPresentacion : IPermisosPresentacion
    {
        private IComunicaciones IComunicaciones;

        public PermisosPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Permisos Actualizar(Permisos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Permisos>(
                "Permisos/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Permisos Borrar(Permisos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Permisos>(
                "Permisos/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Permisos> Buscar(Permisos entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Permisos>>(
                "Permisos/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Permisos> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Permisos>>(
                "Permisos/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Permisos Insertar(Permisos entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Permisos>(
                "Permisos/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Permisos entidad)
        {
            return string.IsNullOrEmpty(entidad.nombre);
        }
    }
}
