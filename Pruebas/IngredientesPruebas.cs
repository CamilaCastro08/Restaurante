using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class IngredientesPruebas
    {
        private TestDbContext conexion;
        private Ingredientes? entidad = null;

        public IngredientesPruebas()
        {
            this.conexion = new TestDbContext();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar();
            this.conexion = new TestDbContext();
        }

        public void Insertar()
        {
            var nuevoProveedor = new Proveedores();
            this.conexion.Proveedores.Add(nuevoProveedor);

            var nuevaReceta = new Recetas();
            this.conexion.Recetas.Add(nuevaReceta);

            this.conexion.SaveChanges();

            var proveedorExistente = this.conexion.Proveedores!.FirstOrDefault();
            var recetaExistente = this.conexion.Recetas!.FirstOrDefault();

            this.entidad = new Ingredientes()
            {
                nombre = "Fríjol cargamanto",
                unidad_medida = "Kilogramo",
                costo_unitario = 8500m,
                proveedor = proveedorExistente.id,
                receta = recetaExistente.id
            };
            this.conexion.Ingredientes!.Add(this.entidad!);
            this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }

        public void Consultar() { if (this.conexion.Ingredientes!.ToList().Count <= 0) throw new Exception("Lista vacia");
            this.conexion = new TestDbContext();
        }

        private void Actualizar()
        {
            this.entidad!.costo_unitario = 9200m;
            var entry = this.conexion!.Entry<Ingredientes>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
            this.conexion = new TestDbContext();
        }

        private void Borrar() { this.conexion.Ingredientes!.Remove(this.entidad!); this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }
    }
}