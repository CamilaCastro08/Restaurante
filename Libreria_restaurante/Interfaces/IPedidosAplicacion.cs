using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Interfaces
{
    public class IPedidosAplicacion
    {
        Pedidos Insertar(Pedidos entidad);
        Pedidos Actualizar(Pedidos entidad);
        List<Pedidos> Consultar();
        List<Pedidos> Buscar(Pedidos entidad);
        Pedidos Borrar(Pedidos entidad);
    }
}