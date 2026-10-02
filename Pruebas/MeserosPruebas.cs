using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class MeserosPruebas
    {
        private TestDbContext conexion;
        private Meseros? entidad = null;

        public MeserosPruebas()
        {
            this.conexion = new TestDbContext();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar();
            this.conexion = new TestDbContext();
        }

        public void Insertar()
        {
            var nuevoEmpleado = new Empleados();
            this.conexion.Empleados.Add(nuevoEmpleado);
            this.conexion.SaveChanges();

            var empleadoExistente = this.conexion.Empleados!.FirstOrDefault();

            this.entidad = new Meseros()
            {
                empleado = empleadoExistente.id,
                zona_asignada = "Terraza",
                num_mesas_asignadas = 5,
                calificacion_promedio = 4.5m
            };
            this.conexion.Meseros!.Add(this.entidad!);
            this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }

        public void Consultar() { if (this.conexion.Meseros!.ToList().Count <= 0) throw new Exception("Lista vacia");
            this.conexion = new TestDbContext();
        }

        private void Actualizar()
        {
            this.entidad!.num_mesas_asignadas = 6;
            var entry = this.conexion!.Entry<Meseros>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
            this.conexion = new TestDbContext();
        }

        private void Borrar() { this.conexion.Meseros!.Remove(this.entidad!); this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }
    }
}