using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Reservas
    {
        public int id { get; set; }
        public int cliente { get; set; }
        public int mesa { get; set; }
        public DateTime fecha_hora { get; set; }
        public int numero_personas { get; set; }
        public string? estado { get; set; }


        [ForeignKey("cliente")]
        public Clientes? _cliente { get; set; }
        [ForeignKey("mesa")]
        public Mesas? _mesa { get; set; }
    }
}
