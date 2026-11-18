using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface ITurnosPresentacion
    {
        Task<Turnos> Insertar(Turnos entidad);
        Task<Turnos> Actualizar(Turnos entidad);
        Task<List<Turnos>> Consultar();
        Task<List<Turnos>> Buscar(Turnos entidad);
        Task<Turnos> Borrar(Turnos entidad);
    }
}
