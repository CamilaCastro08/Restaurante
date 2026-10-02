using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class CocinerosPruebas
    {
        private TestDbContext conexion;
        private Cocineros? entidad = null;

        public CocinerosPruebas()
        {
            this.conexion = new TestDbContext();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var empleadoExistente = this.conexion.Empleados!.FirstOrDefault();

            this.entidad = new Cocineros()
            {
                empleado = empleadoExistente.id,
                especialidad = "Cocina colombiana",
                anios_experiencia = 6,
                estacion_asignada = "Parrilla"
            };
            this.conexion.Cocineros!.Add(this.entidad!);
            this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }

        public void Consultar() { if (this.conexion.Cocineros!.ToList().Count <= 0) throw new Exception("Lista vacia");
            this.conexion = new TestDbContext();
        }

        private void Actualizar()
        {
            this.entidad!.anios_experiencia = 7;
            var entry = this.conexion!.Entry<Cocineros>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
            this.conexion = new TestDbContext();
        }

        private void Borrar() { this.conexion.Cocineros!.Remove(this.entidad!); this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }
    }
}