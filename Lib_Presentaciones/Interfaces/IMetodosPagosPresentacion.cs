using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IMetodosPagosPresentacion
    {
        Task<MetodosPagos> Insertar(MetodosPagos entidad);
        Task<MetodosPagos> Actualizar(MetodosPagos entidad);
        Task<List<MetodosPagos>> Consultar();
        Task<List<MetodosPagos>> Buscar(MetodosPagos entidad);
        Task<MetodosPagos> Borrar(MetodosPagos entidad);
    }
}
