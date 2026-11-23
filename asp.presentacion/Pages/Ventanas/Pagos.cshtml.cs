using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace asp.presentacion.Pages.Ventanas
{
    public class PagosModel : PaginaProtegida
    {
        private readonly IPagosPresentacion iPresentacion;
        private readonly IFacturasPresentacion facturasPres;
        private readonly IMetodosPagosPresentacion metodosPres;

        public PagosModel(
            IPagosPresentacion iPresentacion,
            IFacturasPresentacion facturasPres,
            IMetodosPagosPresentacion metodosPres)
        {
            this.iPresentacion = iPresentacion;
            this.facturasPres = facturasPres;
            this.metodosPres = metodosPres;
        }

        protected override string Permiso => "Pagos";

        [BindProperty] public Pagos Entidad { get; set; } = new() { fecha = DateTime.Now, estado = "Aprobado" };
        [BindProperty] public Pagos Filtro { get; set; } = new();
        public List<Pagos> Lista { get; set; } = new();
        public List<Facturas> ListaFacturas { get; set; } = new();
        public List<MetodosPagos> ListaMetodosPago { get; set; } = new();
        public string? Mensaje { get; set; }

        public void OnGet(int? facturaId)
        {
            Cargar();

            if (facturaId.HasValue && facturaId.Value > 0)
            {
                var factura = ListaFacturas.FirstOrDefault(f => f.id == facturaId.Value);
                if (factura != null)
                {
                    Entidad.factura = factura.id;
                    Entidad.monto = factura.total;
                    Entidad.fecha = DateTime.Now;
                    Entidad.estado = "Aprobado";
                    Entidad.referencia = $"PAG-{DateTime.Now:yyyyMMddHHmmss}";
                }
            }
        }

        public void OnPostBuscar()
        {
            try
            {
                ListaFacturas = facturasPres.Consultar() ?? new();
                ListaMetodosPago = metodosPres.Consultar() ?? new();
                if (string.IsNullOrEmpty(Filtro.referencia))
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
                if (Entidad.id == 0)
                {
                    iPresentacion.Insertar(Entidad);

                    // Buscar la factura para obtener el ID del pedido y redirigir a Impresión
                    var facturas = facturasPres.Consultar() ?? new();
                    var facturaPagada = facturas.FirstOrDefault(f => f.id == Entidad.factura);

                    TempData["Mensaje"] = "Pago registrado correctamente. El pedido quedó Pagado y la mesa ya está Disponible.";

                    if (facturaPagada != null && facturaPagada.pedido > 0)
                    {
                        return RedirectToPage("/Ventanas/Impresion", new { PedidoId = facturaPagada.pedido });
                    }
                }
                else
                {
                    iPresentacion.Actualizar(Entidad);
                    TempData["Mensaje"] = "Pago actualizado correctamente.";
                }

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
                Lista = (iPresentacion.Consultar() ?? new()).OrderByDescending(x => x.id).ToList();
                ListaFacturas = (facturasPres.Consultar() ?? new()).OrderByDescending(x => x.id).ToList();
                ListaMetodosPago = metodosPres.Consultar() ?? new();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}