using System;
using System.Collections.Generic;
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


        public Empleados? _empleado { get; set; }
    }
}
