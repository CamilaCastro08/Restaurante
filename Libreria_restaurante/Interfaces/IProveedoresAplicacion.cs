using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Interfaces
{
    public class IProveedoresAplicacion
    {
        Proveedores Insertar(Proveedores entidad);
        Proveedores Actualizar(Proveedores entidad);
        List<Proveedores> Consultar();
        List<Proveedores> Buscar(Proveedores entidad);
        Proveedores Borrar(Proveedores entidad);
    }
}