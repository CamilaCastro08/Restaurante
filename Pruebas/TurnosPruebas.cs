using Libreria_restaurante.Entidades;
using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;

namespace pruebas
{
    [TestClass]
    public class TurnosPruebas
    {
        private TestDbContext conexion;
        private ITurnosAplicacion iAplicacion;
        private Turnos? entidad = null;

        public TurnosPruebas()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new TurnosAplicacion(this.conexion);
        }

        [TestMethod]
        public void Execute()
        {
            Insertar();
            Consultar();
            Buscar();
            Actualizar();
            Borrar();
        }

        private int ContarAuditorias(string accion)
        {
            return this.conexion.Auditorias!.Count(x => x.tabla == "Turnos" && x.accion == accion);
        }

        private void Reiniciar()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new TurnosAplicacion(this.conexion);
        }

        private void Insertar()
        {
            var p0 = new Empleados();
            this.conexion.Empleados!.Add(p0);
            this.conexion.SaveChanges();

            int antes = ContarAuditorias("Insertar");
            this.entidad = this.iAplicacion.Insertar(new Turnos()
            {
                empleado = p0.id,
                fecha = DateTime.Today,
                hora_entrada = DateTime.Today.AddHours(8),
                hora_salida = DateTime.Today.AddHours(16)
            });
            Reiniciar();
            if (this.entidad.id == 0)
                throw new Exception("No se guardo");
            if (ContarAuditorias("Insertar") != antes + 1)
                throw new Exception("No se guardo la auditoria");
        }

        private void Consultar()
        {
            var lista = this.iAplicacion.Consultar();
            Reiniciar();
            if (lista.Count <= 0)
                throw new Exception("Lista vacia");
        }

        private void Buscar()
        {
            var lista = this.iAplicacion.Buscar(new Turnos() { empleado = this.entidad!.empleado });
            Reiniciar();
            if (lista.Count <= 0)
                throw new Exception("No encontro");
        }

        private void Actualizar()
        {
            int antes = ContarAuditorias("Actualizar");
            this.entidad!.hora_salida = DateTime.Today.AddHours(18);
            this.iAplicacion.Actualizar(this.entidad);
            Reiniciar();
            if (ContarAuditorias("Actualizar") != antes + 1)
                throw new Exception("No se guardo la auditoria");
        }

        private void Borrar()
        {
            int antes = ContarAuditorias("Borrar");
            this.iAplicacion.Borrar(this.entidad!);
            Reiniciar();
            if (ContarAuditorias("Borrar") != antes + 1)
                throw new Exception("No se guardo la auditoria");
        }
    }
}
