using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Ingredientes
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? unidad_medida { get; set; }
        public decimal costo_unitario { get; set; }
        public int proveedor { get; set; }
        public int receta { get; set; }

        [ForeignKey("receta")]
        public Recetas? _receta { get; set; }
        [ForeignKey("proveedor")]
        public Proveedores? _proveedor { get; set; }
        public List<Inventarios> Inventarios { get; set; } = new();

    }
}
