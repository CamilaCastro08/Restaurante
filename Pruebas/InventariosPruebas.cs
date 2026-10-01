using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class InventariosPruebas
    {
        private IConexion conexion;
        private Inventarios? entidad = null;

        public InventariosPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var ingredienteExistente = this.conexion.Ingredientes!.First();

            this.entidad = new Inventarios()
            {
                ingrediente = ingredienteExistente.id,
                stock_actual = 25.5m,
                stock_minimo = 10m,
                fecha_actualizacion = DateTime.Now
            };
            this.conexion.Inventarios!.Add(this.entidad!);
            this.conexion.SaveChanges();
        }

        public void Consultar() { if (this.conexion.Inventarios!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.stock_actual = 8m;
            var entry = this.conexion!.Entry<Inventarios>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Inventarios!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}