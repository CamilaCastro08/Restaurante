using Libreria_restaurante.Entidades;
using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;

namespace pruebas
{
    [TestClass]
    public class PedidosPruebas
    {
        private TestDbContext conexion;
        private IPedidosAplicacion iAplicacion;
        private Pedidos? entidad = null;

        public PedidosPruebas()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new PedidosAplicacion(this.conexion);
        }

        [TestMethod]
        public void Execute()
        {
            Insertar();
            Consultar();
            Buscar();
            Actualizar();
            Borrar();
        }

        private int ContarAuditorias(string accion)
        {
            return this.conexion.Auditorias!.Count(x => x.tabla == "Pedidos" && x.accion == accion);
        }

        private void Reiniciar()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new PedidosAplicacion(this.conexion);
        }

        private void Insertar()
        {
            var p0 = new Clientes();
            this.conexion.Clientes!.Add(p0);
            var p1 = new Mesas();
            this.conexion.Mesas!.Add(p1);
            var p2 = new Meseros();
            this.conexion.Meseros!.Add(p2);
            this.conexion.SaveChanges();

            int antes = ContarAuditorias("Insertar");
            this.entidad = this.iAplicacion.Insertar(new Pedidos()
            {
                cliente = p0.id,
                mesa = p1.id,
                mesero = p2.id,
                fecha_hora = DateTime.Now,
                numero_personas = 3,
                estado = "Pendiente"
            });
            Reiniciar();
            if (this.entidad.id == 0)
                throw new Exception("No se guardo");
            if (ContarAuditorias("Insertar") != antes + 1)
                throw new Exception("No se guardo la auditoria");
        }

        private void Consultar()
        {
            var lista = this.iAplicacion.Consultar();
            Reiniciar();
            if (lista.Count <= 0)
                throw new Exception("Lista vacia");
        }

        private void Buscar()
        {
            var lista = this.iAplicacion.Buscar(new Pedidos() { estado = this.entidad!.estado });
            Reiniciar();
            if (lista.Count <= 0)
                throw new Exception("No encontro");
        }

        private void Actualizar()
        {
            int antes = ContarAuditorias("Actualizar");
            this.entidad!.estado = "Entregado";
            this.iAplicacion.Actualizar(this.entidad);
            Reiniciar();
            if (ContarAuditorias("Actualizar") != antes + 1)
                throw new Exception("No se guardo la auditoria");
        }

        private void Borrar()
        {
            int antes = ContarAuditorias("Borrar");
            this.iAplicacion.Borrar(this.entidad!);
            Reiniciar();
            if (ContarAuditorias("Borrar") != antes + 1)
                throw new Exception("No se guardo la auditoria");
        }
    }
}
