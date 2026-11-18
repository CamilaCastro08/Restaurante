using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Cocineros
    {
        public int id { get; set; }
        public int empleado { get; set; }
        public string? especialidad { get; set; }
        public int anios_experiencia { get; set; }
        public string? estacion_asignada { get; set; }


        [ForeignKey("empleado")]
        public Empleados? _empleado { get; set; } 
    }
}
