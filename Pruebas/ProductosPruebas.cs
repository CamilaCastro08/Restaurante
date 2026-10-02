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
        private TestDbContext conexion;
        private Productos? entidad = null;

        public ProductosPruebas()
        {
            this.conexion = new TestDbContext();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var nuevaCategoria = new Categorias();
            this.conexion.Categorias.Add(nuevaCategoria);

            var nuevoMenu = new Menus();
            this.conexion.Menus.Add(nuevoMenu);

            this.conexion.SaveChanges(); 


            var categoriaExistente = this.conexion.Categorias!.FirstOrDefault();
            var menuExistente = this.conexion.Menus!.FirstOrDefault();

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
            this.conexion = new TestDbContext();
        }

        public void Consultar() { if (this.conexion.Productos!.ToList().Count <= 0) throw new Exception("Lista vacia");
            this.conexion = new TestDbContext();
        }

        private void Actualizar()
        {
            this.entidad!.nombre = "Bandeja paisa especial";
            var entry = this.conexion!.Entry<Productos>(this.entidad);
            this.conexion.Update(this.entidad);
            this.conexion!.SaveChanges();
            this.conexion = new TestDbContext();
        }

        private void Borrar() { this.conexion.Productos!.Remove(this.entidad!); this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }
    }
}