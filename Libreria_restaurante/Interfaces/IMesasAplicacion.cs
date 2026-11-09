using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Interfaces
{
    public class IMesasAplicacion
    {
        Mesas Insertar(Mesas entidad);
        Mesas Actualizar(Mesas entidad);
        List<Mesas> Consultar();
        List<Mesas> Buscar(Mesas entidad);
        Mesas Borrar(Mesas entidad);
    }
}