using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IPermisosPresentacion
    {
        Permisos Insertar(Permisos entidad);
        Permisos Actualizar(Permisos entidad);
        List<Permisos> Consultar();
        List<Permisos> Buscar(Permisos entidad);
        Permisos Borrar(Permisos entidad);
    }
}
