using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace asp.presentacion.Pages.Ventanas
{
    public class RecetasModel : PaginaProtegida
    {
        private readonly IRecetasPresentacion iPresentacion;
        private readonly IProductosPresentacion productosPres;

        public RecetasModel(IRecetasPresentacion iPresentacion, IProductosPresentacion productosPres)
        {
            this.iPresentacion = iPresentacion;
            this.productosPres = productosPres;
        }

        protected override string Permiso => "Recetas";

        [BindProperty] public Recetas Entidad { get; set; } = new() { tiempo_preparacion = DateTime.Now, porciones = 1 };
        [BindProperty] public Recetas Filtro { get; set; } = new();
        public List<Recetas> Lista { get; set; } = new();
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
                ListaProductos = productosPres.Consultar() ?? new();
                if (string.IsNullOrEmpty(Filtro.instrucciones))
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
                ListaProductos = productosPres.Consultar() ?? new();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}