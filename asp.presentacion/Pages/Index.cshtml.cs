using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace asp.presentacion.Pages
{
    public class IndexModel : PaginaProtegida
    {
        private readonly IPedidosPresentacion pedidos;
        private readonly IReservasPresentacion reservas;
        private readonly IMesasPresentacion mesas;
        private readonly IProductosPresentacion productos;
        private readonly IClientesPresentacion clientes;

        public IndexModel(
            IPedidosPresentacion pedidos,
            IReservasPresentacion reservas,
            IMesasPresentacion mesas,
            IProductosPresentacion productos,
            IClientesPresentacion clientes)
        {
            this.pedidos = pedidos;
            this.reservas = reservas;
            this.mesas = mesas;
            this.productos = productos;
            this.clientes = clientes;
        }

        protected override string Permiso => "";

        public int TotalPedidosHoy { get; set; }
        public int TotalPedidos { get; set; }
        public int TotalReservas { get; set; }
        public int TotalMesas { get; set; }
        public int TotalProductos { get; set; }
        public int TotalClientes { get; set; }
        public List<Pedidos> UltimosPedidos { get; set; } = new();

        public void OnGet()
        {
            var listaPedidos = pedidos.Consultar();
            TotalPedidos = listaPedidos.Count;
            TotalPedidosHoy = listaPedidos.Count(p => p.fecha_hora.Date == DateTime.Today);
            TotalReservas = reservas.Consultar().Count;
            TotalMesas = mesas.Consultar().Count;
            TotalProductos = productos.Consultar().Count;
            TotalClientes = clientes.Consultar().Count;

            UltimosPedidos = listaPedidos
                .OrderByDescending(p => p.id)
                .Take(8)
                .ToList();
        }
    }
}