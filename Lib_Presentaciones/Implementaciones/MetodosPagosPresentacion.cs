using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class MetodosPagosPresentacion : IMetodosPagosPresentacion
    {
        private IComunicaciones IComunicaciones;

        public MetodosPagosPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public MetodosPagos Actualizar(MetodosPagos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<MetodosPagos>(
                "MetodosPagos/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public MetodosPagos Borrar(MetodosPagos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<MetodosPagos>(
                "MetodosPagos/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<MetodosPagos> Buscar(MetodosPagos entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<MetodosPagos>>(
                "MetodosPagos/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<MetodosPagos> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<MetodosPagos>>(
                "MetodosPagos/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public MetodosPagos Insertar(MetodosPagos entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<MetodosPagos>(
                "MetodosPagos/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(MetodosPagos entidad)
        {
            return string.IsNullOrEmpty(entidad.tipo);
        }
    }
}
