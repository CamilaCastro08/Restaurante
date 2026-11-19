using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class InventariosPresentacion : IInventariosPresentacion
    {
        private IComunicaciones IComunicaciones;

        public InventariosPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Inventarios Actualizar(Inventarios entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Inventarios>(
                "Inventarios/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Inventarios Borrar(Inventarios entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Inventarios>(
                "Inventarios/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Inventarios> Buscar(Inventarios entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Inventarios>>(
                "Inventarios/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Inventarios> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Inventarios>>(
                "Inventarios/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Inventarios Insertar(Inventarios entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Inventarios>(
                "Inventarios/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Inventarios entidad)
        {
            return entidad.ingrediente == 0;
        }
    }
}
