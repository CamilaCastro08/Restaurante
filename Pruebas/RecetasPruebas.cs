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
        private IConexion conexion;
        private Recetas? entidad = null;

        public RecetasPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var productoExistente = this.conexion.Productos!.First();

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
        }

        public void Consultar() { if (this.conexion.Recetas!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.porciones = 4;
            var entry = this.conexion!.Entry<Recetas>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Recetas!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}