using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class RecetasAplicacion : IRecetasAplicacion
    {
        private IConexion IConexion;

        public RecetasAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Recetas Actualizar(Recetas entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<Recetas>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Recetas Borrar(Recetas entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Recetas!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Recetas> Buscar(Recetas entidad)
        {
            return this.IConexion.Recetas!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<Recetas> Consultar()
        {
            return this.IConexion.Recetas!.ToList();
        }

        public Recetas Insertar(Recetas entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Recetas!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Recetas entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}