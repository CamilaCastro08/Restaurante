using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Mesas
    {
        public int id { get; set; }
        public int numero { get; set; }
        public int capacidad { get; set; }
        public string? estado { get; set; }
        public string? ubicacion { get; set; }

        public List<Reservas> Reservas { get; set; }
        public List<Pedidos> Pedidos { get; set; }
    }
}
