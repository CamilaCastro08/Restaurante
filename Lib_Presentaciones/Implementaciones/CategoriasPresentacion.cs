using Libreria_restaurante.Entidades;
using presentaciones_horarios.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Implementaciones
{
    public class CategoriasPresentacion
    {
        private IComunicaciones IComunicaciones;

        public CategoriasPresentacion(IComunicaciones IComunicaciones)
        {
            this.IComunicaciones = IComunicaciones;
        }

        public Categorias Actualizar(Categorias entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Categorias>(
                "Categorias/Actualizar",
                entidad);
            task.Wait();
            return task.Result;
        }

        public Categorias Borrar(Categorias entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var task = this.IComunicaciones.Ejecutar<Categorias>(
                "Categorias/Borrar",
                entidad);
            task.Wait();
            return task.Result;
        }

        public List<Categorias> Buscar(Categorias entidad)
        {
            var task = this.IComunicaciones.Ejecutar<List<Categorias>>(
                "Categorias/Buscar",
                entidad);
            task.Wait();
            return task.Result;
        }

        public List<Categorias> Consultar()
        {
            var task = this.IComunicaciones.Ejecutar<List<Categorias>>(
                "Categorias/Consultar",
                null);
            task.Wait();
            return task.Result;
        }

        public Categorias Insertar(Categorias entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var task = this.IComunicaciones.Ejecutar<Categorias>(
                "Categorias/Insertar",
                entidad);
            task.Wait();
            return task.Result;
        }

        private bool Validar(Categorias entidad)
        {
            return string.IsNullOrEmpty(entidad.nombre);
        }
    }
}