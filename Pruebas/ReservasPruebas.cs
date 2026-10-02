using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class ReservasPruebas
    {
        private TestDbContext conexion;
        private Reservas? entidad = null;

        public ReservasPruebas()
        {
            this.conexion = new TestDbContext();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var clienteExistente = this.conexion.Clientes!.FirstOrDefault();
            var mesaExistente = this.conexion.Mesas!.FirstOrDefault();

            this.entidad = new Reservas()
            {
                cliente = clienteExistente.id,
                mesa = mesaExistente.id,
                fecha_hora = DateTime.Now.AddDays(3),
                numero_personas = 4,
                estado = "Confirmada"
            };
            this.conexion.Reservas!.Add(this.entidad!);
            this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }

        public void Consultar() { if (this.conexion.Reservas!.ToList().Count <= 0) throw new Exception("Lista vacia");
            this.conexion = new TestDbContext();
        }

        private void Actualizar()
        {
            this.entidad!.estado = "Cancelada";
            var entry = this.conexion!.Entry<Reservas>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
            this.conexion = new TestDbContext();
        }

        private void Borrar() { this.conexion.Reservas!.Remove(this.entidad!); this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }
    }
}