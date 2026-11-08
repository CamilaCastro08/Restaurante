using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public class IPagosPresentacion
    {
        Pagos Insertar(Pagos entidad);
        Pagos Actualizar(Pagos entidad);
        List<Pagos> Consultar();
        List<Pagos> Buscar(Pagos entidad);
        Pagos Borrar(Pagos entidad);
    }
}
