using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class TurnosAplicacion : ITurnosAplicacion
    {
        private IConexion IConexion;

        public TurnosAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Turnos Actualizar(Turnos entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<Turnos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Turnos Borrar(Turnos entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Turnos!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Turnos> Buscar(Turnos entidad)
        {
            return this.IConexion.Turnos!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<Turnos> Consultar()
        {
            return this.IConexion.Turnos!.ToList();
        }

        public Turnos Insertar(Turnos entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Turnos!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Turnos entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}