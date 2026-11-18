using Libreria_restaurante.Entidades;
using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;

namespace pruebas
{
    [TestClass]
    public class MeserosPruebas
    {
        private TestDbContext conexion;
        private IMeserosAplicacion iAplicacion;
        private Meseros? entidad = null;

        public MeserosPruebas()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new MeserosAplicacion(this.conexion);
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
            return this.conexion.Auditorias!.Count(x => x.tabla == "Meseros" && x.accion == accion);
        }

        private void Reiniciar()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new MeserosAplicacion(this.conexion);
        }

        private void Insertar()
        {
            var p0 = new Empleados();
            this.conexion.Empleados!.Add(p0);
            this.conexion.SaveChanges();

            int antes = ContarAuditorias("Insertar");
            this.entidad = this.iAplicacion.Insertar(new Meseros()
            {
                empleado = p0.id,
                zona_asignada = "Zona norte",
                num_mesas_asignadas = 6,
                calificacion_promedio = 4.5m
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
            var lista = this.iAplicacion.Buscar(new Meseros() { zona_asignada = this.entidad!.zona_asignada });
            Reiniciar();
            if (lista.Count <= 0)
                throw new Exception("No encontro");
        }

        private void Actualizar()
        {
            int antes = ContarAuditorias("Actualizar");
            this.entidad!.zona_asignada = "Zona sur";
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
