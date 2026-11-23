using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace asp.presentacion.Pages.Ventanas
{
    public class TurnosModel : PaginaProtegida
    {
        private readonly ITurnosPresentacion iPresentacion;
        private readonly IEmpleadosPresentacion empleadosPres;

        public TurnosModel(ITurnosPresentacion iPresentacion, IEmpleadosPresentacion empleadosPres)
        {
            this.iPresentacion = iPresentacion;
            this.empleadosPres = empleadosPres;
        }

        protected override string Permiso => "Turnos";

        [BindProperty] public Turnos Entidad { get; set; } = new() { fecha = DateTime.Now, hora_entrada = DateTime.Now, hora_salida = DateTime.Now };
        [BindProperty] public Turnos Filtro { get; set; } = new();
        public List<Turnos> Lista { get; set; } = new();
        public List<Empleados> ListaEmpleados { get; set; } = new();
        public string? Mensaje { get; set; }

        public void OnGet()
        {
            Cargar();
        }

        public void OnPostBuscar()
        {
            try
            {
                ListaEmpleados = empleadosPres.Consultar() ?? new();
                if (Filtro.empleado == 0)
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
                ListaEmpleados = empleadosPres.Consultar() ?? new();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}