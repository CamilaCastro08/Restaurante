using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class TurnosPruebas
    {
        private IConexion conexion;
        private Turnos? entidad = null;

        public TurnosPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var empleadoExistente = this.conexion.Empleados!.First();

            this.entidad = new Turnos()
            {
                empleado = empleadoExistente.id,
                fecha = DateTime.Today,
                hora_entrada = DateTime.Today.AddHours(8),
                hora_salida = DateTime.Today.AddHours(16)
            };
            this.conexion.Turnos!.Add(this.entidad!);
            this.conexion.SaveChanges();
        }

        public void Consultar() { if (this.conexion.Turnos!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.hora_salida = DateTime.Today.AddHours(17);
            var entry = this.conexion!.Entry<Turnos>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Turnos!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}