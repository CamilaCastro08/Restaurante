using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class ITurnosPresentacion
    {
        Turnos Insertar(Turnos entidad);
        Turnos Actualizar(Turnos entidad);
        List<Turnos> Consultar();
        List<Turnos> Buscar(Turnos entidad);
        Turnos Borrar(Turnos entidad);
    }
}
