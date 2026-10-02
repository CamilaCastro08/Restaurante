using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class FacturasPruebas
    {
        private TestDbContext conexion;
        private Facturas? entidad = null;

        public FacturasPruebas()
        {
            this.conexion = new TestDbContext();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar();
            this.conexion = new TestDbContext();
        }

        public void Insertar()
        {
            var pedidoExistente = this.conexion.Pedidos!.FirstOrDefault();

            this.entidad = new Facturas()
            {
                pedido = pedidoExistente.id,
                numero = "FAC-2026-0001",
                fecha_emision = DateTime.Now,
                subtotal = 100000m,
                total = 119000m
            };
            this.conexion.Facturas!.Add(this.entidad!);
            this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }

        public void Consultar() { if (this.conexion.Facturas!.ToList().Count <= 0) throw new Exception("Lista vacia");
            this.conexion = new TestDbContext();
        }

        private void Actualizar()
        {
            this.entidad!.total = 142800m;
            var entry = this.conexion!.Entry<Facturas>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
            this.conexion = new TestDbContext();
        }

        private void Borrar() { this.conexion.Facturas!.Remove(this.entidad!); this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }
    }
}