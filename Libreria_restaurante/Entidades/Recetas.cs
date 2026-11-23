using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Libreria_restaurante.Entidades
{
  
        public class Recetas
        {
            public int id { get; set; }
            public int producto { get; set; }
            public DateTime tiempo_preparacion { get; set; }
            public string? instrucciones { get; set; }
            public int porciones { get; set; }
            public string? dificultad { get; set; }


            [ForeignKey("producto")]
            public Productos? _producto { get; set; }
            public List<Ingredientes> Ingredientes { get; set; } = new();
    }
}

