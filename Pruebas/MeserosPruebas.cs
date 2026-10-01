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
        private IConexion conexion;
        private Meseros? entidad = null;

        public MeserosPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            var empleadoExistente = this.conexion.Empleados!.First();

            this.entidad = new Meseros()
            {
                empleado = empleadoExistente.id,
                zona_asignada = "Terraza",
                num_mesas_asignadas = 5,
                calificacion_promedio = 4.5m
            };
            this.conexion.Meseros!.Add(this.entidad!);
            this.conexion.SaveChanges();
        }

        public void Consultar() { if (this.conexion.Meseros!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.num_mesas_asignadas = 6;
            var entry = this.conexion!.Entry<Meseros>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Meseros!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}