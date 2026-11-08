using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class IProductosPresentacion
    {
        Productos Insertar(Productos entidad);
        Productos Actualizar(Productos entidad);
        List<Productos> Consultar();
        List<Productos> Buscar(Productos entidad);
        Productos Borrar(Productos entidad);
    }
}
