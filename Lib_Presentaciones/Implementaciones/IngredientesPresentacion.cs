using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class IngredientesPresentacion : IIngredientesPresentacion
    {
        private IComunicaciones IComunicaciones;

        public IngredientesPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Ingredientes Actualizar(Ingredientes entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Ingredientes>(
                "Ingredientes/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Ingredientes Borrar(Ingredientes entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Ingredientes>(
                "Ingredientes/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Ingredientes> Buscar(Ingredientes entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Ingredientes>>(
                "Ingredientes/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Ingredientes> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Ingredientes>>(
                "Ingredientes/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Ingredientes Insertar(Ingredientes entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Ingredientes>(
                "Ingredientes/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Ingredientes entidad)
        {
            return string.IsNullOrEmpty(entidad.nombre);
        }
    }
}
