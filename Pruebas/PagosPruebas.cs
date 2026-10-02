using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class PagosPruebas
    {
        private TestDbContext conexion;
        private Pagos? entidad = null;

        public PagosPruebas()
        {
            this.conexion = new TestDbContext();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar();
            this.conexion = new TestDbContext();
        }

        public void Insertar()
        {
            var nuevaFactura = new Facturas();
            this.conexion.Facturas.Add(nuevaFactura);

            var nuevoMetodo = new MetodosPagos();
            this.conexion.MetodosPagos.Add(nuevoMetodo);

            this.conexion.SaveChanges();

            var facturaExistente = this.conexion.Facturas!.FirstOrDefault();
            var metodoExistente = this.conexion.MetodosPagos!.FirstOrDefault();

            this.entidad = new Pagos()
            {
                factura = facturaExistente.id,
                metodoPago = metodoExistente.id,
                monto = 119000m,
                fecha = DateTime.Now,
                referencia = "REF-20260929-001",
                estado = "Aprobado"
            };
            this.conexion.Pagos!.Add(this.entidad!);
            this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }

        public void Consultar() { if (this.conexion.Pagos!.ToList().Count <= 0) throw new Exception("Lista vacia");
            this.conexion = new TestDbContext();
        }

        private void Actualizar()
        {
            this.entidad!.estado = "Anulado";
            var entry = this.conexion!.Entry<Pagos>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
            this.conexion = new TestDbContext();
        }

        private void Borrar() { this.conexion.Pagos!.Remove(this.entidad!); this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }
    }
}