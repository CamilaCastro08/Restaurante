using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IPagosPresentacion
    {
        Task<Pagos> Insertar(Pagos entidad);
        Task<Pagos> Actualizar(Pagos entidad);
        Task<List<Pagos>> Consultar();
        Task<List<Pagos>> Buscar(Pagos entidad);
        Task<Pagos> Borrar(Pagos entidad);
    }
}
