using System.ComponentModel.DataAnnotations.Schema;

namespace Libreria_restaurante.Entidades
{
    public class Productos
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? tipo { get; set; }
        public decimal precio { get; set; }
        public DateTime vigencia_desde { get; set; }
        public DateTime vigencia_hasta { get; set; }
        public int categoria { get; set; }
        public int menu { get; set; }

        [ForeignKey("categoria")] public Categorias? _categoria { get; set; }
        [ForeignKey("menu")] public Menus? _menu { get; set; }
    }
}