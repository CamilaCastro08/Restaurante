using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IRecetasPresentacion
    {
        Task<Recetas> Insertar(Recetas entidad);
        Task<Recetas> Actualizar(Recetas entidad);
        Task<List<Recetas>> Consultar();
        Task<List<Recetas>> Buscar(Recetas entidad);
        Task<Recetas> Borrar(Recetas entidad);
    }
}
