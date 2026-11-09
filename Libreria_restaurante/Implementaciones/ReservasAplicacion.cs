using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class ReservasAplicacion : IReservasAplicacion
    {
        private IConexion IConexion;

        public ReservasAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Reservas Actualizar(Reservas entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<Reservas>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Reservas Borrar(Reservas entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Reservas!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Reservas> Buscar(Reservas entidad)
        {
            return this.IConexion.Reservas!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<Reservas> Consultar()
        {
            return this.IConexion.Reservas!.ToList();
        }

        public Reservas Insertar(Reservas entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Reservas!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Reservas entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}