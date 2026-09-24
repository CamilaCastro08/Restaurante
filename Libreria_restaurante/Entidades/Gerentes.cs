using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Gerentes
    {
        public int id { get; set; }
        public int empleado { get; set; }
        public string? nivel_autorizacion { get; set; }
        public decimal bono_desempeno { get; set; }
        public string? area_responsable { get; set; }


        public Empleados? _empleado { get; set; }
    }
}
