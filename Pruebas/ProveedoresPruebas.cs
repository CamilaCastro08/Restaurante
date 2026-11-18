using Libreria_restaurante.Entidades;
using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;

namespace pruebas
{
    [TestClass]
    public class ProveedoresPruebas
    {
        private TestDbContext conexion;
        private IProveedoresAplicacion iAplicacion;
        private Proveedores? entidad = null;

        public ProveedoresPruebas()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new ProveedoresAplicacion(this.conexion);
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
            return this.conexion.Auditorias!.Count(x => x.tabla == "Proveedores" && x.accion == accion);
        }

        private void Reiniciar()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new ProveedoresAplicacion(this.conexion);
        }

        private void Insertar()
        {
            int antes = ContarAuditorias("Insertar");
            this.entidad = this.iAplicacion.Insertar(new Proveedores()
            {
                nombre = "Frutas del Valle",
                contacto = "Luis Rojas",
                direccion = "Zona industrial",
                telefono = "3205558899"
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
            var lista = this.iAplicacion.Buscar(new Proveedores() { nombre = this.entidad!.nombre });
            Reiniciar();
            if (lista.Count <= 0)
                throw new Exception("No encontro");
        }

        private void Actualizar()
        {
            int antes = ContarAuditorias("Actualizar");
            this.entidad!.nombre = "Frutas del Valle SAS";
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
