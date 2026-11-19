using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class CocinerosPresentacion : ICocinerosPresentacion
    {
        private IComunicaciones IComunicaciones;

        public CocinerosPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Cocineros Actualizar(Cocineros entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Cocineros>(
                "Cocineros/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Cocineros Borrar(Cocineros entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Cocineros>(
                "Cocineros/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Cocineros> Buscar(Cocineros entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Cocineros>>(
                "Cocineros/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Cocineros> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Cocineros>>(
                "Cocineros/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Cocineros Insertar(Cocineros entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Cocineros>(
                "Cocineros/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Cocineros entidad)
        {
            return string.IsNullOrEmpty(entidad.especialidad);
        }
    }
}
