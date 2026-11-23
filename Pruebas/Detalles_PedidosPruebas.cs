using Libreria_restaurante.Entidades;
using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;

namespace pruebas
{
    [TestClass]
    public class Detalles_PedidosPruebas
    {
        private TestDbContext conexion;
        private IDetalles_PedidosAplicacion iAplicacion;
        private Detalles_Pedidos? entidad = null;

        public Detalles_PedidosPruebas()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new Detalles_PedidosAplicacion(this.conexion);
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
            return this.conexion.Auditorias!.Count(x => x.tabla == "Detalles_Pedidos" && x.accion == accion);
        }

        private void Reiniciar()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new Detalles_PedidosAplicacion(this.conexion);
        }

        private void Insertar()
        {
            var p0 = new Pedidos();
            this.conexion.Pedidos!.Add(p0);
            var p1 = new Productos { nombre = "Plato Especial", precio = 28000m };
            this.conexion.Productos!.Add(p1);
            this.conexion.SaveChanges();

            int antes = ContarAuditorias("Insertar");
            this.entidad = this.iAplicacion.Insertar(new Detalles_Pedidos()
            {
                pedido = p0.id,
                producto = p1.id,
                cantidad = 2,
                subtotal = 56000m,
                comentarios = "Sin cebolla"
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
            var lista = this.iAplicacion.Buscar(new Detalles_Pedidos() { pedido = this.entidad!.pedido });
            Reiniciar();
            if (lista.Count <= 0)
                throw new Exception("No encontro");
        }

        private void Actualizar()
        {
            int antes = ContarAuditorias("Actualizar");
            this.entidad!.comentarios = "Sin sal";
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
