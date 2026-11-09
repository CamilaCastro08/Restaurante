using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class EmpleadosAplicacion : IEmpleadosAplicacionAplicacion
    {
        private IConexion IConexion;

        public EmpleadosAplicacionAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public EmpleadosAplicacion Actualizar(EmpleadosAplicacion entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<EmpleadosAplicacion>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public EmpleadosAplicacion Borrar(EmpleadosAplicacion entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.EmpleadosAplicacion!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<EmpleadosAplicacion> Buscar(EmpleadosAplicacion entidad)
        {
            return this.IConexion.EmpleadosAplicacion!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<EmpleadosAplicacion> Consultar()
        {
            return this.IConexion.EmpleadosAplicacion!.ToList();
        }

        public EmpleadosAplicacion Insertar(EmpleadosAplicacion entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.EmpleadosAplicacion!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(EmpleadosAplicacion entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}