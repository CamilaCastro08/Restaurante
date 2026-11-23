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

        public Turnos Insertar(Turnos entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Turnos!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new TurnosAuditorias
            {
                tabla = "Turnos",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", empleado: " + entidad.empleado
            };
            this.IConexion.TurnosAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Turnos Actualizar(Turnos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Turnos!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Turnos>(entidad);
            entry.State = EntityState.Modified;

            var audit = new TurnosAuditorias
            {
                tabla = "Turnos",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", empleado: " + anterior.empleado,
                datosNuevos = "id: " + entidad.id + ", empleado: " + entidad.empleado
            };
            this.IConexion.TurnosAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Turnos> Consultar()
        {
            return this.IConexion.Turnos!
                .Include(x => x._empleado).ToList();
        }

        public List<Turnos> Buscar(Turnos entidad)
        {
            return this.IConexion.Turnos!
                .Include(x => x._empleado)
                .Where(x => x.empleado == entidad.empleado)
                .ToList();
        }

        public Turnos Borrar(Turnos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Turnos!.Remove(entidad);

            var audit = new TurnosAuditorias
            {
                tabla = "Turnos",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", empleado: " + entidad.empleado,
                datosNuevos = ""
            };
            this.IConexion.TurnosAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Turnos entidad)
        {
            return entidad.empleado == 0;
        }
    }
}
