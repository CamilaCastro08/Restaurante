using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp.servicios.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GerentesController : ControllerBase
    {
        private IGerentesAplicacion IGerentesAplicacion;

        public GerentesController(IGerentesAplicacion IGerentesAplicacion)
        {
            this.IGerentesAplicacion = IGerentesAplicacion;
        }

        [HttpPost]
        public List<Gerentes> Consultar()
        {
            return this.IGerentesAplicacion.Consultar();
        }

        [HttpPost]
        public List<Gerentes> Buscar(Gerentes entidad)
        {
            return this.IGerentesAplicacion.Buscar(entidad);
        }

        [HttpPost]
        public Gerentes Insertar(Gerentes entidad)
        {
            return this.IGerentesAplicacion.Insertar(entidad);
        }

        [HttpPost]
        public Gerentes Actualizar(Gerentes entidad)
        {
            return this.IGerentesAplicacion.Actualizar(entidad);
        }

        [HttpPost]
        public Gerentes Borrar(Gerentes entidad)
        {
            return this.IGerentesAplicacion.Borrar(entidad);
        }
    }
}
