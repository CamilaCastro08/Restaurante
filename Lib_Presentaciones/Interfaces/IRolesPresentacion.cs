using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IRolesPresentacion
    {
        Task<Roles> Insertar(Roles entidad);
        Task<Roles> Actualizar(Roles entidad);
        Task<List<Roles>> Consultar();
        Task<List<Roles>> Buscar(Roles entidad);
        Task<Roles> Borrar(Roles entidad);
    }
}
