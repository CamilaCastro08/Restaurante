using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asp.servicios.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuditoriasController : ControllerBase
    {
        private IAuditoriasAplicacion IAuditoriasAplicacion;

        public AuditoriasController(IAuditoriasAplicacion IAuditoriasAplicacion)
        {
            this.IAuditoriasAplicacion = IAuditoriasAplicacion;
        }

        [HttpPost]
        public List<Auditorias> Buscar(Auditorias entidad)
        {
            return this.IAuditoriasAplicacion.Buscar(entidad);
        }

        
        [HttpPost]
        public Auditorias Actualizar(Auditorias entidad)
        {
            return this.IAuditoriasAplicacion.Actualizar(entidad);
        }
    }
}
