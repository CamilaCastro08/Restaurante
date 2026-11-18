using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Interfaces
{
    public interface IProveedoresPresentacion
    {
        Task<Proveedores> Insertar(Proveedores entidad);
        Task<Proveedores> Actualizar(Proveedores entidad);
        Task<List<Proveedores>> Consultar();
        Task<List<Proveedores>> Buscar(Proveedores entidad);
        Task<Proveedores> Borrar(Proveedores entidad);
    }
}
