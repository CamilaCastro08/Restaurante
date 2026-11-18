using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IDetalles_PedidosPresentacion
    {
        Task<Detalles_Pedidos> Insertar(Detalles_Pedidos entidad);
        Task<Detalles_Pedidos> Actualizar(Detalles_Pedidos entidad);
        Task<List<Detalles_Pedidos>> Consultar();
        Task<List<Detalles_Pedidos>> Buscar(Detalles_Pedidos entidad);
        Task<Detalles_Pedidos> Borrar(Detalles_Pedidos entidad);
    }
}
