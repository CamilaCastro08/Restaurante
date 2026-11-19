using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class MenusPresentacion : IMenusPresentacion
    {
        private IComunicaciones IComunicaciones;

        public MenusPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Menus Actualizar(Menus entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Menus>(
                "Menus/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Menus Borrar(Menus entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Menus>(
                "Menus/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Menus> Buscar(Menus entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Menus>>(
                "Menus/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Menus> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Menus>>(
                "Menus/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Menus Insertar(Menus entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Menus>(
                "Menus/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Menus entidad)
        {
            return string.IsNullOrEmpty(entidad.nombre);
        }
    }
}
