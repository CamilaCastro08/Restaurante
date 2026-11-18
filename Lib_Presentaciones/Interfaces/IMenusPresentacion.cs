using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IMenusPresentacion
    {
        Task<Menus> Insertar(Menus entidad);
        Task<Menus> Actualizar(Menus entidad);
        Task<List<Menus>> Consultar();
        Task<List<Menus>> Buscar(Menus entidad);
        Task<Menus> Borrar(Menus entidad);
    }
}
