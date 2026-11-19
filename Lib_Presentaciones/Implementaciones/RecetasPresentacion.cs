using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class RecetasPresentacion : IRecetasPresentacion
    {
        private IComunicaciones IComunicaciones;

        public RecetasPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Recetas Actualizar(Recetas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Recetas>(
                "Recetas/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Recetas Borrar(Recetas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Recetas>(
                "Recetas/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Recetas> Buscar(Recetas entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Recetas>>(
                "Recetas/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Recetas> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Recetas>>(
                "Recetas/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Recetas Insertar(Recetas entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Recetas>(
                "Recetas/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Recetas entidad)
        {
            return string.IsNullOrEmpty(entidad.instrucciones);
        }
    }
}
