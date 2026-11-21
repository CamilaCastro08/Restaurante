using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace asp.presentacion.Pages.Ventanas
{
    public class Detalles_PedidosModel : PaginaProtegida
    {
        private readonly IDetalles_PedidosPresentacion iPresentacion;
        private readonly IPedidosPresentacion pedidosPres;
        private readonly IProductosPresentacion productosPres;
        private readonly IAuditoriasPresentacion auditoriasPres;

        public Detalles_PedidosModel(
            IDetalles_PedidosPresentacion iPresentacion,
            IPedidosPresentacion pedidosPres,
            IProductosPresentacion productosPres,
            IAuditoriasPresentacion auditoriasPres)
        {
            this.iPresentacion = iPresentacion;
            this.pedidosPres = pedidosPres;
            this.productosPres = productosPres;
            this.auditoriasPres = auditoriasPres;
        }

        protected override string Permiso => "Detalles_Pedidos";

        [BindProperty] public Detalles_Pedidos Entidad { get; set; } = new() { cantidad = 1 };
        [BindProperty] public Detalles_Pedidos Filtro { get; set; } = new();

        public List<Detalles_Pedidos> Lista { get; set; } = new();
        public List<Pedidos> ListaPedidos { get; set; } = new();
        public List<Productos> ListaProductos { get; set; } = new();
        public List<Auditorias> ListaAuditorias { get; set; } = new();
        public string? Mensaje { get; set; }

        public void OnGet() => Cargar();

        public void OnPostBuscar()
        {
            CargarAuxiliares();
            try
            {
                Lista = Filtro.pedido <= 0
                    ? iPresentacion.Consultar().OrderByDescending(x => x.id).ToList()
                    : iPresentacion.Buscar(Filtro).OrderByDescending(x => x.id).ToList();
            }
            catch (Exception ex) { Mensaje = ex.Message; }
        }

        public IActionResult OnPostGuardar()
        {
            try
            {
                if (Entidad.pedido <= 0) throw new Exception("Seleccione un pedido válido.");
                if (Entidad.producto <= 0) throw new Exception("Seleccione un producto válido.");
                if (Entidad.cantidad <= 0) throw new Exception("La cantidad debe ser mayor a 0.");
                if (Entidad.subtotal < 0) throw new Exception("El subtotal no puede ser negativo.");

                if (Entidad.id == 0) iPresentacion.Insertar(Entidad);
                else iPresentacion.Actualizar(Entidad);

                TempData["Mensaje"] = "Detalle de pedido guardado.";
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
                var item = iPresentacion.Consultar().FirstOrDefault(x => x.id == id);
                if (item != null)
                {
                    item._pedido = null;
                    item._producto = null;
                    iPresentacion.Borrar(item);
                }
                TempData["Mensaje"] = "Detalle eliminado.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Cargar();
                return Page();
            }
        }

        private void CargarAuxiliares()
        {
            ListaPedidos = pedidosPres.Consultar().OrderByDescending(p => p.id).ToList();
            ListaProductos = productosPres.Consultar().OrderBy(p => p.nombre).ToList();
            ListaAuditorias = auditoriasPres.Consultar()
                .Where(a => (a.tabla ?? "").Equals("Detalles_Pedidos", StringComparison.OrdinalIgnoreCase))
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