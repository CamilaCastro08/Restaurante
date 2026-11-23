using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class PermisosAplicacion : IPermisosAplicacion
    {
        private IConexion IConexion;

        public PermisosAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Permisos Insertar(Permisos entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Permisos!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Permisos",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", nombre: " + entidad.nombre + ", rol: " + entidad.rol
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Permisos Actualizar(Permisos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Permisos!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Permisos>(entidad);
            entry.State = EntityState.Modified;

            var audit = new Auditorias
            {
                tabla = "Permisos",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", nombre: " + anterior.nombre + ", rol: " + anterior.rol,
                datosNuevos = "id: " + entidad.id + ", nombre: " + entidad.nombre + ", rol: " + entidad.rol
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Permisos> Consultar()
        {
            return this.IConexion.Permisos!
                .Include(x => x._rol).ToList();
        }

        public List<Permisos> Buscar(Permisos entidad)
        {
            return this.IConexion.Permisos!
                .Include(x => x._rol)
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public Permisos Borrar(Permisos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Permisos!.Remove(entidad);

            var audit = new Auditorias
            {
                tabla = "Permisos",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", nombre: " + entidad.nombre + ", rol: " + entidad.rol,
                datosNuevos = ""
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Permisos entidad)
        {
            return string.IsNullOrEmpty(entidad.nombre);
        }
    }
}
