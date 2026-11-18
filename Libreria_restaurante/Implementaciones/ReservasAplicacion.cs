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

        public Reservas Insertar(Reservas entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (!Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Reservas!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Reservas",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", estado: " + entidad.estado
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Reservas Actualizar(Reservas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (!Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Reservas!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Reservas>(entidad);
            entry.State = EntityState.Modified;

            var audit = new Auditorias
            {
                tabla = "Reservas",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", estado: " + anterior.estado,
                datosNuevos = "id: " + entidad.id + ", estado: " + entidad.estado
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Reservas> Consultar()
        {
            return this.IConexion.Reservas!.ToList();
        }

        public List<Reservas> Buscar(Reservas entidad)
        {
            return this.IConexion.Reservas!
                .Where(x => x.estado!.Contains(entidad.estado!))
                .ToList();
        }

        public Reservas Borrar(Reservas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Reservas!.Remove(entidad);

            var audit = new Auditorias
            {
                tabla = "Reservas",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", estado: " + entidad.estado,
                datosNuevos = ""
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Reservas entidad)
        {
            if (string.IsNullOrEmpty(entidad.estado))
                return false;
            return true;
        }
    }
}
