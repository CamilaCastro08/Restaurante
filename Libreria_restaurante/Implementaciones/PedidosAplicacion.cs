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

        public Pedidos Insertar(Pedidos entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Pedidos!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Pedidos",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", estado: " + entidad.estado
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Pedidos Actualizar(Pedidos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Pedidos!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Pedidos>(entidad);
            entry.State = EntityState.Modified;

            var audit = new Auditorias
            {
                tabla = "Pedidos",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", estado: " + anterior.estado,
                datosNuevos = "id: " + entidad.id + ", estado: " + entidad.estado
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Pedidos> Consultar()
        {
            return this.IConexion.Pedidos!
                .Include(x => x._cliente)
                .Include(x => x._mesa)
                .Include(x => x._mesero).ToList();
        }

        public List<Pedidos> Buscar(Pedidos entidad)
        {
            return this.IConexion.Pedidos!
                .Include(x => x._cliente)
                .Include(x => x._mesa)
                .Include(x => x._mesero)
                .Where(x => x.estado!.Contains(entidad.estado!))
                .ToList();
        }

        public Pedidos Borrar(Pedidos entidad)
        {
            if (entidad == null || entidad.id == 0)
                throw new Exception("No se ha guardado");

            var existente = this.IConexion.Pedidos!.FirstOrDefault(x => x.id == entidad.id);
            if (existente == null)
                throw new Exception("El pedido no existe");

            bool tieneDetalles = this.IConexion.Detalles_Pedidos!.Any(x => x.pedido == entidad.id);
            if (tieneDetalles)
                throw new Exception("No se puede borrar el pedido porque tiene productos asociados en Detalles de Pedidos.");

            bool tieneFacturas = this.IConexion.Facturas!.Any(x => x.pedido == entidad.id);
            if (tieneFacturas)
                throw new Exception("No se puede borrar el pedido porque ya tiene una factura generada.");

            var datosAnt = "id: " + existente.id + ", estado: " + existente.estado;

            this.IConexion.Pedidos!.Remove(existente);

            var audit = new Auditorias
            {
                tabla = "Pedidos",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = datosAnt,
                datosNuevos = ""
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return existente;
        }
        private bool Validar(Pedidos entidad)
        {
            return string.IsNullOrEmpty(entidad.estado);
        }
    }
}
