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

        public MetodosPagos Insertar(MetodosPagos entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.MetodosPagos!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "MetodosPagos",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", tipo: " + entidad.tipo
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public MetodosPagos Actualizar(MetodosPagos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.MetodosPagos!.AsNoTracking().FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<MetodosPagos>(entidad);
            entry.State = EntityState.Modified;

            var audit = new Auditorias
            {
                tabla = "MetodosPagos",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", tipo: " + anterior.tipo,
                datosNuevos = "id: " + entidad.id + ", tipo: " + entidad.tipo
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<MetodosPagos> Consultar()
        {
            return this.IConexion.MetodosPagos!.ToList();
        }

        public List<MetodosPagos> Buscar(MetodosPagos entidad)
        {
            return this.IConexion.MetodosPagos!
                .Where(x => x.tipo!.Contains(entidad.tipo!))
                .ToList();
        }

        public MetodosPagos Borrar(MetodosPagos entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var itemDb = this.IConexion.MetodosPagos!.FirstOrDefault(x => x.id == entidad.id);
            if (itemDb == null)
                throw new Exception("El método de pago no existe");

            if (this.IConexion.Pagos!.Any(p => p.metodoPago == itemDb.id))
                throw new Exception("No se puede eliminar el método de pago porque tiene pagos asociados.");

            var pagos = this.IConexion.Pagos!.Where(p => p.metodoPago == itemDb.id).ToList();
                if (pagos.Any()) this.IConexion.Pagos!.RemoveRange(pagos);
                this.IConexion.MetodosPagos!.Remove(itemDb);

            var audit = new Auditorias
            {
                tabla = "MetodosPagos",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + itemDb.id + ", tipo: " + itemDb.tipo,
                datosNuevos = ""
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return itemDb;
        }

        private bool Validar(MetodosPagos entidad)
        {
            return string.IsNullOrEmpty(entidad.tipo);
        }
    }
}

