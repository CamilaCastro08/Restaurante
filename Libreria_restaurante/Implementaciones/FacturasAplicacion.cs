using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Libreria_restaurante.Implementaciones
{
    public class FacturasAplicacion : IFacturasAplicacion
    {
        private IConexion IConexion;

        public FacturasAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Facturas Insertar(Facturas entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (entidad.pedido == 0)
                throw new Exception("Debe seleccionar un pedido");

            var pedido = this.IConexion.Pedidos!.FirstOrDefault(p => p.id == entidad.pedido);
            if (pedido == null)
                throw new Exception("El pedido no existe");

            // Si ya existe factura para este pedido, devolverla en lugar de bloquear el cobro
            var facturaExistente = this.IConexion.Facturas!.FirstOrDefault(f => f.pedido == entidad.pedido);
            if (facturaExistente != null)
                return facturaExistente;

            if (string.IsNullOrEmpty(entidad.numero))
                entidad.numero = $"FAC-{DateTime.Now:yyyyMMddHHmmss}";

            if (entidad.fecha_emision == default)
                entidad.fecha_emision = DateTime.Now;

            if (entidad.subtotal <= 0)
            {
                var detalles = this.IConexion.Detalles_Pedidos!
                    .Where(d => d.pedido == entidad.pedido)
                    .ToList();

                if (!detalles.Any())
                    throw new Exception("El pedido no tiene productos agregados para facturar.");

                entidad.subtotal = detalles.Sum(d => d.subtotal);
            }

            if (entidad.total <= 0)
            {
                // Impoconsumo 8% (coherente con Pedidos e Impresión)
                decimal impuesto = Math.Round(entidad.subtotal * 0.08m, 2);
                entidad.total = entidad.subtotal + impuesto;
            }

            entidad._pedido = null;
            entidad.Pagos = new();

            this.IConexion.Facturas!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Facturas",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = $"id: {entidad.id}, numero: {entidad.numero}, total: {entidad.total}"
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Facturas Actualizar(Facturas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Facturas!.AsNoTracking().FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            entidad._pedido = null;
            entidad.Pagos = new();

            var entry = this.IConexion.Entry<Facturas>(entidad);
            entry.State = EntityState.Modified;

            var audit = new Auditorias
            {
                tabla = "Facturas",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", numero: " + anterior.numero,
                datosNuevos = "id: " + entidad.id + ", numero: " + entidad.numero
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Facturas> Consultar()
        {
            return this.IConexion.Facturas!
                .Include(x => x._pedido).ToList();
        }

        public List<Facturas> Buscar(Facturas entidad)
        {
            return this.IConexion.Facturas!
                .Include(x => x._pedido)
                .Where(x => x.numero!.Contains(entidad.numero!))
                .ToList();
        }

        public Facturas Borrar(Facturas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var itemDb = this.IConexion.Facturas!.FirstOrDefault(x => x.id == entidad.id);
            if (itemDb == null)
                throw new Exception("La factura no existe");

            if (this.IConexion.Pagos!.Any(p => p.factura == itemDb.id))
                throw new Exception("No se puede eliminar la factura porque ya registra pagos asociados.");

            this.IConexion.Facturas!.Remove(itemDb);

            var audit = new Auditorias
            {
                tabla = "Facturas",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + itemDb.id + ", numero: " + itemDb.numero,
                datosNuevos = ""
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return itemDb;
        }

        private bool Validar(Facturas entidad)
        {
            return string.IsNullOrEmpty(entidad.numero);
        }
    }
}