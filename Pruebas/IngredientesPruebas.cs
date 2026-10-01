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
        private IConexion conexion;
        private Ingredientes? entidad = null;

        public IngredientesPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var proveedorExistente = this.conexion.Proveedores!.First();
            var recetaExistente = this.conexion.Recetas!.First();

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
        }

        public void Consultar() { if (this.conexion.Ingredientes!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.costo_unitario = 9200m;
            var entry = this.conexion!.Entry<Ingredientes>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Ingredientes!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}