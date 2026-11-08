using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class IDetalles_PedidosPresentacion
    {
        Detalles_Pedidos Insertar(Detalles_Pedidos entidad);
        Detalles_Pedidos Actualizar(Detalles_Pedidos entidad);
        List<Detalles_Pedidos> Consultar();
        List<Detalles_Pedidos> Buscar(Detalles_Pedidos entidad);
        Detalles_Pedidos Borrar(Detalles_Pedidos entidad);
    }
}
