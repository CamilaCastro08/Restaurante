using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IMenusPresentacion
    {
        Menus Insertar(Menus entidad);
        Menus Actualizar(Menus entidad);
        List<Menus> Consultar();
        List<Menus> Buscar(Menus entidad);
        Menus Borrar(Menus entidad);
    }
}
