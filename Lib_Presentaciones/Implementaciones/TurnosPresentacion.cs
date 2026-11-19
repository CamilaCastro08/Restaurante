using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class TurnosPresentacion : ITurnosPresentacion
    {
        private IComunicaciones IComunicaciones;

        public TurnosPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Turnos Actualizar(Turnos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Turnos>(
                "Turnos/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Turnos Borrar(Turnos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Turnos>(
                "Turnos/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Turnos> Buscar(Turnos entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Turnos>>(
                "Turnos/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Turnos> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Turnos>>(
                "Turnos/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Turnos Insertar(Turnos entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Turnos>(
                "Turnos/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Turnos entidad)
        {
            return entidad.empleado == 0;
        }
    }
}
