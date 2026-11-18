using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IPedidosPresentacion
    {
        Task<Pedidos> Insertar(Pedidos entidad);
        Task<Pedidos> Actualizar(Pedidos entidad);
        Task<List<Pedidos>> Consultar();
        Task<List<Pedidos>> Buscar(Pedidos entidad);
        Task<Pedidos> Borrar(Pedidos entidad);
    }
}
