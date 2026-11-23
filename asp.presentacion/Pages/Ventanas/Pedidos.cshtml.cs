using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace asp.presentacion.Pages.Ventanas
{
    public class PedidosModel : PaginaProtegida
    {
        private readonly IPedidosPresentacion iPresentacion;
        private readonly IClientesPresentacion clientesPres;
        private readonly IMesasPresentacion mesasPres;
        private readonly IMeserosPresentacion meserosPres;
        private readonly IEmpleadosPresentacion empleadosPres;
        private readonly IProductosPresentacion productosPres;
        private readonly IRecetasPresentacion recetasPres;
        private readonly IDetalles_PedidosPresentacion detallesPres;
        private readonly IFacturasPresentacion facturasPres;

        public PedidosModel(
            IPedidosPresentacion iPresentacion,
            IClientesPresentacion clientesPres,
            IMesasPresentacion mesasPres,
            IMeserosPresentacion meserosPres,
            IEmpleadosPresentacion empleadosPres,
            IProductosPresentacion productosPres,
            IRecetasPresentacion recetasPres,
            IDetalles_PedidosPresentacion detallesPres,
            IFacturasPresentacion facturasPres)
        {
            this.iPresentacion = iPresentacion;
            this.clientesPres = clientesPres;
            this.mesasPres = mesasPres;
            this.meserosPres = meserosPres;
            this.empleadosPres = empleadosPres;
            this.productosPres = productosPres;
            this.recetasPres = recetasPres;
            this.detallesPres = detallesPres;
            this.facturasPres = facturasPres;
        }

        protected override string Permiso => "Pedidos";

        [BindProperty] public Pedidos Entidad { get; set; } = new() { fecha_hora = DateTime.Now, numero_personas = 2, estado = "En preparación" };
        [BindProperty] public Pedidos Filtro { get; set; } = new();
        [BindProperty] public Detalles_Pedidos NuevoDetalle { get; set; } = new() { cantidad = 1 };

        public List<Pedidos> Lista { get; set; } = new();
        public List<Clientes> ListaClientes { get; set; } = new();
        public List<Mesas> ListaMesas { get; set; } = new();
        public List<Meseros> ListaMeseros { get; set; } = new();
        public List<Empleados> ListaEmpleados { get; set; } = new();
        public List<Productos> ListaProductos { get; set; } = new();
        public List<Recetas> ListaRecetas { get; set; } = new();
        public List<Detalles_Pedidos> DetallesPedidoActual { get; set; } = new();

        public string? Mensaje { get; set; }

        public void OnGet(int? id, int? mesaId)
        {
            CargarListasAuxiliares();

            if (mesaId.HasValue && mesaId.Value > 0)
            {
                var pedidoActivo = Lista.FirstOrDefault(p => p.mesa == mesaId.Value && (p.estado == "En preparación" || p.estado == "Servido"));
                if (pedidoActivo != null)
                {
                    Entidad = pedidoActivo;
                    CargarDetalles(pedidoActivo.id);
                    return;
                }
                Entidad.mesa = mesaId.Value;
            }

            if (id.HasValue && id.Value > 0)
            {
                var item = Lista.FirstOrDefault(x => x.id == id.Value);
                if (item != null)
                {
                    Entidad = item;
                    CargarDetalles(item.id);
                }
            }
        }

        public void OnPostBuscar()
        {
            try
            {
                CargarListasAuxiliares();
                if (!string.IsNullOrEmpty(Filtro.estado))
                    Lista = iPresentacion.Buscar(Filtro);
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        public IActionResult OnPostGuardar()
        {
            try
            {
                if (Entidad.id == 0)
                {
                    var creado = iPresentacion.Insertar(Entidad);
                    TempData["Mensaje"] = $"Pedido #{creado.id} abierto. Agrega los platos en el panel derecho.";
                    return RedirectToPage(new { id = creado.id });
                }
                else
                {
                    iPresentacion.Actualizar(Entidad);
                    TempData["Mensaje"] = $"Datos del pedido #{Entidad.id} actualizados.";
                    return RedirectToPage(new { id = Entidad.id });
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                CargarListasAuxiliares();
                if (Entidad.id > 0) CargarDetalles(Entidad.id);
                return Page();
            }
        }

        public IActionResult OnPostCambiarEstadoPedido(int pedidoId, string nuevoEstado)
        {
            try
            {
                var todos = iPresentacion.Consultar();
                var pedido = todos.FirstOrDefault(p => p.id == pedidoId);
                if (pedido != null)
                {
                    pedido.estado = nuevoEstado;
                    iPresentacion.Actualizar(pedido);
                    TempData["Mensaje"] = $"Pedido #{pedidoId} marcado como '{nuevoEstado}'.";
                }
                return RedirectToPage(new { id = pedidoId });
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                return RedirectToPage(new { id = pedidoId });
            }
        }

        public IActionResult OnPostAgregarProducto()
        {
            try
            {
                if (NuevoDetalle.pedido == 0)
                    throw new Exception("Primero abre el pedido antes de agregar productos.");

                NuevoDetalle.id = 0;
                detallesPres.Insertar(NuevoDetalle);
                TempData["Mensaje"] = "Producto agregado al pedido.";
                return RedirectToPage(new { id = NuevoDetalle.pedido });
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                CargarListasAuxiliares();
                var pedido = Lista.FirstOrDefault(x => x.id == NuevoDetalle.pedido);
                if (pedido != null)
                {
                    Entidad = pedido;
                    CargarDetalles(pedido.id);
                }
                return Page();
            }
        }

        public IActionResult OnPostQuitarProducto(int detalleId, int pedidoId)
        {
            try
            {
                var todos = detallesPres.Consultar();
                var item = todos.FirstOrDefault(x => x.id == detalleId);
                if (item != null)
                    detallesPres.Borrar(item);

                TempData["Mensaje"] = "Producto quitado del pedido.";
                return RedirectToPage(new { id = pedidoId });
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                return RedirectToPage(new { id = pedidoId });
            }
        }

        public IActionResult OnPostFacturarPedido(int pedidoId)
        {
            try
            {
                var factura = new Facturas
                {
                    pedido = pedidoId,
                    fecha_emision = DateTime.Now
                };
                facturasPres.Insertar(factura);
                TempData["Mensaje"] = $"Pedido #{pedidoId} cobrado y mesa liberada.";
                return RedirectToPage("/Ventanas/Impresion", new { PedidoId = pedidoId });
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                return RedirectToPage(new { id = pedidoId });
            }
        }

        public IActionResult OnPostBorrar(int id)
        {
            try
            {
                var lista = iPresentacion.Consultar();
                var item = lista.FirstOrDefault(x => x.id == id);
                if (item != null)
                    iPresentacion.Borrar(item);
                TempData["Mensaje"] = "Pedido eliminado y mesa liberada.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                CargarListasAuxiliares();
                return Page();
            }
        }

        public string NombreMesero(int meseroId)
        {
            var mesero = ListaMeseros.FirstOrDefault(m => m.id == meseroId);
            if (mesero == null) return $"Mesero #{meseroId}";
            var emp = ListaEmpleados.FirstOrDefault(e => e.id == mesero.empleado);
            return emp != null ? emp.nombre! : $"Mesero #{meseroId}";
        }

        private void CargarListasAuxiliares()
        {
            try
            {
                Lista = (iPresentacion.Consultar() ?? new()).OrderByDescending(x => x.id).ToList();
                ListaClientes = clientesPres.Consultar() ?? new();
                ListaMesas = mesasPres.Consultar() ?? new();
                ListaMeseros = meserosPres.Consultar() ?? new();
                ListaEmpleados = empleadosPres.Consultar() ?? new();
                ListaProductos = productosPres.Consultar() ?? new();
                ListaRecetas = recetasPres.Consultar() ?? new();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        private void CargarDetalles(int pedidoId)
        {
            var todos = detallesPres.Consultar() ?? new();
            DetallesPedidoActual = todos.Where(d => d.pedido == pedidoId).ToList();
            NuevoDetalle.pedido = pedidoId;
        }
    }
}