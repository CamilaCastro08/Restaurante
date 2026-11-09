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

        public Ingredientes Actualizar(Ingredientes entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<Ingredientes>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Ingredientes Borrar(Ingredientes entidad)
        {
            {
                if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Ingredientes!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Ingredientes> Buscar(Ingredientes entidad)
        {
            return this.IConexion.Ingredientes!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<Ingredientes> Consultar()
        {
            return this.IConexion.Ingredientes!.ToList();
        }

        public Ingredientes Insertar(Ingredientes entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Ingredientes!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Ingredientes entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}