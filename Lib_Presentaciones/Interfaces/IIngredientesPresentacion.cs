using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IIngredientesPresentacion
    {
        Ingredientes Insertar(Ingredientes entidad);
        Ingredientes Actualizar(Ingredientes entidad);
        List<Ingredientes> Consultar();
        List<Ingredientes> Buscar(Ingredientes entidad);
        Ingredientes Borrar(Ingredientes entidad);
    }
}
