using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{

    public class Categorias
    {
        public int id { get; set; }
        public string? nombre { get; set; }

        public List<Productos> Productos { get; set; }
    }
}
