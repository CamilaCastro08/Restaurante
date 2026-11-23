using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class EmpleadosAplicacion : IEmpleadosAplicacion
    {
        private IConexion IConexion;

        public EmpleadosAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Empleados Insertar(Empleados entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Empleados!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new EmpleadosAuditorias
            {
                tabla = "Empleados",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", nombre: " + entidad.nombre
            };
            this.IConexion.EmpleadosAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Empleados Actualizar(Empleados entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Empleados!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Empleados>(entidad);
            entry.State = EntityState.Modified;

            var audit = new EmpleadosAuditorias
            {
                tabla = "Empleados",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", nombre: " + anterior.nombre,
                datosNuevos = "id: " + entidad.id + ", nombre: " + entidad.nombre
            };
            this.IConexion.EmpleadosAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Empleados> Consultar()
        {
            return this.IConexion.Empleados!.ToList();
        }

        public List<Empleados> Buscar(Empleados entidad)
        {
            return this.IConexion.Empleados!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public Empleados Borrar(Empleados entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Empleados!.Remove(entidad);

            var audit = new EmpleadosAuditorias
            {
                tabla = "Empleados",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", nombre: " + entidad.nombre,
                datosNuevos = ""
            };
            this.IConexion.EmpleadosAuditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Empleados entidad)
        {
            return string.IsNullOrEmpty(entidad.nombre);
        }
    }
}
