using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp.servicios.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RecetasController : ControllerBase
    {
        private IRecetasAplicacion IRecetasAplicacion;

        public RecetasController(IRecetasAplicacion IRecetasAplicacion)
        {
            this.IRecetasAplicacion = IRecetasAplicacion;
        }

        [HttpPost]
        public List<Recetas> Consultar()
        {
            return this.IRecetasAplicacion.Consultar();
        }

        [HttpPost]
        public List<Recetas> Buscar(Recetas entidad)
        {
            return this.IRecetasAplicacion.Buscar(entidad);
        }

        [HttpPost]
        public Recetas Insertar(Recetas entidad)
        {
            return this.IRecetasAplicacion.Insertar(entidad);
        }

        [HttpPost]
        public Recetas Actualizar(Recetas entidad)
        {
            return this.IRecetasAplicacion.Actualizar(entidad);
        }

        [HttpPost]
        public Recetas Borrar(Recetas entidad)
        {
            return this.IRecetasAplicacion.Borrar(entidad);
        }
    }
}
