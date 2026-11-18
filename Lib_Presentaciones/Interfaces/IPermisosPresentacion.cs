using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IPermisosPresentacion
    {
        Task<Permisos> Insertar(Permisos entidad);
        Task<Permisos> Actualizar(Permisos entidad);
        Task<List<Permisos>> Consultar();
        Task<List<Permisos>> Buscar(Permisos entidad);
        Task<Permisos> Borrar(Permisos entidad);
    }
}
