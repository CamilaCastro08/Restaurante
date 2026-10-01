using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class MesasPruebas
    {
        private IConexion conexion;
        private Mesas? entidad = null;

        public MesasPruebas()
        {
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            this.entidad = new Mesas()
            {
                numero = 5,
                capacidad = 4,
                estado = "Disponible",
                ubicacion = "Terraza"
            };
            this.conexion.Mesas!.Add(this.entidad!);
            this.conexion.SaveChanges();
        }

        public void Consultar() { if (this.conexion.Mesas!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.estado = "Ocupada";
            var entry = this.conexion!.Entry<Mesas>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
        }

        private void Borrar() { this.conexion.Mesas!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}