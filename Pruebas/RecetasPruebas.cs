using Libreria_restaurante.Entidades;
using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;

namespace pruebas
{
    [TestClass]
    public class RecetasPruebas
    {
        private TestDbContext conexion;
        private IRecetasAplicacion iAplicacion;
        private Recetas? entidad = null;

        public RecetasPruebas()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new RecetasAplicacion(this.conexion);
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
            return this.conexion.Auditorias!.Count(x => x.tabla == "Recetas" && x.accion == accion);
        }

        private void Reiniciar()
        {
            this.conexion = new TestDbContext();
            this.iAplicacion = new RecetasAplicacion(this.conexion);
        }

        private void Insertar()
        {
            var p0 = new Productos();
            this.conexion.Productos!.Add(p0);
            this.conexion.SaveChanges();

            int antes = ContarAuditorias("Insertar");
            this.entidad = this.iAplicacion.Insertar(new Recetas()
            {
                producto = p0.id,
                tiempo_preparacion = DateTime.Today.AddMinutes(45),
                instrucciones = "Hervir y servir",
                porciones = 4,
                dificultad = "Media"
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
            var lista = this.iAplicacion.Buscar(new Recetas() { instrucciones = this.entidad!.instrucciones });
            Reiniciar();
            if (lista.Count <= 0)
                throw new Exception("No encontro");
        }

        private void Actualizar()
        {
            int antes = ContarAuditorias("Actualizar");
            this.entidad!.instrucciones = "Freir y servir";
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
