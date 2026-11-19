using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class Detalles_PedidosPresentacion : IDetalles_PedidosPresentacion
    {
        private IComunicaciones IComunicaciones;

        public Detalles_PedidosPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Detalles_Pedidos Actualizar(Detalles_Pedidos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Detalles_Pedidos>(
                "Detalles_Pedidos/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Detalles_Pedidos Borrar(Detalles_Pedidos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Detalles_Pedidos>(
                "Detalles_Pedidos/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Detalles_Pedidos> Buscar(Detalles_Pedidos entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Detalles_Pedidos>>(
                "Detalles_Pedidos/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Detalles_Pedidos> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Detalles_Pedidos>>(
                "Detalles_Pedidos/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Detalles_Pedidos Insertar(Detalles_Pedidos entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Detalles_Pedidos>(
                "Detalles_Pedidos/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Detalles_Pedidos entidad)
        {
            return entidad.pedido == 0;
        }
    }
}
