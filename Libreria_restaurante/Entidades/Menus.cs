using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Menus
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public DateTime vigencia_desde { get; set; }
        public DateTime vigencia_hasta { get; set; }
        public string? tipo { get; set; }

        public List<Productos> Productos { get; set; }
    }
}
