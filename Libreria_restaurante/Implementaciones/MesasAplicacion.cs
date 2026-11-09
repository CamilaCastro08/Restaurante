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

        public Mesas Actualizar(Mesas entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<Mesas>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Mesas Borrar(Mesas entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Mesas!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Mesas> Buscar(Mesas entidad)
        {
            return this.IConexion.Mesas!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<Mesas> Consultar()
        {
            return this.IConexion.Mesas!.ToList();
        }

        public Mesas Insertar(Mesas entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Mesas!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Mesas entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}