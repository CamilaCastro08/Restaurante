using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class MesasPresentacion : IMesasPresentacion
    {
        private IComunicaciones IComunicaciones;

        public MesasPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Mesas Actualizar(Mesas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Mesas>(
                "Mesas/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Mesas Borrar(Mesas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Mesas>(
                "Mesas/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Mesas> Buscar(Mesas entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Mesas>>(
                "Mesas/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Mesas> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Mesas>>(
                "Mesas/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Mesas Insertar(Mesas entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Mesas>(
                "Mesas/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Mesas entidad)
        {
            return string.IsNullOrEmpty(entidad.ubicacion);
        }
    }
}
