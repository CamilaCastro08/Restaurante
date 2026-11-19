using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IMetodosPagosPresentacion
    {
        MetodosPagos Insertar(MetodosPagos entidad);
        MetodosPagos Actualizar(MetodosPagos entidad);
        List<MetodosPagos> Consultar();
        List<MetodosPagos> Buscar(MetodosPagos entidad);
        MetodosPagos Borrar(MetodosPagos entidad);
    }
}
