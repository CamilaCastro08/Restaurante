using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Turnos
    {
        public int id { get; set; }
        public int empleado { get; set; }
        public DateTime fecha { get; set; }
        public DateTime hora_entrada { get; set; }
        public DateTime hora_salida { get; set; }

        public Empleados? _empleado { get; set; }
    }
}
