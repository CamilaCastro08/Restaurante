using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class MeserosAplicacion : IMeserosAplicacion
    {
        private IConexion IConexion;

        public MeserosAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Meseros Insertar(Meseros entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Meseros!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new MeserosAuditorias
            {
                tabla = "Meseros",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", zona_asignada: " + entidad.zona_asignada
            };
            this.IConexion.MeserosAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Meseros Actualizar(Meseros entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Meseros!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Meseros>(entidad);
            entry.State = EntityState.Modified;

            var audit = new MeserosAuditorias
            {
                tabla = "Meseros",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", zona_asignada: " + anterior.zona_asignada,
                datosNuevos = "id: " + entidad.id + ", zona_asignada: " + entidad.zona_asignada
            };
            this.IConexion.MeserosAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Meseros> Consultar()
        {
            return this.IConexion.Meseros!
                .Include(x => x._empleado).ToList();
        }

        public List<Meseros> Buscar(Meseros entidad)
        {
            return this.IConexion.Meseros!
                .Include(x => x._empleado)
                .Where(x => x.zona_asignada!.Contains(entidad.zona_asignada!))
                .ToList();
        }

        public Meseros Borrar(Meseros entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Meseros!.Remove(entidad);

            var audit = new MeserosAuditorias
            {
                tabla = "Meseros",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", zona_asignada: " + entidad.zona_asignada,
                datosNuevos = ""
            };
            this.IConexion.MeserosAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Meseros entidad)
        {
            return string.IsNullOrEmpty(entidad.zona_asignada);
        }
    }
}
