using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace asp.presentacion.Pages
{
    public class IndexModel : PaginaProtegida
    {
        private readonly IMesasPresentacion mesasPres;
        private readonly IPedidosPresentacion pedidosPres;
        private readonly IFacturasPresentacion facturasPres;
        private readonly IInventariosPresentacion inventariosPres;

        public IndexModel(
            IMesasPresentacion mesasPres,
            IPedidosPresentacion pedidosPres,
            IFacturasPresentacion facturasPres,
            IInventariosPresentacion inventariosPres)
        {
            this.mesasPres = mesasPres;
            this.pedidosPres = pedidosPres;
            this.facturasPres = facturasPres;
            this.inventariosPres = inventariosPres;
        }

        protected override string Permiso => "";

        public int TotalMesas { get; set; }
        public int MesasDisponibles { get; set; }
        public int MesasOcupadas { get; set; }
        public int TotalPedidos { get; set; }
        public decimal TotalVentas { get; set; }
        public int StockCritico { get; set; }

        public void OnGet()
        {
            try
            {
                var mesas = mesasPres.Consultar() ?? new();
                TotalMesas = mesas.Count;
                MesasDisponibles = mesas.Count(m => m.estado == "Disponible");
                MesasOcupadas = mesas.Count(m => m.estado == "Ocupada");

                var pedidos = pedidosPres.Consultar() ?? new();
                TotalPedidos = pedidos.Count;

                var facturas = facturasPres.Consultar() ?? new();
                TotalVentas = facturas.Sum(f => f.total);

                var inventario = inventariosPres.Consultar() ?? new();
                StockCritico = inventario.Count(i => i.stock_actual <= i.stock_minimo);
            }
            catch
            {
                // Silently fallback if services are warming up
            }
        }
    }
}
