using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace asp.presentacion.Pages.Ventanas
{
    public class AuditoriasModel : PaginaProtegida
    {
        private readonly IAuditoriasPresentacion iPresentacion;

        public AuditoriasModel(IAuditoriasPresentacion iPresentacion)
        {
            this.iPresentacion = iPresentacion;
        }

        protected override string Permiso => "Auditorias";

        [BindProperty] public Auditorias Filtro { get; set; } = new();
        public List<Auditorias> Lista { get; set; } = new();
        public string? Mensaje { get; set; }

        public void OnGet()
        {
            try
            {
                Lista = (iPresentacion.Consultar()).OrderByDescending(x => x.id).ToList();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        public void OnPostBuscar()
        {
            try
            {
                if (string.IsNullOrEmpty(Filtro.tabla))
                    Lista = iPresentacion.Consultar();
                else
                    Lista = iPresentacion.Buscar(Filtro);
                Lista = Lista.OrderByDescending(x => x.id).ToList();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}
