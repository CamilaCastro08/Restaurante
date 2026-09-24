using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Facturas
    {
        public int id { get; set; }
        public int pedido { get; set; }
        public string? numero { get; set; }
        public DateTime fecha_emision { get; set; }
        public decimal subtotal { get; set; }
        public decimal total { get; set; }

        public Pedidos? _pedido { get; set; }
        public List<Pagos> Pagos { get; set; }
    }
}
