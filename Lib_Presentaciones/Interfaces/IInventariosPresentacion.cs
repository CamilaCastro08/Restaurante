using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IInventariosPresentacion
    {
        Task<Inventarios> Insertar(Inventarios entidad);
        Task<Inventarios> Actualizar(Inventarios entidad);
        Task<List<Inventarios>> Consultar();
        Task<List<Inventarios>> Buscar(Inventarios entidad);
        Task<Inventarios> Borrar(Inventarios entidad);
    }
}
