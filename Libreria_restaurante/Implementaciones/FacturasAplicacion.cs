using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Facturas!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Facturas",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", numero: " + entidad.numero
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

            var anterior = this.IConexion.Facturas!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

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

            this.IConexion.Facturas!.Remove(entidad);

            var audit = new Auditorias
            {
                tabla = "Facturas",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", numero: " + entidad.numero,
                datosNuevos = ""
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Facturas entidad)
        {
            return string.IsNullOrEmpty(entidad.numero);
        }
    }
}
