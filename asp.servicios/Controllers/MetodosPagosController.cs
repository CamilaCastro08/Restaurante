using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp.servicios.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MetodosPagosController : ControllerBase
    {
        private IMetodosPagosAplicacion IMetodosPagosAplicacion;

        public MetodosPagosController(IMetodosPagosAplicacion IMetodosPagosAplicacion)
        {
            this.IMetodosPagosAplicacion = IMetodosPagosAplicacion;
        }

        [HttpPost]
        public List<MetodosPagos> Consultar()
        {
            return this.IMetodosPagosAplicacion.Consultar();
        }

        [HttpPost]
        public List<MetodosPagos> Buscar(MetodosPagos entidad)
        {
            return this.IMetodosPagosAplicacion.Buscar(entidad);
        }

        [HttpPost]
        public MetodosPagos Insertar(MetodosPagos entidad)
        {
            return this.IMetodosPagosAplicacion.Insertar(entidad);
        }

        [HttpPost]
        public MetodosPagos Actualizar(MetodosPagos entidad)
        {
            return this.IMetodosPagosAplicacion.Actualizar(entidad);
        }

        [HttpPost]
        public MetodosPagos Borrar(MetodosPagos entidad)
        {
            return this.IMetodosPagosAplicacion.Borrar(entidad);
        }
    }
}
