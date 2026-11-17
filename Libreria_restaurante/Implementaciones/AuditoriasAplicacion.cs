using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Nucleo;


namespace Libreria_restaurante.Implementaciones
{
    public class AuditoriasAplicacion : IAuditoriasAplicacion
    {
        private IConexion IConexion;

        public List<Auditorias> Consultar()
        {
            IConexion = new Conexion();
            IConexion.StringConexion = MetodosGenerales.ObtenerStringConexion("string_conexion");
            return IConexion.Auditorias!.ToList();
        }



    }
}