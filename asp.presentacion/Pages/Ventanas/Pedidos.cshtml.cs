using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;

namespace asp.presentacion.Pages.Ventanas
{
    public class PedidosModel : PaginaProtegida
    {
        private readonly IPedidosPresentacion iPresentacion;
        private readonly IClientesPresentacion clientesPres;
        private readonly IMesasPresentacion mesasPres;
        private readonly IMeserosPresentacion meserosPres;
        private readonly IEmpleadosPresentacion empleadosPres;
        private readonly IDetalles_PedidosPresentacion detallesPres;
        private readonly IProductosPresentacion productosPres;
        private readonly IFacturasPresentacion facturasPres;
        private readonly IAuditoriasPresentacion auditoriasPres;

        public PedidosModel(
            IPedidosPresentacion iPresentacion,
            IClientesPresentacion clientesPres,
            IMesasPresentacion mesasPres,
            IMeserosPresentacion meserosPres,
            IEmpleadosPresentacion empleadosPres,
            IDetalles_PedidosPresentacion detallesPres,
            IProductosPresentacion productosPres,
            IFacturasPresentacion facturasPres,
            IAuditoriasPresentacion auditoriasPres)
        {
            this.iPresentacion = iPresentacion;
            this.clientesPres = clientesPres;
            this.mesasPres = mesasPres;
            this.meserosPres = meserosPres;
            this.empleadosPres = empleadosPres;
            this.detallesPres = detallesPres;
            this.productosPres = productosPres;
            this.facturasPres = facturasPres;
            this.auditoriasPres = auditoriasPres;
        }

        protected override string Permiso => "Pedidos";

        [BindProperty] public Pedidos Entidad { get; set; } = new() { fecha_hora = DateTime.Now, numero_personas = 1, estado = "Pendiente" };
        [BindProperty] public Pedidos Filtro { get; set; } = new();

        public List<Pedidos> Lista { get; set; } = new();
        public List<Clientes> ListaClientes { get; set; } = new();
        public List<Mesas> ListaMesas { get; set; } = new();
        public List<Meseros> ListaMeseros { get; set; } = new();
        public List<Productos> ListaProductos { get; set; } = new();
        public List<Detalles_Pedidos> ListaDetalles { get; set; } = new();
        public List<Facturas> ListaFacturas { get; set; } = new();
        public List<Auditorias> ListaAuditorias { get; set; } = new();
        public int IdClienteGeneral { get; set; }
        public string? Mensaje { get; set; }

        public void OnGet() => Cargar();

        public void OnPostBuscar()
        {
            CargarAuxiliares();
            try
            {
                Lista = string.IsNullOrWhiteSpace(Filtro.estado)
                    ? iPresentacion.Consultar().OrderByDescending(x => x.id).ToList()
                    : iPresentacion.Buscar(Filtro).OrderByDescending(x => x.id).ToList();
            }
            catch (Exception ex) { Mensaje = ex.Message; }
        }

        public IActionResult OnPostGuardar()
        {
            try
            {
                if (Entidad.cliente <= 0)
                    Entidad.cliente = ObtenerOcrearClienteGeneral();

                if (Entidad.fecha_hora == default)
                    Entidad.fecha_hora = DateTime.Now;

                var mesaObj = mesasPres.Consultar().FirstOrDefault(m => m.id == Entidad.mesa);
                var meseroObj = meserosPres.Consultar().FirstOrDefault(m => m.id == Entidad.mesero);

                if (mesaObj == null)
                    throw new Exception("Debe seleccionar una mesa válida.");
                if (meseroObj == null)
                    throw new Exception("Debe seleccionar un mesero válido.");
                if (Entidad.numero_personas <= 0)
                    throw new Exception("El número de personas debe ser mayor a 0.");
                if (Entidad.numero_personas > mesaObj.capacidad)
                    throw new Exception($"La Mesa {mesaObj.numero} tiene capacidad máxima para {mesaObj.capacidad} personas (ingresó {Entidad.numero_personas}).");

                var zonaMesero = Normalizar(meseroObj.zona_asignada);
                var ubicacionMesa = Normalizar(mesaObj.ubicacion);
                if (!string.IsNullOrEmpty(zonaMesero) && !string.IsNullOrEmpty(ubicacionMesa) &&
                    zonaMesero != ubicacionMesa)
                {
                    var emp = empleadosPres.Consultar().FirstOrDefault(e => e.id == meseroObj.empleado);
                    var nomMesero = emp?.nombre ?? $"Mesero #{meseroObj.id}";
                    throw new Exception($"Zona no permitida: {nomMesero} atiende '{meseroObj.zona_asignada}', pero la Mesa {mesaObj.numero} está en '{mesaObj.ubicacion}'.");
                }

                if (Entidad.id == 0 && (mesaObj.estado ?? "").Equals("Ocupada", StringComparison.OrdinalIgnoreCase))
                    throw new Exception($"La Mesa {mesaObj.numero} se encuentra actualmente Ocupada.");

                if (Entidad.id == 0)
                    iPresentacion.Insertar(Entidad);
                else
                    iPresentacion.Actualizar(Entidad);

                TempData["Mensaje"] = "Pedido guardado correctamente.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Cargar();
                return Page();
            }
        }

        public IActionResult OnPostAgregarItems(int pedidoId, int[] productoIds, int[] cantidades, decimal[] subtotales, string[] comentarios)
        {
            try
            {
                if (pedidoId <= 0)
                    throw new Exception("Pedido no válido.");

                if (productoIds == null || productoIds.Length == 0)
                    throw new Exception("Agregue al menos un producto a la lista antes de guardar.");

                for (int i = 0; i < productoIds.Length; i++)
                {
                    if (productoIds[i] <= 0 || cantidades[i] <= 0) continue;

                    detallesPres.Insertar(new Detalles_Pedidos
                    {
                        pedido = pedidoId,
                        producto = productoIds[i],
                        cantidad = cantidades[i],
                        subtotal = subtotales[i],
                        comentarios = (comentarios != null && i < comentarios.Length) ? comentarios[i] : ""
                    });
                }

                TempData["Mensaje"] = $"Productos agregados al Pedido #{pedidoId}.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Cargar();
                return Page();
            }
        }

        public IActionResult OnPostBorrarDetalle(int detalleId)
        {
            try
            {
                var det = detallesPres.Consultar().FirstOrDefault(d => d.id == detalleId);
                if (det != null)
                {
                    det._pedido = null;
                    det._producto = null;
                    detallesPres.Borrar(det);
                }
                TempData["Mensaje"] = "Producto retirado de la orden.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Cargar();
                return Page();
            }
        }

        public IActionResult OnPostFacturar(int pedidoId, decimal porcentajeImpuesto)
        {
            try
            {
                var pedido = iPresentacion.Consultar().FirstOrDefault(p => p.id == pedidoId);
                if (pedido == null)
                    throw new Exception("El pedido no existe.");

                if (facturasPres.Consultar().Any(f => f.pedido == pedidoId))
                    throw new Exception($"El Pedido #{pedidoId} ya tiene una factura generada.");

                var items = detallesPres.Consultar().Where(d => d.pedido == pedidoId).ToList();
                if (items.Count == 0)
                    throw new Exception("No se puede facturar un pedido sin productos. Agregue productos primero.");

                decimal subtotal = items.Sum(x => x.subtotal);
                decimal total = Math.Round(subtotal * (1 + (porcentajeImpuesto / 100m)), 2);
                int consecutivo = facturasPres.Consultar().Count + 1;

                facturasPres.Insertar(new Facturas
                {
                    pedido = pedidoId,
                    numero = $"FAC-{consecutivo:D4}",
                    fecha_emision = DateTime.Now,
                    subtotal = subtotal,
                    total = total
                });

                pedido._cliente = null;
                pedido._mesa = null;
                pedido._mesero = null;
                pedido.estado = "Facturado";
                iPresentacion.Actualizar(pedido);

                TempData["Mensaje"] = $"Factura FAC-{consecutivo:D4} generada por ${total:N0}.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Cargar();
                return Page();
            }
        }

        public IActionResult OnPostBorrar(int id)
        {
            try
            {
                if (detallesPres.Consultar().Any(d => d.pedido == id))
                    throw new Exception("No se puede borrar el pedido porque tiene productos asociados. Retire sus productos primero desde 'Comanda / Cuenta'.");

                if (facturasPres.Consultar().Any(f => f.pedido == id))
                    throw new Exception("No se puede borrar el pedido porque ya tiene una factura generada.");

                var item = iPresentacion.Consultar().FirstOrDefault(x => x.id == id);
                if (item != null)
                {
                    item._cliente = null;
                    item._mesa = null;
                    item._mesero = null;
                    iPresentacion.Borrar(item);
                }

                TempData["Mensaje"] = "Pedido eliminado correctamente.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Cargar();
                return Page();
            }
        }

        public decimal ObtenerPrecioSugerido(Productos p)
        {
            var nom = Normalizar(p.nombre);
            var tipo = Normalizar(p.tipo);

            if (nom.Contains("lomo")) return 48000;
            if (nom.Contains("risotto")) return 42000;
            if (nom.Contains("carpaccio")) return 28000;
            if (nom.Contains("copa") || nom.Contains("vino")) return 18000;
            if (nom.Contains("tiramisu")) return 16000;

            if (tipo.Contains("entrada")) return 22000;
            if (tipo.Contains("fuerte")) return 45000;
            if (tipo.Contains("bebida")) return 12000;
            if (tipo.Contains("postre")) return 15000;
            return 25000;
        }

        private static string Normalizar(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "";
            var norm = texto.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in norm)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private int ObtenerOcrearClienteGeneral()
        {
            var general = clientesPres.Consultar()
                .FirstOrDefault(c => (c.nombre ?? "").Equals("Consumidor Final", StringComparison.OrdinalIgnoreCase));
            if (general != null) return general.id;

            return clientesPres.Insertar(new Clientes
            {
                nombre = "Consumidor Final",
                telefono = "0000000000",
                cedula = "222222222222",
                direccion = "Local"
            }).id;
        }

        private void CargarAuxiliares()
        {
            IdClienteGeneral = ObtenerOcrearClienteGeneral();
            ListaClientes = clientesPres.Consultar().OrderBy(c => c.nombre).ToList();
            ListaMesas = mesasPres.Consultar().OrderBy(m => m.numero).ToList();
            ListaProductos = productosPres.Consultar().OrderBy(p => p.nombre).ToList();
            ListaDetalles = detallesPres.Consultar().ToList();
            ListaFacturas = facturasPres.Consultar().ToList();

            var empleados = empleadosPres.Consultar();
            ListaMeseros = meserosPres.Consultar().OrderBy(m => m.id).ToList();
            foreach (var ms in ListaMeseros)
            {
                if (ms._empleado == null)
                    ms._empleado = empleados.FirstOrDefault(e => e.id == ms.empleado);
            }

            ListaAuditorias = auditoriasPres.Consultar()
                .Where(a => (a.tabla ?? "").Equals("Pedidos", StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(a => a.id).ToList();
        }

        private void Cargar()
        {
            try
            {
                CargarAuxiliares();
                Lista = iPresentacion.Consultar().OrderByDescending(x => x.id).ToList();
            }
            catch (Exception ex) { Mensaje = ex.Message; }
        }
    }
}