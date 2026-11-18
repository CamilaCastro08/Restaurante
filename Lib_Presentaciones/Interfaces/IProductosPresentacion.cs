using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IProductosPresentacion
    {
        Task<Productos> Insertar(Productos entidad);
        Task<Productos> Actualizar(Productos entidad);
        Task<List<Productos>> Consultar();
        Task<List<Productos>> Buscar(Productos entidad);
        Task<Productos> Borrar(Productos entidad);
    }
}
