using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class PedidosPruebas
    {
        private TestDbContext conexion;
        private Pedidos? entidad = null;

        public PedidosPruebas()
        {
            this.conexion = new TestDbContext();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var nuevoCliente = new Clientes();
            this.conexion.Clientes.Add(nuevoCliente);

            var nuevaMesa = new Mesas();
            this.conexion.Mesas.Add(nuevaMesa);

            var nuevoMesero = new Meseros();
            this.conexion.Meseros.Add(nuevoMesero);

            this.conexion.SaveChanges();

            var clienteExistente = this.conexion.Clientes!.FirstOrDefault();
            var mesaExistente = this.conexion.Mesas!.FirstOrDefault();
            var meseroExistente = this.conexion.Meseros!.FirstOrDefault();

            this.entidad = new Pedidos()
            {
                cliente = clienteExistente.id,
                mesa = mesaExistente.id,
                mesero = meseroExistente.id,
                fecha_hora = DateTime.Now,
                numero_personas = 4,
                estado = "En preparación"
            };
            this.conexion.Pedidos!.Add(this.entidad!);
            this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }

        public void Consultar() { if (this.conexion.Pedidos!.ToList().Count <= 0) throw new Exception("Lista vacia");
            this.conexion = new TestDbContext();
        }

        private void Actualizar()
        {
            this.entidad!.estado = "Entregado";
            var entry = this.conexion!.Entry<Pedidos>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
            this.conexion = new TestDbContext();
        }

        private void Borrar() { this.conexion.Pedidos!.Remove(this.entidad!); this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }
    }
}