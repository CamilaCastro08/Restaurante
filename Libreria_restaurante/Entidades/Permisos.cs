using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    internal class Permisos
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public int? rol { get; set; }

        public Roles? _rol { get; set; }
    }
}
