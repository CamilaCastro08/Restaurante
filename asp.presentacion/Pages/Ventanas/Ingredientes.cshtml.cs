using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace asp.presentacion.Pages.Ventanas
{
    public class IngredientesModel : PaginaProtegida
    {
        private readonly IIngredientesPresentacion iPresentacion;
        private readonly IProveedoresPresentacion proveedoresPres;
        private readonly IRecetasPresentacion recetasPres;
        private readonly IProductosPresentacion productosPres;

        public IngredientesModel(
            IIngredientesPresentacion iPresentacion,
            IProveedoresPresentacion proveedoresPres,
            IRecetasPresentacion recetasPres,
            IProductosPresentacion productosPres)
        {
            this.iPresentacion = iPresentacion;
            this.proveedoresPres = proveedoresPres;
            this.recetasPres = recetasPres;
            this.productosPres = productosPres;
        }

        protected override string Permiso => "Ingredientes";

        [BindProperty] public Ingredientes Entidad { get; set; } = new();
        [BindProperty] public Ingredientes Filtro { get; set; } = new();
        public List<Ingredientes> Lista { get; set; } = new();
        public List<Proveedores> ListaProveedores { get; set; } = new();
        public List<Recetas> ListaRecetas { get; set; } = new();
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
                CargarAuxiliares();
                if (string.IsNullOrEmpty(Filtro.nombre))
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

        public string DescripcionReceta(int recetaId)
        {
            var rec = ListaRecetas.FirstOrDefault(r => r.id == recetaId);
            if (rec == null) return $"Receta #{recetaId}";
            var prod = ListaProductos.FirstOrDefault(p => p.id == rec.producto);
            return $"Receta #{rec.id} - {(prod?.nombre ?? "Producto")}";
        }

        private void Cargar()
        {
            try
            {
                Lista = iPresentacion.Consultar() ?? new();
                CargarAuxiliares();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        private void CargarAuxiliares()
        {
            ListaProveedores = proveedoresPres.Consultar() ?? new();
            ListaRecetas = recetasPres.Consultar() ?? new();
            ListaProductos = productosPres.Consultar() ?? new();
        }
    }
}