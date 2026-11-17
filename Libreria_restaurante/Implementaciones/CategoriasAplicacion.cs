using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class CategoriasAplicacion : ICategoriasAplicacion
    {
        private IConexion IConexion;

        public CategoriasAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Categorias Actualizar(Categorias entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<Categorias>(entidad);
            entry.State = EntityState.Modified;
            var audit = new Auditorias
            {
                tabla = "Categorias",
                accion = "Acutalizó",
                fecha = DateTime.Now,
                datosAnteriores = "Id: " + entidad.id,
                datosNuevos = "Se actualizó Id: {entidad.id} en Categorias"
            };
            IConexion.Auditorias!.Add(audit);
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Categorias Borrar(Categorias entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Categorias!.Remove(entidad!);
            var audit = new Auditorias
            {
                tabla = "Categorias",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "Id: " + entidad.id,
                datosNuevos = "Se borró registro Id: {entidad.id} en Categorias"
            };
            IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Categorias> Buscar(Categorias entidad)
        {
            return this.IConexion.Categorias!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<Categorias> Consultar()
        {
            return this.IConexion.Categorias!.ToList();
        }

        public Categorias Insertar(Categorias entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Categorias!.Add(entidad!);
            var audit = new Auditorias
            {
                tabla = "Categorias",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "Id: " + entidad.id,
                datosNuevos = "Se insertó registro Id: {entidad.id} en Categorias"
            };
            IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Categorias entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}