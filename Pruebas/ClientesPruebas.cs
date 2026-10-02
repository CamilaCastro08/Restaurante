using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Entidades;
using Libreria_restaurante.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace pruebas
{
    [TestClass]
    public class ClientesPruebas
    {
        private TestDbContext conexion;
        private IConexion conexion;
        private Clientes? entidad = null;

        public ClientesPruebas()
        {
            this.conexion = new TestDbContext();
            this.conexion = new Conexion();
            this.conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
        }

        [TestMethod]
        public void Execute() { Insertar(); Consultar(); Actualizar(); Borrar(); }

        public void Insertar()
        {
            this.entidad = new Clientes()
            {
                nombre = "Carlos Gomez",
                telefono = "3001112233",
                cedula = "12345678",
                direccion = "Avenida 45"
            };
            this.conexion.Clientes!.Add(this.entidad!);
            this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }

        public void Consultar() { if (this.conexion.Clientes!.ToList().Count <= 0) throw new Exception("Lista vacia");
        this.conexion = new TestDbContext();
        }
        }

        public void Consultar() { if (this.conexion.Clientes!.ToList().Count <= 0) throw new Exception("Lista vacia"); }

        private void Actualizar()
        {
            this.entidad!.telefono = "3119998877";
            var entry = this.conexion!.Entry<Clientes>(this.entidad);
            entry.State = EntityState.Modified;
            this.conexion!.SaveChanges();
            this.conexion = new TestDbContext();
        }

        private void Borrar() { this.conexion.Clientes!.Remove(this.entidad!); this.conexion.SaveChanges();
            this.conexion = new TestDbContext();
        }
        }

        private void Borrar() { this.conexion.Clientes!.Remove(this.entidad!); this.conexion.SaveChanges(); }
    }
}