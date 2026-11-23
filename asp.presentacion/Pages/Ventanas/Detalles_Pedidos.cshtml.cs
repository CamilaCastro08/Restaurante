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

        public Detalles_PedidosModel(
            IDetalles_PedidosPresentacion iPresentacion,
            IPedidosPresentacion pedidosPres,
            IProductosPresentacion productosPres)
        {
            this.iPresentacion = iPresentacion;
            this.pedidosPres = pedidosPres;
            this.productosPres = productosPres;
        }

        protected override string Permiso => "Detalles_Pedidos";

        [BindProperty] public Detalles_Pedidos Entidad { get; set; } = new() { cantidad = 1 };
        [BindProperty] public Detalles_Pedidos Filtro { get; set; } = new();
        public List<Detalles_Pedidos> Lista { get; set; } = new();
        public List<Pedidos> ListaPedidos { get; set; } = new();
        public List<Productos> ListaProductos { get; set; } = new();
        public string? Mensaje { get; set; }

        public void OnGet()
        {
            Cargar();
        }

        public void OnPostBuscar()
        {
            try
            {
                ListaPedidos = pedidosPres.Consultar() ?? new();
                ListaProductos = productosPres.Consultar() ?? new();
                if (Filtro.pedido == 0)
                    Lista = iPresentacion.Consultar();
                else
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
                var productos = productosPres.Consultar() ?? new();
                var prodSeleccionado = productos.FirstOrDefault(p => p.id == Entidad.producto);
                if (prodSeleccionado == null)
                    throw new Exception("Debe seleccionar un producto válido.");

                if (Entidad.cantidad <= 0)
                    throw new Exception("La cantidad debe ser mayor a cero.");

                // Forzar el cálculo exacto en el servidor para evitar manipulación de precios
                Entidad.subtotal = prodSeleccionado.precio * Entidad.cantidad;

                if (Entidad.id == 0)
                    iPresentacion.Insertar(Entidad);
                else
                    iPresentacion.Actualizar(Entidad);

                TempData["Mensaje"] = "Guardado";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Cargar();
                return Page();
            }
        }

        public void OnPostEditar(int id)
        {
            Cargar();
            ModelState.Clear();
            var item = Lista.FirstOrDefault(x => x.id == id);
            if (item != null)
                Entidad = item;
        }

        public IActionResult OnPostBorrar(int id)
        {
            try
            {
                var lista = iPresentacion.Consultar();
                var item = lista.FirstOrDefault(x => x.id == id);
                if (item != null)
                    iPresentacion.Borrar(item);
                TempData["Mensaje"] = "Borrado";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Cargar();
                return Page();
            }
        }

        private void Cargar()
        {
            try
            {
                Lista = iPresentacion.Consultar() ?? new();
                ListaPedidos = (pedidosPres.Consultar() ?? new()).OrderByDescending(p => p.id).ToList();
                ListaProductos = productosPres.Consultar() ?? new();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}