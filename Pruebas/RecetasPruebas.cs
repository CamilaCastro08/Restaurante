using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class RecetasPruebas
    {
        private TestDbContext conexion;
        private Recetas? entidad = null;

        public RecetasPruebas()
        {
            this.conexion = new TestDbContext();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var productoExistente = this.conexion.Productos!.FirstOrDefault();

            this.entidad = new Recetas()
            {
                producto = productoExistente.id,
                tiempo_preparacion = DateTime.Today.AddMinutes(45),
                instrucciones = "Remojar los fríjoles la noche anterior y cocinarlos a fuego lento.",
                porciones = 2,
                dificultad = "Media"
            };
            this.conexion.Recetas!.Add(this.entidad!);
            this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }

        public void Consultar() { if (this.conexion.Recetas!.ToList().Count <= 0) throw new Exception("Lista vacia");
            this.conexion = new TestDbContext();
        }

        private void Actualizar()
        {
            this.entidad!.porciones = 4;
            var entry = this.conexion!.Entry<Recetas>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
            this.conexion = new TestDbContext();
        }

        private void Borrar() { this.conexion.Recetas!.Remove(this.entidad!); this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }
    }
}