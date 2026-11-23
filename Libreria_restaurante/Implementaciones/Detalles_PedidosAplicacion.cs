using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;

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

            if (entidad.pedido == 0 || entidad.producto == 0 || entidad.cantidad <= 0)
                throw new Exception("Falta informacion");

            var pedidoDb = this.IConexion.Pedidos!.FirstOrDefault(p => p.id == entidad.pedido);
            if (pedidoDb == null)
                throw new Exception("El pedido no existe.");

            if (pedidoDb.estado == "Pagado" || pedidoDb.estado == "Cancelado")
                throw new Exception("No se pueden agregar productos a un pedido cerrado.");

            var productoDb = this.IConexion.Productos!.FirstOrDefault(p => p.id == entidad.producto);
            if (productoDb == null)
                throw new Exception("El producto seleccionado no existe.");

            if (productoDb.precio <= 0)
                throw new Exception($"El producto '{productoDb.nombre}' no tiene un precio configurado.");

            // Descontar inventario
            AjustarInventario(entidad.producto, -entidad.cantidad);

            // Si ya existe el mismo producto sin comentarios diferentes en el pedido, acumular
            var existente = this.IConexion.Detalles_Pedidos!.FirstOrDefault(d =>
                d.pedido == entidad.pedido &&
                d.producto == entidad.producto &&
                (d.comentarios ?? "") == (entidad.comentarios ?? ""));

            if (existente != null)
            {
                int cantAnterior = existente.cantidad;
                existente.cantidad += entidad.cantidad;
                existente.subtotal = Math.Round(productoDb.precio * existente.cantidad, 2);
                existente._pedido = null;
                existente._producto = null;

                this.IConexion.Entry(existente).State = EntityState.Modified;
                this.IConexion.SaveChanges();

                this.IConexion.Auditorias!.Add(new Auditorias
                {
                    tabla = "Detalles_Pedidos",
                    accion = "Actualizar",
                    fecha = DateTime.Now,
                    datosAnteriores = $"id: {existente.id}, cantidad: {cantAnterior}",
                    datosNuevos = $"id: {existente.id}, cantidad: {existente.cantidad}, subtotal: {existente.subtotal}"
                });
                this.IConexion.SaveChanges();
                return existente;
            }

            entidad.subtotal = Math.Round(productoDb.precio * entidad.cantidad, 2);
            entidad._pedido = null;
            entidad._producto = null;

            this.IConexion.Detalles_Pedidos!.Add(entidad);
            this.IConexion.SaveChanges();

            this.IConexion.Auditorias!.Add(new Auditorias
            {
                tabla = "Detalles_Pedidos",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = $"id: {entidad.id}, pedido: {entidad.pedido}, producto: {entidad.producto}, cantidad: {entidad.cantidad}, subtotal: {entidad.subtotal}"
            });
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Detalles_Pedidos Actualizar(Detalles_Pedidos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (entidad.pedido == 0 || entidad.producto == 0 || entidad.cantidad <= 0)
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Detalles_Pedidos!.AsNoTracking().FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var productoDb = this.IConexion.Productos!.FirstOrDefault(p => p.id == entidad.producto);
            if (productoDb == null)
                throw new Exception("El producto no existe.");

            int diferencia = entidad.cantidad - anterior.cantidad;
            if (diferencia != 0)
                AjustarInventario(entidad.producto, -diferencia);

            entidad.subtotal = Math.Round(productoDb.precio * entidad.cantidad, 2);
            entidad._pedido = null;
            entidad._producto = null;

            var entry = this.IConexion.Entry<Detalles_Pedidos>(entidad);
            entry.State = EntityState.Modified;

            this.IConexion.Auditorias!.Add(new Auditorias
            {
                tabla = "Detalles_Pedidos",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = $"id: {anterior.id}, cantidad: {anterior.cantidad}",
                datosNuevos = $"id: {entidad.id}, cantidad: {entidad.cantidad}"
            });
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

            var detalleDb = this.IConexion.Detalles_Pedidos!.FirstOrDefault(d => d.id == entidad.id);
            if (detalleDb == null)
                throw new Exception("No existe");

            var pedidoDb = this.IConexion.Pedidos!.FirstOrDefault(p => p.id == detalleDb.pedido);
            if (pedidoDb != null && (pedidoDb.estado == "Pagado" || pedidoDb.estado == "Cancelado"))
                throw new Exception("No se pueden quitar productos de un pedido ya cerrado.");

            AjustarInventario(detalleDb.producto, detalleDb.cantidad);

            this.IConexion.Detalles_Pedidos!.Remove(detalleDb);

            this.IConexion.Auditorias!.Add(new Auditorias
            {
                tabla = "Detalles_Pedidos",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + detalleDb.id + ", pedido: " + detalleDb.pedido,
                datosNuevos = ""
            });
            this.IConexion.SaveChanges();
            return detalleDb;
        }

        private void AjustarInventario(int productoId, int deltaCantidad)
        {
            var recetasIds = this.IConexion.Recetas!
                .Where(r => r.producto == productoId)
                .Select(r => r.id)
                .ToList();

            var ingredientes = this.IConexion.Ingredientes!
                .Where(ing => recetasIds.Contains(ing.receta))
                .ToList();

            foreach (var ing in ingredientes)
            {
                var inventario = this.IConexion.Inventarios!
                    .FirstOrDefault(inv => inv.ingrediente == ing.id);

                if (inventario != null)
                {
                    if (deltaCantidad < 0 && inventario.stock_actual < Math.Abs(deltaCantidad))
                        throw new Exception($"Stock insuficiente para el ingrediente: {ing.nombre}");

                    inventario.stock_actual += deltaCantidad;
                    inventario.fecha_actualizacion = DateTime.Now;
                    this.IConexion.Entry(inventario).State = EntityState.Modified;
                }
            }
        }
    }
}