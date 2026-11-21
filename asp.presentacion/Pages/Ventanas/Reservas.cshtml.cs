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
        private readonly IAuditoriasPresentacion auditoriasPres;

        public ReservasModel(
            IReservasPresentacion iPresentacion,
            IClientesPresentacion clientesPres,
            IMesasPresentacion mesasPres,
            IAuditoriasPresentacion auditoriasPres)
        {
            this.iPresentacion = iPresentacion;
            this.clientesPres = clientesPres;
            this.mesasPres = mesasPres;
            this.auditoriasPres = auditoriasPres;
        }

        protected override string Permiso => "Reservas";

        [BindProperty] public Reservas Entidad { get; set; } = new() { fecha_hora = DateTime.Now, numero_personas = 2, estado = "Confirmada" };
        [BindProperty] public Reservas Filtro { get; set; } = new();

        public List<Reservas> Lista { get; set; } = new();
        public List<Clientes> ListaClientes { get; set; } = new();
        public List<Mesas> ListaMesas { get; set; } = new();
        public List<Auditorias> ListaAuditorias { get; set; } = new();
        public string? Mensaje { get; set; }

        public void OnGet() => Cargar();

        public void OnPostBuscar()
        {
            CargarAuxiliares();
            try
            {
                Lista = string.IsNullOrWhiteSpace(Filtro.estado)
                    ? iPresentacion.Consultar().OrderByDescending(x => x.id).ToList()
                    : iPresentacion.Buscar(Filtro).OrderByDescending(x => x.id).ToList();
            }
            catch (Exception ex) { Mensaje = ex.Message; }
        }

        public IActionResult OnPostGuardar()
        {
            try
            {
                var mesaObj = mesasPres.Consultar().FirstOrDefault(m => m.id == Entidad.mesa);
                if (Entidad.cliente <= 0) throw new Exception("Debe seleccionar un cliente.");
                if (mesaObj == null) throw new Exception("Debe seleccionar una mesa.");
                if (Entidad.numero_personas <= 0) throw new Exception("El número de personas debe ser mayor a 0.");
                if (Entidad.numero_personas > mesaObj.capacidad)
                    throw new Exception($"La Mesa {mesaObj.numero} solo admite hasta {mesaObj.capacidad} personas.");

                var cruce = iPresentacion.Consultar().Any(r =>
                    r.id != Entidad.id &&
                    r.mesa == Entidad.mesa &&
                    !(r.estado ?? "").Equals("Cancelada", StringComparison.OrdinalIgnoreCase) &&
                    Math.Abs((r.fecha_hora - Entidad.fecha_hora).TotalHours) < 2);

                if (cruce)
                    throw new Exception($"La Mesa {mesaObj.numero} ya tiene una reserva cercana a esa hora (±2h).");

                if (Entidad.id == 0) iPresentacion.Insertar(Entidad);
                else iPresentacion.Actualizar(Entidad);

                TempData["Mensaje"] = "Reserva guardada correctamente.";
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
                    item._cliente = null;
                    item._mesa = null;
                    iPresentacion.Borrar(item);
                }
                TempData["Mensaje"] = "Reserva eliminada.";
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
            ListaClientes = clientesPres.Consultar().OrderBy(c => c.nombre).ToList();
            ListaMesas = mesasPres.Consultar().OrderBy(m => m.numero).ToList();
            ListaAuditorias = auditoriasPres.Consultar()
                .Where(a => (a.tabla ?? "").Equals("Reservas", StringComparison.OrdinalIgnoreCase))
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