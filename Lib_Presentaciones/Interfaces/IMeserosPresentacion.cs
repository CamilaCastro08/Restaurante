using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IMeserosPresentacion
    {
        Meseros Insertar(Meseros entidad);
        Meseros Actualizar(Meseros entidad);
        List<Meseros> Consultar();
        List<Meseros> Buscar(Meseros entidad);
        Meseros Borrar(Meseros entidad);
    }
}
