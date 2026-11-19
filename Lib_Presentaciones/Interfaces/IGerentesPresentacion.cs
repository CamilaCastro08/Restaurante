using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IGerentesPresentacion
    {
        Gerentes Insertar(Gerentes entidad);
        Gerentes Actualizar(Gerentes entidad);
        List<Gerentes> Consultar();
        List<Gerentes> Buscar(Gerentes entidad);
        Gerentes Borrar(Gerentes entidad);
    }
}
