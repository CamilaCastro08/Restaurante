using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class MetodosPagos
    {
        public int id { get; set; }
        public string? tipo { get; set; }
        public List<Pagos> Pagos { get; set; }
    }
}
