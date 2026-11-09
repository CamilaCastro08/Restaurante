using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp.servicios.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MeserosController : ControllerBase
    {
        private IMeserosAplicacion IMeserosAplicacion;

        public MeserosController(IMeserosAplicacion IMeserosAplicacion)
        {
            this.IMeserosAplicacion = IMeserosAplicacion;
        }

        [HttpPost]
        public List<Meseros> Consultar()
        {
            return this.IMeserosAplicacion.Consultar();
        }

        [HttpPost]
        public List<Meseros> Buscar(Meseros entidad)
        {
            return this.IMeserosAplicacion.Buscar(entidad);
        }

        [HttpPost]
        public Meseros Insertar(Meseros entidad)
        {
            return this.IMeserosAplicacion.Insertar(entidad);
        }

        [HttpPost]
        public Meseros Actualizar(Meseros entidad)
        {
            return this.IMeserosAplicacion.Actualizar(entidad);
        }

        [HttpPost]
        public Meseros Borrar(Meseros entidad)
        {
            return this.IMeserosAplicacion.Borrar(entidad);
        }
    }
}
