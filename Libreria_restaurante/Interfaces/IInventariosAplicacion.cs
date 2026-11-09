using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Interfaces
{
    public class IInventariosAplicacion
    {
        Inventarios Insertar(Inventarios entidad);
        Inventarios Actualizar(Inventarios entidad);
        List<Inventarios> Consultar();
        List<Inventarios> Buscar(Inventarios entidad);
        Inventarios Borrar(Inventarios entidad);
    }
}