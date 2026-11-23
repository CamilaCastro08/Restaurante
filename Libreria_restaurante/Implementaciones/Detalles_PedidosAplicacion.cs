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

       
            var productoDb = this.IConexion.Productos!.FirstOrDefault(p => p.id == entidad.producto);
            if (productoDb == null)
                throw new Exception("El producto seleccionado no existe.");

            if (productoDb.precio <= 0)
                throw new Exception($"El producto '{productoDb.nombre}' no tiene un precio configurado.");

            entidad.subtotal = Math.Round(productoDb.precio * entidad.cantidad, 2);

            
            var recetasIds = this.IConexion.Recetas!
                .Where(r => r.producto == entidad.producto)
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
                    if (inventario.stock_actual < entidad.cantidad)
                        throw new Exception($"Stock insuficiente para el ingrediente: {ing.nombre}");

                    inventario.stock_actual -= entidad.cantidad;
                    inventario.fecha_actualizacion = DateTime.Now;
                    this.IConexion.Entry(inventario).State = EntityState.Modified;
                }
            }

            this.IConexion.Detalles_Pedidos!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Detalles_Pedidos",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = $"id: {entidad.id}, pedido: {entidad.pedido}, producto: {entidad.producto}, cantidad: {entidad.cantidad}, subtotal: {entidad.subtotal}"
            };
            this.IConexion.Auditorias!.Add(audit);
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

            var detalleDb = this.IConexion.Detalles_Pedidos!.FirstOrDefault(d => d.id == entidad.id);
            if (detalleDb == null)
                throw new Exception("No existe");

            var recetasIds = this.IConexion.Recetas!
                .Where(r => r.producto == detalleDb.producto)
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
                    inventario.stock_actual += detalleDb.cantidad;
                    inventario.fecha_actualizacion = DateTime.Now;
                    this.IConexion.Entry(inventario).State = EntityState.Modified;
                }
            }

            this.IConexion.Detalles_Pedidos!.Remove(detalleDb);

            var audit = new Auditorias
            {
                tabla = "Detalles_Pedidos",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + detalleDb.id + ", pedido: " + detalleDb.pedido,
                datosNuevos = ""
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return detalleDb;
        }
    }
}