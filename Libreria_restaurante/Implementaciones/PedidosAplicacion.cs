using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;

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

            entidad.estado = "En preparación";

            if (entidad.fecha_hora == default)
                entidad.fecha_hora = DateTime.Now;

            if (entidad.numero_personas <= 0)
                entidad.numero_personas = 1;

            if (entidad.mesa == 0)
                throw new Exception("Debe seleccionar una mesa");

            if (entidad.mesero == 0)
                throw new Exception("Debe seleccionar un mesero");

            if (entidad.cliente == 0)
            {
                var consumidorFinal = this.IConexion.Clientes!
                    .FirstOrDefault(c => c.nombre == "Consumidor Final");

                if (consumidorFinal == null)
                {
                    consumidorFinal = new Clientes
                    {
                        nombre = "Consumidor Final",
                        cedula = "2222222222",
                        telefono = "N/A",
                        direccion = "Mostrador / Salón"
                    };
                    this.IConexion.Clientes!.Add(consumidorFinal);
                    this.IConexion.SaveChanges();
                }

                entidad.cliente = consumidorFinal.id;
            }

            var mesa = this.IConexion.Mesas!.FirstOrDefault(m => m.id == entidad.mesa);
            if (mesa == null)
                throw new Exception("La mesa seleccionada no existe");

            if (mesa.estado != null && mesa.estado.Equals("Ocupada", StringComparison.OrdinalIgnoreCase))
                throw new Exception("La mesa ya se encuentra ocupada");

            mesa.estado = "Ocupada";
            this.IConexion.Entry(mesa).State = EntityState.Modified;

            this.IConexion.Pedidos!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Pedidos",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = $"id: {entidad.id}, mesa: {mesa.numero}, estado: {entidad.estado}"
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Pedidos Actualizar(Pedidos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (entidad.mesa == 0)
                throw new Exception("Debe seleccionar una mesa");

            var anterior = this.IConexion.Pedidos!.AsNoTracking().FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            if (string.IsNullOrEmpty(entidad.estado))
                entidad.estado = anterior.estado;

            if (entidad.cliente == 0)
                entidad.cliente = anterior.cliente;

            var entry = this.IConexion.Entry<Pedidos>(entidad);
            entry.State = EntityState.Modified;

            var mesa = this.IConexion.Mesas!.FirstOrDefault(m => m.id == entidad.mesa);
            if (mesa != null && !string.IsNullOrEmpty(entidad.estado))
            {
                if (entidad.estado.Equals("Pagado", StringComparison.OrdinalIgnoreCase) ||
                    entidad.estado.Equals("Cancelado", StringComparison.OrdinalIgnoreCase) ||
                    entidad.estado.Equals("Finalizado", StringComparison.OrdinalIgnoreCase))
                {
                    mesa.estado = "Disponible";
                }
                else
                {
                    mesa.estado = "Ocupada";
                }
                this.IConexion.Entry(mesa).State = EntityState.Modified;
            }

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
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            // Buscar la entidad rastreada directamente desde la BD para evitar conflictos de claves/navegación
            var pedidoDb = this.IConexion.Pedidos!.FirstOrDefault(p => p.id == entidad.id);
            if (pedidoDb == null)
                throw new Exception("El pedido no existe");

            // 1. Si tiene factura asociada, no permitir borrar sin anular o eliminar factura
            if (this.IConexion.Facturas!.Any(f => f.pedido == pedidoDb.id))
                throw new Exception("No se puede borrar un pedido que ya tiene factura generada.");

            // 2. Eliminar detalles del pedido y devolver stock al inventario
            var detalles = this.IConexion.Detalles_Pedidos!.Where(d => d.pedido == pedidoDb.id).ToList();
            foreach (var det in detalles)
            {
                var recetasIds = this.IConexion.Recetas!
                    .Where(r => r.producto == det.producto)
                    .Select(r => r.id)
                    .ToList();

                var ingredientes = this.IConexion.Ingredientes!
                    .Where(ing => recetasIds.Contains(ing.receta))
                    .ToList();

                foreach (var ing in ingredientes)
                {
                    var inv = this.IConexion.Inventarios!.FirstOrDefault(i => i.ingrediente == ing.id);
                    if (inv != null)
                    {
                        inv.stock_actual += det.cantidad;
                        inv.fecha_actualizacion = DateTime.Now;
                        this.IConexion.Entry(inv).State = EntityState.Modified;
                    }
                }

                this.IConexion.Detalles_Pedidos!.Remove(det);
            }

            // 3. Liberar la mesa
            var mesa = this.IConexion.Mesas!.FirstOrDefault(m => m.id == pedidoDb.mesa);
            if (mesa != null)
            {
                mesa.estado = "Disponible";
                this.IConexion.Entry(mesa).State = EntityState.Modified;
            }

            string infoAnterior = $"id: {pedidoDb.id}, estado: {pedidoDb.estado}";
            this.IConexion.Pedidos!.Remove(pedidoDb);

            var audit = new Auditorias
            {
                tabla = "Pedidos",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = infoAnterior,
                datosNuevos = ""
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return pedidoDb;
        }
    }
}