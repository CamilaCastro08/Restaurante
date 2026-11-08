using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class ICocinerosPresentacion
    {
        Cocineros Insertar(Cocineros entidad);
        Cocineros Actualizar(Cocineros entidad);
        List<Cocineros> Consultar();
        List<Cocineros> Buscar(Cocineros entidad);
        Cocineros Borrar(Cocineros entidad);
    }
}
