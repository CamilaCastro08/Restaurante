using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace asp.presentacion.Pages.Ventanas
{
    public class InventariosModel : PaginaProtegida
    {
        private readonly IInventariosPresentacion iPresentacion;
        private readonly IIngredientesPresentacion ingredientesPres;

        public InventariosModel(IInventariosPresentacion iPresentacion, IIngredientesPresentacion ingredientesPres)
        {
            this.iPresentacion = iPresentacion;
            this.ingredientesPres = ingredientesPres;
        }

        protected override string Permiso => "Inventarios";

        [BindProperty] public Inventarios Entidad { get; set; } = new() { fecha_actualizacion = DateTime.Now };
        [BindProperty] public Inventarios Filtro { get; set; } = new();
        public List<Inventarios> Lista { get; set; } = new();
        public List<Ingredientes> ListaIngredientes { get; set; } = new();
        public string? Mensaje { get; set; }

        public void OnGet()
        {
            Cargar();
        }

        public void OnPostBuscar()
        {
            try
            {
                ListaIngredientes = ingredientesPres.Consultar() ?? new();
                if (Filtro.ingrediente == 0)
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
                ListaIngredientes = ingredientesPres.Consultar() ?? new();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}