using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp.servicios.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MenusController : ControllerBase
    {
        private IMenusAplicacion IMenusAplicacion;

        public MenusController(IMenusAplicacion IMenusAplicacion)
        {
            this.IMenusAplicacion = IMenusAplicacion;
        }

        [HttpPost]
        public List<Menus> Consultar()
        {
            return this.IMenusAplicacion.Consultar();
        }

        [HttpPost]
        public List<Menus> Buscar(Menus entidad)
        {
            return this.IMenusAplicacion.Buscar(entidad);
        }

        [HttpPost]
        public Menus Insertar(Menus entidad)
        {
            return this.IMenusAplicacion.Insertar(entidad);
        }

        [HttpPost]
        public Menus Actualizar(Menus entidad)
        {
            return this.IMenusAplicacion.Actualizar(entidad);
        }

        [HttpPost]
        public Menus Borrar(Menus entidad)
        {
            return this.IMenusAplicacion.Borrar(entidad);
        }
    }
}
