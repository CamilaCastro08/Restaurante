using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class ProveedoresPruebas
    {
        private IConexion conexion;
        private Proveedores? entidad = null;

        public ProveedoresPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            this.entidad = new Proveedores()
            {
                nombre = "Distribuidora S.A.",
                contacto = "Juan Pérez",
                direccion = "Calle 123",
                telefono = "5551234"
            };
            this.conexion.Proveedores!.Add(this.entidad!);
            this.conexion.SaveChanges();
        }

        public void Consultar() { if (this.conexion.Proveedores!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.telefono = "5559876";
            var entry = this.conexion!.Entry<Proveedores>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Proveedores!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}