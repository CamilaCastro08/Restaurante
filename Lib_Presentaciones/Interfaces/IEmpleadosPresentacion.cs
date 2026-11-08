using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class IEmpleadosPresentacion
    {
        Empleados Insertar(Empleados entidad);
        Empleados Actualizar(Empleados entidad);
        List<Empleados> Consultar();
        List<Empleados> Buscar(Empleados entidad);
        Empleados Borrar(Empleados entidad);
    }
}
