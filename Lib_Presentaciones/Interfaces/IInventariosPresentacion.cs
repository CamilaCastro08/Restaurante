using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class IInventariosPresentacion
    {
        Inventarios Insertar(Inventarios entidad);
        Inventarios Actualizar(Inventarios entidad);
        List<Inventarios> Consultar();
        List<Inventarios> Buscar(Inventarios entidad);
        Inventarios Borrar(Inventarios entidad);
    }
}
