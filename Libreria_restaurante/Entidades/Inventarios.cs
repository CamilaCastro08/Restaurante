using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Inventarios
    {
        public int id { get; set; }
        public int ingrediente { get; set; }
        public decimal stock_actual { get; set; }
        public decimal stock_minimo { get; set; }
        public DateTime fecha_actualizacion { get; set; }


        [ForeignKey("ingrediente")]
        public Ingredientes? _ingrediente { get; set; }
    }
}
