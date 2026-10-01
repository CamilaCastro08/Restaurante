using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class MenusPruebas
    {
        private IConexion conexion;
        private Menus? entidad = null;

        public MenusPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            this.entidad = new Menus()
            {
                nombre = "Menú Ejecutivo",
                vigencia_desde = DateTime.Now,
                vigencia_hasta = DateTime.Now.AddMonths(1),
                tipo = "Almuerzo"
            };
            this.conexion.Menus!.Add(this.entidad!);
            this.conexion.SaveChanges();
        }

        public void Consultar() { if (this.conexion.Menus!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.tipo = "Cena";
            var entry = this.conexion!.Entry<Menus>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Menus!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}