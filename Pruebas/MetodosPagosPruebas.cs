using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class MetodosPagosPruebas
    {
        private IConexion conexion;
        private MetodosPagos? entidad = null;

        public MetodosPagosPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            this.entidad = new MetodosPagos() { tipo = "Efectivo" };
            this.conexion.MetodosPagos!.Add(this.entidad!);
            this.conexion.SaveChanges();
        }

        public void Consultar()
        {
            if (this.conexion.MetodosPagos!.ToList().Count <= 0) throw new Exception("Lista vacia");
        }

        private void Actualizar()
        {
            this.entidad!.tipo = "Tarjeta de Crédito";
            var entry = this.conexion!.Entry<MetodosPagos>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar()
        {
            this.conexion.MetodosPagos!.Remove(this.entidad!);
            this.conexion.SaveChanges();
        }
    }
}