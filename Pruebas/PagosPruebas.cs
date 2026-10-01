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
        private IConexion conexion;
        private Pagos? entidad = null;

        public PagosPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var facturaExistente = this.conexion.Facturas!.First();
            var metodoExistente = this.conexion.MetodosPagos!.First();

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
        }

        public void Consultar() { if (this.conexion.Pagos!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.estado = "Anulado";
            var entry = this.conexion!.Entry<Pagos>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Pagos!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}