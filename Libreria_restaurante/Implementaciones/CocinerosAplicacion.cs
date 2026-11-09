using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class CocinerosAplicacion : ICocinerosAplicacion
    {
        private IConexion IConexion;

        public CocinerosAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Cocineros Actualizar(Cocineros entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<Cocineros>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Cocineros Borrar(Cocineros entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Cocineros!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Cocineros> Buscar(Cocineros entidad)
        {
            return this.IConexion.Cocineros!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<Cocineros> Consultar()
        {
            return this.IConexion.Cocineros!.ToList();
        }

        public Cocineros Insertar(Cocineros entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Cocineros!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Cocineros entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}