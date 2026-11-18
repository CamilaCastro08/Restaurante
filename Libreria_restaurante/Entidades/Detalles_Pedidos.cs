using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Detalles_Pedidos
    {
        public int id { get; set; }
        public int pedido { get; set; }
        public int producto { get; set; }
        public int cantidad { get; set; }
        public decimal subtotal { get; set; }
        public string? comentarios { get; set; }

        [ForeignKey("pedido")]
        public Pedidos? _pedido { get; set; }
        [ForeignKey("producto")]
        public Productos? _producto { get; set; }
    }
}
