using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Interfaces
{
    public class IMeserosAplicacion
    {
        Meseros Insertar(Meseros entidad);
        Meseros Actualizar(Meseros entidad);
        List<Meseros> Consultar();
        List<Meseros> Buscar(Meseros entidad);
        Meseros Borrar(Meseros entidad);
    }
}