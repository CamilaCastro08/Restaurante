using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp.servicios.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CocinerosController : ControllerBase
    {
        private ICocinerosAplicacion ICocinerosAplicacion;

        public CocinerosController(ICocinerosAplicacion ICocinerosAplicacion)
        {
            this.ICocinerosAplicacion = ICocinerosAplicacion;
        }

        [HttpPost]
        public List<Cocineros> Consultar()
        {
            return this.ICocinerosAplicacion.Consultar();
        }

        [HttpPost]
        public List<Cocineros> Buscar(Cocineros entidad)
        {
            return this.ICocinerosAplicacion.Buscar(entidad);
        }

        [HttpPost]
        public Cocineros Insertar(Cocineros entidad)
        {
            return this.ICocinerosAplicacion.Insertar(entidad);
        }

        [HttpPost]
        public Cocineros Actualizar(Cocineros entidad)
        {
            return this.ICocinerosAplicacion.Actualizar(entidad);
        }

        [HttpPost]
        public Cocineros Borrar(Cocineros entidad)
        {
            return this.ICocinerosAplicacion.Borrar(entidad);
        }
    }
}
