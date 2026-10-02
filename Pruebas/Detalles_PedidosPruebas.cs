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
        private TestDbContext conexion;
        private Detalles_Pedidos? entidad = null;

        public Detalles_PedidosPruebas()
        {
            this.conexion = new TestDbContext();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }


        public void Insertar()
        {
            var nuevoPedido = new Pedidos();
            this.conexion.Pedidos.Add(nuevoPedido);

            var nuevoProducto = new Productos();
            this.conexion.Productos.Add(nuevoProducto);

            this.conexion.SaveChanges();

            var pedidoExistente = this.conexion.Pedidos!.FirstOrDefault();
            var productoExistente = this.conexion.Productos!.FirstOrDefault();

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
            this.conexion = new TestDbContext();
        }

        public void Consultar() { if (this.conexion.Detalles_Pedidos!.ToList().Count <= 0) throw new Exception("Lista vacia");
            this.conexion = new TestDbContext();
        }


        private void Actualizar()
        {
            this.entidad!.cantidad = 3;
            this.entidad.subtotal = 84000m;
            var entry = this.conexion!.Entry<Detalles_Pedidos>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
            this.conexion = new TestDbContext();
        }

        private void Borrar() { this.conexion.Detalles_Pedidos!.Remove(this.entidad!); this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }
    }
}