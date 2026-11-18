using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IUsuariosPresentacion
    {
        Task<Usuarios> Insertar(Usuarios entidad);
        Task<Usuarios> Actualizar(Usuarios entidad);
        Task<List<Usuarios>> Consultar();
        Task<List<Usuarios>> Buscar(Usuarios entidad);
        Task<Usuarios> Borrar(Usuarios entidad);
        Task<Usuarios> Login(Usuarios entidad);
    }
}
