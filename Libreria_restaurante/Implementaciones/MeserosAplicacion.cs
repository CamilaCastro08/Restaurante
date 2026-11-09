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

        public Meseros Actualizar(Meseros entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<Meseros>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Meseros Borrar(Meseros entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Meseros!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Meseros> Buscar(Meseros entidad)
        {
            return this.IConexion.Meseros!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<Meseros> Consultar()
        {
            return this.IConexion.Meseros!.ToList();
        }

        public Meseros Insertar(Meseros entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Meseros!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Meseros entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}