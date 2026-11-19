using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class ReservasPresentacion : IReservasPresentacion
    {
        private IComunicaciones IComunicaciones;

        public ReservasPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Reservas Actualizar(Reservas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Reservas>(
                "Reservas/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Reservas Borrar(Reservas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Reservas>(
                "Reservas/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Reservas> Buscar(Reservas entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Reservas>>(
                "Reservas/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Reservas> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Reservas>>(
                "Reservas/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Reservas Insertar(Reservas entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Reservas>(
                "Reservas/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Reservas entidad)
        {
            return string.IsNullOrEmpty(entidad.estado);
        }
    }
}
