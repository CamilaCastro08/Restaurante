using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace asp.presentacion.Pages.Ventanas
{
    public class ImpresionModel : PaginaProtegida
    {
        private readonly IPedidosPresentacion pedidosPres;
        private readonly IDetalles_PedidosPresentacion detallesPres;
        private readonly IFacturasPresentacion facturasPres;
        private readonly IProductosPresentacion productosPres;
        private readonly IClientesPresentacion clientesPres;
        private readonly IMesasPresentacion mesasPres;
        private readonly IMeserosPresentacion meserosPres;
        private readonly IEmpleadosPresentacion empleadosPres;

        public ImpresionModel(
            IPedidosPresentacion pedidosPres,
            IDetalles_PedidosPresentacion detallesPres,
            IFacturasPresentacion facturasPres,
            IProductosPresentacion productosPres,
            IClientesPresentacion clientesPres,
            IMesasPresentacion mesasPres,
            IMeserosPresentacion meserosPres,
            IEmpleadosPresentacion empleadosPres)
        {
            this.pedidosPres = pedidosPres;
            this.detallesPres = detallesPres;
            this.facturasPres = facturasPres;
            this.productosPres = productosPres;
            this.clientesPres = clientesPres;
            this.mesasPres = mesasPres;
            this.meserosPres = meserosPres;
            this.empleadosPres = empleadosPres;
        }

        protected override string Permiso => "Impresion";

        [BindProperty(SupportsGet = true)]
        public int? PedidoId { get; set; }

        public List<Pedidos> ListaPedidos { get; set; } = new();
        public Pedidos? PedidoSeleccionado { get; set; }
        public List<Detalles_Pedidos> DetallesPedido { get; set; } = new();
        public Facturas? FacturaAsociada { get; set; }
        public Clientes? ClienteAsociado { get; set; }
        public Mesas? MesaAsociada { get; set; }
        public Empleados? EmpleadoMesero { get; set; }
        public Dictionary<int, string> NombresProductos { get; set; } = new();

        public string? Mensaje { get; set; }

        public void OnGet()
        {
            Cargar();
        }

        private void Cargar()
        {
            try
            {
                ListaPedidos = pedidosPres.Consultar() ?? new();
                var productos = productosPres.Consultar() ?? new();
                NombresProductos = productos.ToDictionary(p => p.id, p => p.nombre ?? $"Producto #{p.id}");

                if (PedidoId.HasValue && PedidoId.Value > 0)
                {
                    PedidoSeleccionado = ListaPedidos.FirstOrDefault(p => p.id == PedidoId.Value);
                }
                else if (ListaPedidos.Any())
                {
                    PedidoSeleccionado = ListaPedidos.First();
                    PedidoId = PedidoSeleccionado.id;
                }

                if (PedidoSeleccionado != null)
                {
                    var todosDetalles = detallesPres.Consultar() ?? new();
                    DetallesPedido = todosDetalles.Where(d => d.pedido == PedidoSeleccionado.id).ToList();

                    var facturas = facturasPres.Consultar() ?? new();
                    FacturaAsociada = facturas.FirstOrDefault(f => f.pedido == PedidoSeleccionado.id);

                    var clientes = clientesPres.Consultar() ?? new();
                    ClienteAsociado = clientes.FirstOrDefault(c => c.id == PedidoSeleccionado.cliente);

                    var mesas = mesasPres.Consultar() ?? new();
                    MesaAsociada = mesas.FirstOrDefault(m => m.id == PedidoSeleccionado.mesa);

                    var meseros = meserosPres.Consultar() ?? new();
                    var mesero = meseros.FirstOrDefault(m => m.id == PedidoSeleccionado.mesero);
                    if (mesero != null)
                    {
                        var empleados = empleadosPres.Consultar() ?? new();
                        EmpleadoMesero = empleados.FirstOrDefault(e => e.id == mesero.empleado);
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error al cargar datos de impresión: " + ex.Message;
            }
        }
    }
}
