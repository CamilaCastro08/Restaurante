using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class IFacturasPresentacion
    {
        Facturas Insertar(Facturas entidad);
        Facturas Actualizar(Facturas entidad);
        List<Facturas> Consultar();
        List<Facturas> Buscar(Facturas entidad);
        Facturas Borrar(Facturas entidad);
    }
}
