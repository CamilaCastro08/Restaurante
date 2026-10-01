using System;
using System.Collections.Generic;
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

        public Pedidos? _pedido { get; set; }
        public Productos? _producto { get; set; }
    }
}
