using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Pagos
    {
        public int id { get; set; }
        public int factura { get; set; }
        public int metodoPago { get; set; }
        public decimal monto { get; set; }
        public DateTime fecha { get; set; }
        public string? referencia { get; set; }
        public string? estado { get; set; }


        public Facturas? _factura { get; set; }

        public MetodosPagos? _metodoPago { get; set; }
    }
}
