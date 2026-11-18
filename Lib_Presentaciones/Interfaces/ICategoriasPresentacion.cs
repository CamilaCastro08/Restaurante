using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface ICategoriasPresentacion
    {
        Task<Categorias> Insertar(Categorias entidad);
        Task<Categorias> Actualizar(Categorias entidad);
        Task<List<Categorias>> Consultar();
        Task<List<Categorias>> Buscar(Categorias entidad);
        Task<Categorias> Borrar(Categorias entidad);
    }
}
