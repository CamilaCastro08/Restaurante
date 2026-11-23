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

        public Gerentes Insertar(Gerentes entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Gerentes!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new GerentesAuditorias
            {
                tabla = "Gerentes",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", area_responsable: " + entidad.area_responsable
            };
            this.IConexion.GerentesAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Gerentes Actualizar(Gerentes entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Gerentes!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Gerentes>(entidad);
            entry.State = EntityState.Modified;

            var audit = new GerentesAuditorias
            {
                tabla = "Gerentes",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", area_responsable: " + anterior.area_responsable,
                datosNuevos = "id: " + entidad.id + ", area_responsable: " + entidad.area_responsable
            };
            this.IConexion.GerentesAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Gerentes> Consultar()
        {
            return this.IConexion.Gerentes!
                .Include(x => x._empleado).ToList();
        }

        public List<Gerentes> Buscar(Gerentes entidad)
        {
            return this.IConexion.Gerentes!
                .Include(x => x._empleado)
                .Where(x => x.area_responsable!.Contains(entidad.area_responsable!))
                .ToList();
        }

        public Gerentes Borrar(Gerentes entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Gerentes!.Remove(entidad);

            var audit = new GerentesAuditorias
            {
                tabla = "Gerentes",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", area_responsable: " + entidad.area_responsable,
                datosNuevos = ""
            };
            this.IConexion.GerentesAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Gerentes entidad)
        {
            return string.IsNullOrEmpty(entidad.area_responsable);
        }
    }
}
