using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class ProductosPruebas
    {
        private IConexion conexion;
        private Productos? entidad = null;

        public ProductosPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var categoriaExistente = this.conexion.Categorias!.First();
            var menuExistente = this.conexion.Menus!.First();

            this.entidad = new Productos()
            {
                nombre = "Bandeja paisa",
                vigencia_desde = DateTime.Today,
                vigencia_hasta = DateTime.Today.AddMonths(6),
                tipo = "Plato fuerte",
                categoria = categoriaExistente.id,
                menu = menuExistente.id
            };
            this.conexion.Productos!.Add(this.entidad!);
            this.conexion.SaveChanges();
        }

        public void Consultar() { if (this.conexion.Productos!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.nombre = "Bandeja paisa especial";
            var entry = this.conexion!.Entry<Productos>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Productos!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}