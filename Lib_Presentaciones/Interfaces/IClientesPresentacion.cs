using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IClientesPresentacion
    {
        Task<Clientes> Insertar(Clientes entidad);
        Task<Clientes> Actualizar(Clientes entidad);
        Task<List<Clientes>> Consultar();
        Task<List<Clientes>> Buscar(Clientes entidad);
        Task<Clientes> Borrar(Clientes entidad);
    }
}
