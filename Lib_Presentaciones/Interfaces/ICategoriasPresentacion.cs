using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface ICategoriasPresentacion
    {
        Categorias Insertar(Categorias entidad);
        Categorias Actualizar(Categorias entidad);
        List<Categorias> Consultar();
        List<Categorias> Buscar(Categorias entidad);
        Categorias Borrar(Categorias entidad);
    }
}
