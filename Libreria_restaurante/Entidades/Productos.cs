using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Entidades
{
    public class Productos
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public DateTime vigencia_desde { get; set; }
        public DateTime vigencia_hasta { get; set; }
        public string? tipo { get; set; }
        public int categoria { get; set; }
        public int menu { get; set; }

        public Categorias? _categoria { get; set; }
        public Menus? _menu { get; set; }

        public List<Recetas> Recetas { get; set; }
        public List<Detalles_Pedidos> DetallesPedidos { get; set; }
    }
}
