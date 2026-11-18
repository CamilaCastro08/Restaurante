using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IAuditoriasPresentacion
    {
        Task<List<Auditorias>> Consultar();
        Task<List<Auditorias>> Buscar(Auditorias entidad);
    }
}
