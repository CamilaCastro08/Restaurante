using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IMesasPresentacion
    {
        Task<Mesas> Insertar(Mesas entidad);
        Task<Mesas> Actualizar(Mesas entidad);
        Task<List<Mesas>> Consultar();
        Task<List<Mesas>> Buscar(Mesas entidad);
        Task<Mesas> Borrar(Mesas entidad);
    }
}
