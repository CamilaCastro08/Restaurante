using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class FacturasAplicacion : IFacturasAplicacion
    {
        private IConexion IConexion;

        public FacturasAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Facturas Actualizar(Facturas entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<Facturas>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Facturas Borrar(Facturas entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Facturas!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Facturas> Buscar(Facturas entidad)
        {
            return this.IConexion.Facturas!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<Facturas> Consultar()
        {
            return this.IConexion.Facturas!.ToList();
        }

        public Facturas Insertar(Facturas entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Facturas!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Facturas entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}