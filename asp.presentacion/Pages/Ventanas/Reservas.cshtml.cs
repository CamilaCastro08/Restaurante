using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace asp.presentacion.Pages.Ventanas
{
    public class ReservasModel : PaginaProtegida
    {
        private readonly IReservasPresentacion iPresentacion;

        public ReservasModel(IReservasPresentacion iPresentacion)
        {
            this.iPresentacion = iPresentacion;
        }

        protected override string Permiso => "Reservas";

        [BindProperty] public Reservas Entidad { get; set; } = new() { fecha_hora = DateTime.Now };
        [BindProperty] public Reservas Filtro { get; set; } = new();
        public List<Reservas> Lista { get; set; } = new();
        public string? Mensaje { get; set; }

        public async Task OnGet()
        {
            await Cargar();
        }

        public async Task OnPostBuscar()
        {
            try
            {
                if (string.IsNullOrEmpty(Filtro.estado))
                    Lista = await iPresentacion.Consultar();
                else
                    Lista = await iPresentacion.Buscar(Filtro);
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        public async Task<IActionResult> OnPostGuardar()
        {
            try
            {
                if (Entidad.id == 0)
                    await iPresentacion.Insertar(Entidad);
                else
                    await iPresentacion.Actualizar(Entidad);
                TempData["Mensaje"] = "Guardado";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                await Cargar();
                return Page();
            }
        }

        public async Task OnPostEditar(int id)
        {
            await Cargar();
            ModelState.Clear();
            var item = Lista.FirstOrDefault(x => x.id == id);
            if (item != null)
                Entidad = item;
        }

        public async Task<IActionResult> OnPostBorrar(int id)
        {
            try
            {
                var lista = await iPresentacion.Consultar();
                var item = lista.FirstOrDefault(x => x.id == id);
                if (item != null)
                    await iPresentacion.Borrar(item);
                TempData["Mensaje"] = "Borrado";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                await Cargar();
                return Page();
            }
        }

        private async Task Cargar()
        {
            try
            {
                Lista = await iPresentacion.Consultar();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}
