using Lib_Presentaciones.Interfaces;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class ClientesPresentacion : IClientesPresentacion
    {
        private IComunicaciones IComunicaciones;

        public ClientesPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Clientes Actualizar(Clientes entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Clientes>(
                "Clientes/Actualizar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public Clientes Borrar(Clientes entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Clientes>(
                "Clientes/Borrar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Clientes> Buscar(Clientes entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Clientes>>(
                "Clientes/Buscar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        public List<Clientes> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Clientes>>(
                "Clientes/Consultar",
                null);
            return task.GetAwaiter().GetResult();
        }

        public Clientes Insertar(Clientes entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Clientes>(
                "Clientes/Insertar",
                entidad);
            return task.GetAwaiter().GetResult();
        }

        private bool Validar(Clientes entidad)
        {
            return string.IsNullOrEmpty(entidad.nombre);
        }
    }
}
