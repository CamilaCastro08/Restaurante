using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Clientes
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? telefono { get; set; }
        public string? cedula { get; set; }
        public string? direccion { get; set; }

        public List<Reservas> Reservas { get; set; }
        public List<Pedidos> Pedidos { get; set; }
    }
}
