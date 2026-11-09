using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class GerentesAplicacion : IGerentesAplicacion
    {
        private IConexion IConexion;

        public GerentesAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Gerentes Actualizar(Gerentes entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<Gerentes>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Gerentes Borrar(Gerentes entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Gerentes!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Gerentes> Buscar(Gerentes entidad)
        {
            return this.IConexion.Gerentes!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<Gerentes> Consultar()
        {
            return this.IConexion.Gerentes!.ToList();
        }

        public Gerentes Insertar(Gerentes entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Gerentes!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Gerentes entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}