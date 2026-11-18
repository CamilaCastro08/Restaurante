using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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


        [ForeignKey("factura")]
        public Facturas? _factura { get; set; }

        [ForeignKey("metodoPago")]
        public MetodosPagos? _metodoPago { get; set; }
    }
}
