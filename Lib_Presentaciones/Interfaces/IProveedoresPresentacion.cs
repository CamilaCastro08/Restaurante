using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class IProveedoresPresentacion
    {
        Proveedores Insertar(Proveedores entidad);
        Proveedores Actualizar(Proveedores entidad);
        List<Proveedores> Consultar();
        List<Proveedores> Buscar(Proveedores entidad);
        Proveedores Borrar(Proveedores entidad);
    }
}
