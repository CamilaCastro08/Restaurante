using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Interfaces
{
    public class IMetodosPagosAplicacion
    {
        MetodosPagos Insertar(MetodosPagos entidad);
        MetodosPagos Actualizar(MetodosPagos entidad);
        List<MetodosPagos> Consultar();
        List<MetodosPagos> Buscar(MetodosPagos entidad);
        MetodosPagos Borrar(MetodosPagos entidad);
    }
}