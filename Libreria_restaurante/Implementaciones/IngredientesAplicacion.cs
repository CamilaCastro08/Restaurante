using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class IngredientesAplicacion : IIngredientesAplicacion
    {
        private IConexion IConexion;

        public IngredientesAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Ingredientes Insertar(Ingredientes entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Ingredientes!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Ingredientes",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", nombre: " + entidad.nombre
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Ingredientes Actualizar(Ingredientes entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Ingredientes!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Ingredientes>(entidad);
            entry.State = EntityState.Modified;

            var audit = new Auditorias
            {
                tabla = "Ingredientes",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", nombre: " + anterior.nombre,
                datosNuevos = "id: " + entidad.id + ", nombre: " + entidad.nombre
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Ingredientes> Consultar()
        {
            return this.IConexion.Ingredientes!
                .Include(x => x._receta)
                .Include(x => x._proveedor).ToList();
        }

        public List<Ingredientes> Buscar(Ingredientes entidad)
        {
            return this.IConexion.Ingredientes!
                .Include(x => x._receta)
                .Include(x => x._proveedor)
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public Ingredientes Borrar(Ingredientes entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Ingredientes!.Remove(entidad);

            var audit = new Auditorias
            {
                tabla = "Ingredientes",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", nombre: " + entidad.nombre,
                datosNuevos = ""
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Ingredientes entidad)
        {
            return string.IsNullOrEmpty(entidad.nombre);
        }
    }
}
