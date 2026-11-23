using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class InventariosAplicacion : IInventariosAplicacion
    {
        private IConexion IConexion;

        public InventariosAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Inventarios Insertar(Inventarios entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Inventarios!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new InventariosAuditorias
            {
                tabla = "Inventarios",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", ingrediente: " + entidad.ingrediente
            };
            this.IConexion.InventariosAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Inventarios Actualizar(Inventarios entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Inventarios!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Inventarios>(entidad);
            entry.State = EntityState.Modified;

            var audit = new InventariosAuditorias
            {
                tabla = "Inventarios",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", ingrediente: " + anterior.ingrediente,
                datosNuevos = "id: " + entidad.id + ", ingrediente: " + entidad.ingrediente
            };
            this.IConexion.InventariosAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Inventarios> Consultar()
        {
            return this.IConexion.Inventarios!
                .Include(x => x._ingrediente).ToList();
        }

        public List<Inventarios> Buscar(Inventarios entidad)
        {
            return this.IConexion.Inventarios!
                .Include(x => x._ingrediente)
                .Where(x => x.ingrediente == entidad.ingrediente)
                .ToList();
        }

        public Inventarios Borrar(Inventarios entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Inventarios!.Remove(entidad);

            var audit = new InventariosAuditorias
            {
                tabla = "Inventarios",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", ingrediente: " + entidad.ingrediente,
                datosNuevos = ""
            };
            this.IConexion.InventariosAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Inventarios entidad)
        {
            return entidad.ingrediente == 0;
        }
    }
}
