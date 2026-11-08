using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class IMeserosPresentacion
    {
        Meseros Insertar(Meseros entidad);
        Meseros Actualizar(Meseros entidad);
        List<Meseros> Consultar();
        List<Meseros> Buscar(Meseros entidad);
        Meseros Borrar(Meseros entidad);
    }
}
