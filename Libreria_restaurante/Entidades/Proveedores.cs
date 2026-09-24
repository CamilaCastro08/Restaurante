using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Proveedores
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? contacto { get; set; }
        public string? direccion { get; set; }
        public string? telefono { get; set; }


        public List<Ingredientes> Ingredientes { get; set; }
    }
}
