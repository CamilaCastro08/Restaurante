using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IIngredientesPresentacion
    {
        Task<Ingredientes> Insertar(Ingredientes entidad);
        Task<Ingredientes> Actualizar(Ingredientes entidad);
        Task<List<Ingredientes>> Consultar();
        Task<List<Ingredientes>> Buscar(Ingredientes entidad);
        Task<Ingredientes> Borrar(Ingredientes entidad);
    }
}
