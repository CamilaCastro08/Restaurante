using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface ICocinerosPresentacion
    {
        Task<Cocineros> Insertar(Cocineros entidad);
        Task<Cocineros> Actualizar(Cocineros entidad);
        Task<List<Cocineros>> Consultar();
        Task<List<Cocineros>> Buscar(Cocineros entidad);
        Task<Cocineros> Borrar(Cocineros entidad);
    }
}
