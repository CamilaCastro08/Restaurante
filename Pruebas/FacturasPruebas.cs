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
        private IConexion conexion;
        private Facturas? entidad = null;

        public FacturasPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var pedidoExistente = this.conexion.Pedidos!.First();

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
        }

        public void Consultar() { if (this.conexion.Facturas!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.total = 142800m;
            var entry = this.conexion!.Entry<Facturas>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Facturas!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}