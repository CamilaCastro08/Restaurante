using System.Text;
using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace asp.presentacion.Pages.Ventanas
{
    public class ReportesModel : PaginaProtegida
    {
        private readonly IFacturasPresentacion facturasPres;
        private readonly IInventariosPresentacion inventariosPres;
        private readonly IIngredientesPresentacion ingredientesPres;
        private readonly IMesasPresentacion mesasPres;
        private readonly IPedidosPresentacion pedidosPres;

        public ReportesModel(
            IFacturasPresentacion facturasPres,
            IInventariosPresentacion inventariosPres,
            IIngredientesPresentacion ingredientesPres,
            IMesasPresentacion mesasPres,
            IPedidosPresentacion pedidosPres)
        {
            this.facturasPres = facturasPres;
            this.inventariosPres = inventariosPres;
            this.ingredientesPres = ingredientesPres;
            this.mesasPres = mesasPres;
            this.pedidosPres = pedidosPres;
        }

        protected override string Permiso => "Reportes";

        [BindProperty(SupportsGet = true)]
        public string TipoReporte { get; set; } = "Ventas";

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaDesde { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaHasta { get; set; }

        public List<Facturas> ListaFacturas { get; set; } = new();
        public List<Inventarios> ListaInventarios { get; set; } = new();
        public List<Mesas> ListaMesas { get; set; } = new();

        public decimal TotalVentas { get; set; }
        public decimal PromedioVentas { get; set; }
        public int TotalItems { get; set; }
        public string? Mensaje { get; set; }

        public void OnGet()
        {
            CargarDatos();
        }

        public IActionResult OnGetExportarExcel(string tipo, DateTime? desde, DateTime? hasta)
        {
            TipoReporte = tipo ?? "Ventas";
            FechaDesde = desde;
            FechaHasta = hasta;
            CargarDatos();

            var sb = new StringBuilder();

            if (TipoReporte == "Ventas")
            {
                sb.AppendLine("ID;Numero Factura;Fecha Emision;Subtotal;Total");
                foreach (var f in ListaFacturas)
                {
                    sb.AppendLine($"{f.id};{f.numero};{f.fecha_emision:yyyy-MM-dd HH:mm};{f.subtotal:F2};{f.total:F2}");
                }
                sb.AppendLine($";;;TOTAL;{TotalVentas:F2}");
                var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
                return File(bytes, "text/csv; charset=utf-8", $"Reporte_Ventas_{DateTime.Now:yyyyMMdd_HHmm}.csv");
            }
            else if (TipoReporte == "Inventario")
            {
                sb.AppendLine("ID;ID Ingrediente;Stock Actual;Stock Minimo;Fecha Actualizacion;Alerta");
                foreach (var inv in ListaInventarios)
                {
                    string alerta = inv.stock_actual <= inv.stock_minimo ? "CRITICO" : "OPTIMO";
                    sb.AppendLine($"{inv.id};{inv.ingrediente};{inv.stock_actual:F2};{inv.stock_minimo:F2};{inv.fecha_actualizacion:yyyy-MM-dd HH:mm};{alerta}");
                }
                var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
                return File(bytes, "text/csv; charset=utf-8", $"Reporte_Inventario_{DateTime.Now:yyyyMMdd_HHmm}.csv");
            }
            else
            {
                sb.AppendLine("ID;Numero Mesa;Capacidad Personas;Estado;Ubicacion");
                foreach (var m in ListaMesas)
                {
                    sb.AppendLine($"{m.id};{m.numero};{m.capacidad};{m.estado};{m.ubicacion}");
                }
                var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
                return File(bytes, "text/csv; charset=utf-8", $"Reporte_Mesas_{DateTime.Now:yyyyMMdd_HHmm}.csv");
            }
        }

        private void CargarDatos()
        {
            try
            {
                if (TipoReporte == "Ventas")
                {
                    var facturas = facturasPres.Consultar() ?? new();
                    if (FechaDesde.HasValue)
                        facturas = facturas.Where(x => x.fecha_emision >= FechaDesde.Value).ToList();
                    if (FechaHasta.HasValue)
                        facturas = facturas.Where(x => x.fecha_emision <= FechaHasta.Value.AddDays(1)).ToList();

                    ListaFacturas = facturas.OrderByDescending(x => x.fecha_emision).ToList();
                    TotalVentas = ListaFacturas.Sum(x => x.total);
                    TotalItems = ListaFacturas.Count;
                    PromedioVentas = TotalItems > 0 ? TotalVentas / TotalItems : 0;
                }
                else if (TipoReporte == "Inventario")
                {
                    ListaInventarios = inventariosPres.Consultar() ?? new();
                    TotalItems = ListaInventarios.Count;
                }
                else if (TipoReporte == "Mesas")
                {
                    ListaMesas = mesasPres.Consultar() ?? new();
                    TotalItems = ListaMesas.Count;
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error al cargar reporte: " + ex.Message;
            }
        }
    }
}
