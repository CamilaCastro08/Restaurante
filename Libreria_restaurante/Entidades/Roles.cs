using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Roles
    {
        [Key] public int id { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public bool activo { get; set; }

        public List<Usuarios>? _Usuarios { get; set; } = new();
        public List<Permisos>? _Permisos { get; set; } = new();
    }
}
