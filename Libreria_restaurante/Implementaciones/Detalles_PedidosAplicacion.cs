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

        public Detalles_Pedidos Insertar(Detalles_Pedidos entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Detalles_Pedidos!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Detalles_Pedidos",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", pedido: " + entidad.pedido
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Detalles_Pedidos Actualizar(Detalles_Pedidos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Detalles_Pedidos!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Detalles_Pedidos>(entidad);
            entry.State = EntityState.Modified;

            var audit = new Auditorias
            {
                tabla = "Detalles_Pedidos",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", pedido: " + anterior.pedido,
                datosNuevos = "id: " + entidad.id + ", pedido: " + entidad.pedido
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Detalles_Pedidos> Consultar()
        {
            return this.IConexion.Detalles_Pedidos!
                .Include(x => x._pedido)
                .Include(x => x._producto).ToList();
        }

        public List<Detalles_Pedidos> Buscar(Detalles_Pedidos entidad)
        {
            return this.IConexion.Detalles_Pedidos!
                .Include(x => x._pedido)
                .Include(x => x._producto)
                .Where(x => x.pedido == entidad.pedido)
                .ToList();
        }

        public Detalles_Pedidos Borrar(Detalles_Pedidos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Detalles_Pedidos!.Remove(entidad);

            var audit = new Auditorias
            {
                tabla = "Detalles_Pedidos",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", pedido: " + entidad.pedido,
                datosNuevos = ""
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Detalles_Pedidos entidad)
        {
            return entidad.pedido == 0;
        }
    }
}
