using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IRecetasPresentacion
    {
        Recetas Insertar(Recetas entidad);
        Recetas Actualizar(Recetas entidad);
        List<Recetas> Consultar();
        List<Recetas> Buscar(Recetas entidad);
        Recetas Borrar(Recetas entidad);
    }
}
