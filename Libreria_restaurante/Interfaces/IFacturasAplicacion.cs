using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Interfaces
{
    public class IFacturasAplicacion
    {
        Facturas Insertar(Facturas entidad);
        Facturas Actualizar(Facturas entidad);
        List<Facturas> Consultar();
        List<Facturas> Buscar(Facturas entidad);
        Facturas Borrar(Facturas entidad);
    }
}