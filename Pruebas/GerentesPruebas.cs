using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class GerentesPruebas
    {
        private TestDbContext conexion;
        private Gerentes? entidad = null;

        public GerentesPruebas()
        {
            this.conexion = new TestDbContext();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var empleadoExistente = this.conexion.Empleados!.FirstOrDefault();

            this.entidad = new Gerentes()
            {
                empleado = empleadoExistente.id,
                nivel_autorizacion = "Alto",
                bono_desempeno = 500000m,
                area_responsable = "Operaciones"
            };
            this.conexion.Gerentes!.Add(this.entidad!);
            this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }

        public void Consultar() { if (this.conexion.Gerentes!.ToList().Count <= 0) throw new Exception("Lista vacia");
            this.conexion = new TestDbContext();
        }

        private void Actualizar()
        {
            this.entidad!.bono_desempeno = 650000m;
            var entry = this.conexion!.Entry<Gerentes>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
            this.conexion = new TestDbContext();
        }

        private void Borrar() { this.conexion.Gerentes!.Remove(this.entidad!); this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }
    }
}