using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace asp.presentacion.Pages.Ventanas
{
    public class ReservasModel : PaginaProtegida
    {
        private readonly IReservasPresentacion iPresentacion;
        private readonly IClientesPresentacion clientesPres;
        private readonly IMesasPresentacion mesasPres;

        public ReservasModel(IReservasPresentacion iPresentacion, IClientesPresentacion clientesPres, IMesasPresentacion mesasPres)
        {
            this.iPresentacion = iPresentacion;
            this.clientesPres = clientesPres;
            this.mesasPres = mesasPres;
        }

        protected override string Permiso => "Reservas";

        [BindProperty] public Reservas Entidad { get; set; } = new() { fecha_hora = DateTime.Now, numero_personas = 2, estado = "Confirmada" };
        [BindProperty] public Reservas Filtro { get; set; } = new();
        public List<Reservas> Lista { get; set; } = new();
        public List<Clientes> ListaClientes { get; set; } = new();
        public List<Mesas> ListaMesas { get; set; } = new();
        public string? Mensaje { get; set; }

        public void OnGet()
        {
            Cargar();
        }

        public void OnPostBuscar()
        {
            try
            {
                ListaClientes = clientesPres.Consultar() ?? new();
                ListaMesas = mesasPres.Consultar() ?? new();
                if (string.IsNullOrEmpty(Filtro.estado))
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
                ListaClientes = clientesPres.Consultar() ?? new();
                ListaMesas = mesasPres.Consultar() ?? new();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}