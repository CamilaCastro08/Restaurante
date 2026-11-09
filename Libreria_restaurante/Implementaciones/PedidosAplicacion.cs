using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class PedidosAplicacion : IPedidosAplicacion
    {
        private IConexion IConexion;

        public PedidosAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Pedidos Actualizar(Pedidos entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<Pedidos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Pedidos Borrar(Pedidos entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Pedidos!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Pedidos> Buscar(Pedidos entidad)
        {
            return this.IConexion.Pedidos!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<Pedidos> Consultar()
        {
            return this.IConexion.Pedidos!.ToList();
        }

        public Pedidos Insertar(Pedidos entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Pedidos!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Pedidos entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}