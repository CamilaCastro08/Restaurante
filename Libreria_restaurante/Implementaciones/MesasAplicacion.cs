using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class MesasAplicacion : IMesasAplicacion
    {
        private IConexion IConexion;

        public MesasAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Mesas Insertar(Mesas entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (!Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Mesas!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Mesas",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", ubicacion: " + entidad.ubicacion
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Mesas Actualizar(Mesas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (!Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Mesas!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Mesas>(entidad);
            entry.State = EntityState.Modified;

            var audit = new Auditorias
            {
                tabla = "Mesas",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", ubicacion: " + anterior.ubicacion,
                datosNuevos = "id: " + entidad.id + ", ubicacion: " + entidad.ubicacion
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Mesas> Consultar()
        {
            return this.IConexion.Mesas!.ToList();
        }

        public List<Mesas> Buscar(Mesas entidad)
        {
            return this.IConexion.Mesas!
                .Where(x => x.ubicacion!.Contains(entidad.ubicacion!))
                .ToList();
        }

        public Mesas Borrar(Mesas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Mesas!.Remove(entidad);

            var audit = new Auditorias
            {
                tabla = "Mesas",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", ubicacion: " + entidad.ubicacion,
                datosNuevos = ""
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Mesas entidad)
        {
            if (string.IsNullOrEmpty(entidad.ubicacion))
                return false;
            return true;
        }
    }
}
