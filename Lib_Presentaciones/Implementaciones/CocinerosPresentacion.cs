using Libreria_restaurante.Entidades;
using Microsoft.IdentityModel.Tokens;
using presentaciones_horarios.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Implementaciones
{
    public class CocinerosPresentacion
    {
        private IComunicaciones IComunicaciones;

        public CocinerosPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Cocineros Actualizar(Cocineros entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Cocineros>(
                "Cocineros/Actualizar",
                entidad);
            task.Wait();
            return task.Result;
        }

        public Cocineros Borrar(Cocineros entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Cocineros>(
                "Cocineros/Borrar",
                entidad);
            task.Wait();
            return task.Result;
        }

        public List<Cocineros> Buscar(Cocineros entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Cocineros>>(
                "Cocineros/Buscar",
                entidad);
            task.Wait();
            return task.Result;
        }

        public List<Cocineros> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Cocineros>>(
                "Cocineros/Consultar",
                null);
            task.Wait();
            return task.Result;
        }

        public Cocineros Insertar(Cocineros entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Cocineros>(
                "Cocineros/Insertar",
                entidad);
            task.Wait();
            return task.Result;
        }

        private bool Validar(Cocineros entidad)
        {
            return string.IsNullOrEmpty(entidad.empleado) ||
                string.IsNullOrEmpty(entidad.especialidad) ||
                string.IsNullOrEmpty(entidad.anios_experiencia) ||
                string.IsNullOrEmpty(entidad.estacion_asignada);
        }
    }
}
