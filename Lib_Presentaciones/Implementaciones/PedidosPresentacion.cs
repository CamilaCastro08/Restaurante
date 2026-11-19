using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class PedidosPresentacion : IPedidosPresentacion
    {
        private IComunicaciones IComunicaciones;

        public PedidosPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Pedidos Actualizar(Pedidos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Pedidos>(
                "Pedidos/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Pedidos Borrar(Pedidos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Pedidos>(
                "Pedidos/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Pedidos> Buscar(Pedidos entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Pedidos>>(
                "Pedidos/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Pedidos> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Pedidos>>(
                "Pedidos/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Pedidos Insertar(Pedidos entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Pedidos>(
                "Pedidos/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Pedidos entidad)
        {
            return string.IsNullOrEmpty(entidad.estado);
        }
    }
}
