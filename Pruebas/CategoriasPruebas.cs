using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class CategoriasPruebas
    {

        private TestDbContext conexion;
        private IConexion conexion;
        private Categorias? entidad = null;

        public CategoriasPruebas()
        {
            this.conexion = new TestDbContext();
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();

        }

        [TestMethod]
        public void Execute()
        {
            Insertar();
            Consultar();
            Actualizar();
            Borrar();
        }

        public void Insertar()
        {
            this.entidad = new Categorias()
            {
                nombre = "Bebidas"
            };
            this.conexion.Categorias!.Add(this.entidad!);
            this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }

        public void Consultar()
        {
            var lista = this.conexion.Categorias!.ToList();
            if (lista.Count <= 0)
                throw new Exception("Lista vacia");
            this.conexion = new TestDbContext();
        }

        private void Actualizar()
        {
            this.entidad!.nombre = "Bebidas Actualizadas";
            var entry = this.conexion!.Entry<Categorias>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
            this.conexion = new TestDbContext();
        }

        private void Borrar()
        {
            this.conexion.Categorias!.Remove(this.entidad!);
            this.conexion.SaveChanges();
            this.conexion = new TestDbContext();

        }
    }
}