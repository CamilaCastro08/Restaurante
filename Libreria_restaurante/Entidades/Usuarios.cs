using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Usuarios
    {
        [Key] public int id { get; set; }
        public string? nombreUsuario { get; set; }
        public string? correo { get; set; }
        public string? contraseña { get; set; }
        public bool activo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int? rol { get; set; }

        public Roles? _rol { get; set; }

        public List<Auditorias>? _Auditorias { get; set; } = new();
    }
}
