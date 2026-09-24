using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Pedidos
    {
        public int id { get; set; }
        public int cliente { get; set; }
        public int mesa { get; set; }
        public int mesero { get; set; }
        public DateTime fecha_hora { get; set; }
        public int numero_personas { get; set; }
        public string? estado { get; set; }

        public Clientes? _cliente { get; set; }
        public Mesas? _mesa { get; set; }
        public Meseros? _mesero { get; set; }

        public List<Detalles_Pedidos> Detalles_Pedidos { get; set; }
        public List<Facturas> Facturas { get; set; }
    }
}
