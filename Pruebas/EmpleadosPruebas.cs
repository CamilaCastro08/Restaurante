using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class EmpleadosPruebas
    {
        private IConexion conexion;
        private Empleados? entidad = null;

        public EmpleadosPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            this.entidad = new Empleados()
            {
                nombre = "Ana Lopez",
                telefono = "3216549870",
                direccion = "Calle 80",
                salario = 1500.00m,
                fecha_contratacion = DateTime.Now,
                cargo = "Mesero"
            };
            this.conexion.Empleados!.Add(this.entidad!);
            this.conexion.SaveChanges();
        }

        public void Consultar() { if (this.conexion.Empleados!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.salario = 1700.00m;
            var entry = this.conexion!.Entry<Empleados>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Empleados!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}