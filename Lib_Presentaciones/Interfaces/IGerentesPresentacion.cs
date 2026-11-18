using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IGerentesPresentacion
    {
        Task<Gerentes> Insertar(Gerentes entidad);
        Task<Gerentes> Actualizar(Gerentes entidad);
        Task<List<Gerentes>> Consultar();
        Task<List<Gerentes>> Buscar(Gerentes entidad);
        Task<Gerentes> Borrar(Gerentes entidad);
    }
}
