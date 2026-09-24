using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Empleados
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? telefono { get; set; }
        public string? direccion { get; set; }
        public decimal salario { get; set; }
        public DateTime fecha_contratacion { get; set; }
        public string? cargo { get; set; }


        public List<Turnos> Turnos { get; set; }
        public List<Meseros> Meseros { get; set; }
        public List<Gerentes> Gerentes { get; set; }
        public List<Cocineros> Cocineros { get; set; }
    }
}
