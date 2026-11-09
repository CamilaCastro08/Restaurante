using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class Detalles_PedidosAplicacion : IDetalles_PedidosAplicacion
    {
        private IConexion IConexion;

        public Detalles_PedidosAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Detalles_Pedidos Actualizar(Detalles_Pedidos entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<Detalles_Pedidos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Detalles_Pedidos Borrar(Detalles_Pedidos entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Detalles_Pedidos!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Detalles_Pedidos> Buscar(Detalles_Pedidos entidad)
        {
            return this.IConexion.Detalles_Pedidos!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<Detalles_Pedidos> Consultar()
        {
            return this.IConexion.Detalles_Pedidos!.ToList();
        }

        public Detalles_Pedidos Insertar(Detalles_Pedidos entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Detalles_Pedidos!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Detalles_Pedidos entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}