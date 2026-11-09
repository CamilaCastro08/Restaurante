using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class MetodosPagosAplicacion : IMetodosPagosAplicacion
    {
        private IConexion IConexion;

        public MetodosPagosAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public MetodosPagos Actualizar(MetodosPagos entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<MetodosPagos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public MetodosPagos Borrar(MetodosPagos entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.MetodosPagos!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<MetodosPagos> Buscar(MetodosPagos entidad)
        {
            return this.IConexion.MetodosPagos!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<MetodosPagos> Consultar()
        {
            return this.IConexion.MetodosPagos!.ToList();
        }

        public MetodosPagos Insertar(MetodosPagos entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.MetodosPagos!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(MetodosPagos entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}