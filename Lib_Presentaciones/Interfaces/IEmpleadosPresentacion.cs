using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IEmpleadosPresentacion
    {
        Task<Empleados> Insertar(Empleados entidad);
        Task<Empleados> Actualizar(Empleados entidad);
        Task<List<Empleados>> Consultar();
        Task<List<Empleados>> Buscar(Empleados entidad);
        Task<Empleados> Borrar(Empleados entidad);
    }
}
