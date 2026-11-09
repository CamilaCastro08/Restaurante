using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp.servicios.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Detalles_PedidosController : ControllerBase
    {
        private IDetalles_PedidosAplicacion IDetalles_PedidosAplicacion;

        public Detalles_PedidosController(IDetalles_PedidosAplicacion IDetalles_PedidosAplicacion)
        {
            this.IDetalles_PedidosAplicacion = IDetalles_PedidosAplicacion;
        }

        [HttpPost]
        public List<Detalles_Pedidos> Consultar()
        {
            return this.IDetalles_PedidosAplicacion.Consultar();
        }

        [HttpPost]
        public List<Detalles_Pedidos> Buscar(Detalles_Pedidos entidad)
        {
            return this.IDetalles_PedidosAplicacion.Buscar(entidad);
        }

        [HttpPost]
        public Detalles_Pedidos Insertar(Detalles_Pedidos entidad)
        {
            return this.IDetalles_PedidosAplicacion.Insertar(entidad);
        }

        [HttpPost]
        public Detalles_Pedidos Actualizar(Detalles_Pedidos entidad)
        {
            return this.IDetalles_PedidosAplicacion.Actualizar(entidad);
        }

        [HttpPost]
        public Detalles_Pedidos Borrar(Detalles_Pedidos entidad)
        {
            return this.IDetalles_PedidosAplicacion.Borrar(entidad);
        }
    }
}
