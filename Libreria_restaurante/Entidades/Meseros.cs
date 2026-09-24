using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Meseros
    {
        public int id { get; set; }
        public int empleado { get; set; }
        public string? zona_asignada { get; set; }
        public int num_mesas_asignadas { get; set; }
        public decimal calificacion_promedio { get; set; }


        public Empleados? _empleado { get; set; }

        public List<Pedidos> Pedidos { get; set; }
    }
}
