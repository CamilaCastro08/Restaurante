using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface ICocinerosPresentacion
    {
        Cocineros Insertar(Cocineros entidad);
        Cocineros Actualizar(Cocineros entidad);
        List<Cocineros> Consultar();
        List<Cocineros> Buscar(Cocineros entidad);
        Cocineros Borrar(Cocineros entidad);
    }
}
