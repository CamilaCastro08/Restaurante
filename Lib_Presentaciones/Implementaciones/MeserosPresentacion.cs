using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class MeserosPresentacion : IMeserosPresentacion
    {
        private IComunicaciones IComunicaciones;

        public MeserosPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Meseros Actualizar(Meseros entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Meseros>(
                "Meseros/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Meseros Borrar(Meseros entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Meseros>(
                "Meseros/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Meseros> Buscar(Meseros entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Meseros>>(
                "Meseros/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Meseros> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Meseros>>(
                "Meseros/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Meseros Insertar(Meseros entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Meseros>(
                "Meseros/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Meseros entidad)
        {
            return string.IsNullOrEmpty(entidad.zona_asignada);
        }
    }
}
