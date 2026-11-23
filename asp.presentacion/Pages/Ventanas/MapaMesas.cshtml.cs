using asp.presentacion.Nucleo;
using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace asp.presentacion.Pages.Ventanas
{
    public class MapaMesasModel : PaginaProtegida
    {
        private readonly IMesasPresentacion mesasPres;
        private readonly IReservasPresentacion reservasPres;

        public MapaMesasModel(IMesasPresentacion mesasPres, IReservasPresentacion reservasPres)
        {
            this.mesasPres = mesasPres;
            this.reservasPres = reservasPres;
        }

        protected override string Permiso => "MapaMesas";

        public List<Mesas> ListaMesas { get; set; } = new();
        public List<Reservas> ListaReservas { get; set; } = new();

        public int MesasDisponibles => ListaMesas.Count(m => m.estado == "Disponible");
        public int MesasOcupadas => ListaMesas.Count(m => m.estado == "Ocupada");
        public int MesasReservadas => ListaMesas.Count(m => m.estado == "Reservada");
        public int CapacidadTotal => ListaMesas.Sum(m => m.capacidad);

        public string? Mensaje { get; set; }

        public void OnGet()
        {
            CargarDatos();
        }

        public IActionResult OnPostCambiarEstado(int mesaId, string nuevoEstado)
        {
            try
            {
                var mesas = mesasPres.Consultar() ?? new();
                var mesa = mesas.FirstOrDefault(m => m.id == mesaId);
                if (mesa != null)
                {
                    mesa.estado = nuevoEstado;
                    mesasPres.Actualizar(mesa);
                    TempData["Mensaje"] = $"Estado de Mesa {mesa.numero} actualizado a '{nuevoEstado}'.";
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error al actualizar estado: " + ex.Message;
            }

            return RedirectToPage();
        }

        private void CargarDatos()
        {
            try
            {
                ListaMesas = mesasPres.Consultar() ?? new();
                ListaReservas = reservasPres.Consultar() ?? new();
            }
            catch (Exception ex)
            {
                Mensaje = "Error al cargar mesas: " + ex.Message;
            }
        }
    }
}
