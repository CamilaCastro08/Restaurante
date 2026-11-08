using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class IMesasPresentacion
    {
        Mesas Insertar(Mesas entidad);
        Mesas Actualizar(Mesas entidad);
        List<Mesas> Consultar();
        List<Mesas> Buscar(Mesas entidad);
        Mesas Borrar(Mesas entidad);
    }
}
