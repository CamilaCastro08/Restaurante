using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IPedidosPresentacion
    {
        Pedidos Insertar(Pedidos entidad);
        Pedidos Actualizar(Pedidos entidad);
        List<Pedidos> Consultar();
        List<Pedidos> Buscar(Pedidos entidad);
        Pedidos Borrar(Pedidos entidad);
    }
}
