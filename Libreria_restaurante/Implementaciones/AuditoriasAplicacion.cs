using Microsoft.EntityFrameworkCore;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;

namespace Libreria_restaurante.Implementaciones
{
    public class AuditoriasAplicacion : IAuditoriasAplicacion
    {
        private IConexion IConexion;

        public AuditoriasAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public List<Auditorias> Consultar()
        {
            return this.IConexion.Auditorias!
                .Include(x => x._Usuario).ToList();
        }

        public List<Auditorias> Buscar(Auditorias entidad)
        {
            return this.IConexion.Auditorias!
                .Include(x => x._Usuario)
                .Where(x => x.tabla!.Contains(entidad.tabla!))
                .ToList();
        }
    }
}
