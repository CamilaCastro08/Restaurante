using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class GerentesPresentacion : IGerentesPresentacion
    {
        private IComunicaciones IComunicaciones;

        public GerentesPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Gerentes Actualizar(Gerentes entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Gerentes>(
                "Gerentes/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Gerentes Borrar(Gerentes entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Gerentes>(
                "Gerentes/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Gerentes> Buscar(Gerentes entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Gerentes>>(
                "Gerentes/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Gerentes> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Gerentes>>(
                "Gerentes/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Gerentes Insertar(Gerentes entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Gerentes>(
                "Gerentes/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Gerentes entidad)
        {
            return string.IsNullOrEmpty(entidad.area_responsable);
        }
    }
}
