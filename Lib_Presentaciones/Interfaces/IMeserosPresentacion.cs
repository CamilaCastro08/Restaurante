using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IMeserosPresentacion
    {
        Task<Meseros> Insertar(Meseros entidad);
        Task<Meseros> Actualizar(Meseros entidad);
        Task<List<Meseros>> Consultar();
        Task<List<Meseros>> Buscar(Meseros entidad);
        Task<Meseros> Borrar(Meseros entidad);
    }
}
