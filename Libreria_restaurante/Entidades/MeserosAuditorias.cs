using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class MeserosAuditorias
    {
        public int id { get; set; }
        public string? tabla { get; set; }
        public string? accion { get; set; }
        public DateTime fecha { get; set; }
        public string? datosAnteriores { get; set; }
        public string? datosNuevos { get; set; }
        public int? usuario { get; set; }

        [ForeignKey("usuario")]
        public Usuarios? _Usuario { get; set; }
    }
}
