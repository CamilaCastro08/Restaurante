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
        private TestDbContext conexion;
        private Inventarios? entidad = null;

        public InventariosPruebas()
        {
            this.conexion = new TestDbContext();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar();
        }

        public void Insertar()
        {

            var nuevoIngrediente = new Ingredientes();
            this.conexion.Ingredientes.Add(nuevoIngrediente);
            this.conexion.SaveChanges();

            var ingredienteExistente = this.conexion.Ingredientes!.FirstOrDefault();

            this.entidad = new Inventarios()

            {
                ingrediente = ingredienteExistente.id,
                stock_actual = 25.5m,
                stock_minimo = 10m,
                fecha_actualizacion = DateTime.Now
            };
            this.conexion.Inventarios!.Add(this.entidad!);
            this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }

        public void Consultar() { if (this.conexion.Inventarios!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.stock_actual = 8m;
            this.conexion = new TestDbContext();
            var entry = this.conexion!.Entry<Inventarios>(this.entidad);
            this.conexion.Update(this.entidad);
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Inventarios!.Remove(this.entidad!); this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }
    }
}