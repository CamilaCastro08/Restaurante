using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class Detalles_PedidosPruebas
    {
        private IConexion conexion;
        private Detalles_Pedidos? entidad = null;

        public Detalles_PedidosPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var pedidoExistente = this.conexion.Pedidos!.First();
            var productoExistente = this.conexion.Productos!.First();

            this.entidad = new Detalles_Pedidos()
            {
                pedido = pedidoExistente.id,
                producto = productoExistente.id,
                cantidad = 2,
                subtotal = 56000m,
                comentarios = "Sin cebolla, por favor"
            };
            this.conexion.Detalles_Pedidos!.Add(this.entidad!);
            this.conexion.SaveChanges();
        }

        public void Consultar() { if (this.conexion.Detalles_Pedidos!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.cantidad = 3;
            this.entidad.subtotal = 84000m;
            var entry = this.conexion!.Entry<Detalles_Pedidos>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Detalles_Pedidos!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}