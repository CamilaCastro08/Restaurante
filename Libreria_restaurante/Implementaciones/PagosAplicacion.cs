using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Libreria_restaurante.Implementaciones
{
    public class PagosAplicacion : IPagosAplicacion
    {
        private IConexion IConexion;

        public PagosAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Pagos Insertar(Pagos entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (entidad.factura == 0)
                throw new Exception("Debe seleccionar una factura");

            if (entidad.metodoPago == 0)
                throw new Exception("Debe seleccionar un método de pago");

            if (entidad.monto <= 0)
                throw new Exception("El monto a pagar debe ser mayor a cero");

            if (string.IsNullOrEmpty(entidad.referencia))
                entidad.referencia = $"PAG-{DateTime.Now:yyyyMMddHHmmss}";

            if (string.IsNullOrEmpty(entidad.estado))
                entidad.estado = "Aprobado";

            if (entidad.fecha == default)
                entidad.fecha = DateTime.Now;

            entidad._factura = null;
            entidad._metodoPago = null;

            var factura = this.IConexion.Facturas!.FirstOrDefault(f => f.id == entidad.factura);
            if (factura == null)
                throw new Exception("La factura no existe");

            if (factura.total > 0)
            {
                var pagosPrevios = this.IConexion.Pagos!.Where(p => p.factura == factura.id).ToList();
                decimal acumulado = pagosPrevios.Sum(p => p.monto);
                if (acumulado >= factura.total)
                    throw new Exception("La factura ya ha sido pagada en su totalidad.");
            }

            this.IConexion.Pagos!.Add(entidad);
            this.IConexion.SaveChanges();

            // Sincronización de Pedido y Mesa: cuando la factura se paga por completo
            if (factura.pedido > 0)
            {
                var pedido = this.IConexion.Pedidos!.FirstOrDefault(p => p.id == factura.pedido);
                if (pedido != null)
                {
                    var todosPagos = this.IConexion.Pagos!.Where(p => p.factura == factura.id).ToList();
                    decimal totalPagado = todosPagos.Sum(p => p.monto);

                    if (factura.total <= 0 || totalPagado >= factura.total)
                    {
                        pedido.estado = "Pagado";
                        this.IConexion.Entry(pedido).State = EntityState.Modified;

                        var mesa = this.IConexion.Mesas!.FirstOrDefault(m => m.id == pedido.mesa);
                        if (mesa != null)
                        {
                            mesa.estado = "Disponible";
                            this.IConexion.Entry(mesa).State = EntityState.Modified;
                        }
                        this.IConexion.SaveChanges();
                    }
                }
            }

            var audit = new Auditorias
            {
                tabla = "Pagos",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", referencia: " + entidad.referencia
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Pagos Actualizar(Pagos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Pagos!.AsNoTracking().FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            entidad._factura = null;
            entidad._metodoPago = null;

            var entry = this.IConexion.Entry<Pagos>(entidad);
            entry.State = EntityState.Modified;

            var audit = new Auditorias
            {
                tabla = "Pagos",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", referencia: " + anterior.referencia,
                datosNuevos = "id: " + entidad.id + ", referencia: " + entidad.referencia
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Pagos> Consultar()
        {
            return this.IConexion.Pagos!
                .Include(x => x._factura)
                .Include(x => x._metodoPago).ToList();
        }

        public List<Pagos> Buscar(Pagos entidad)
        {
            return this.IConexion.Pagos!
                .Include(x => x._factura)
                .Include(x => x._metodoPago)
                .Where(x => x.referencia!.Contains(entidad.referencia!))
                .ToList();
        }

        public Pagos Borrar(Pagos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var itemDb = this.IConexion.Pagos!.FirstOrDefault(x => x.id == entidad.id);
            if (itemDb == null)
                throw new Exception("El pago no existe");

            this.IConexion.Pagos!.Remove(itemDb);

            var audit = new Auditorias
            {
                tabla = "Pagos",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + itemDb.id + ", referencia: " + itemDb.referencia,
                datosNuevos = ""
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return itemDb;
        }

        private bool Validar(Pagos entidad)
        {
            return string.IsNullOrEmpty(entidad.referencia);
        }
    }
}