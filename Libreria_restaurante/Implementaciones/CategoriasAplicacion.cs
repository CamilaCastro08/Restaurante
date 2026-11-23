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

        public Categorias Insertar(Categorias entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Categorias!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Categorias",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", nombre: " + entidad.nombre
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Categorias Actualizar(Categorias entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Categorias!.AsNoTracking().FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Categorias>(entidad);
            entry.State = EntityState.Modified;

            var audit = new Auditorias
            {
                tabla = "Categorias",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", nombre: " + anterior.nombre,
                datosNuevos = "id: " + entidad.id + ", nombre: " + entidad.nombre
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Categorias> Consultar()
        {
            return this.IConexion.Categorias!.ToList();
        }

        public List<Categorias> Buscar(Categorias entidad)
        {
            return this.IConexion.Categorias!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public Categorias Borrar(Categorias entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var itemDb = this.IConexion.Categorias!.FirstOrDefault(x => x.id == entidad.id);
            if (itemDb == null)
                throw new Exception("La categoría no existe");

            if (this.IConexion.Productos!.Any(p => p.categoria == itemDb.id))
                throw new Exception("No se puede eliminar la categoría porque tiene productos asociados.");

            this.IConexion.Categorias!.Remove(itemDb);

            var audit = new Auditorias
            {
                tabla = "Categorias",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + itemDb.id + ", nombre: " + itemDb.nombre,
                datosNuevos = ""
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return itemDb;
        }

        private bool Validar(Categorias entidad)
        {
            return string.IsNullOrEmpty(entidad.nombre);
        }
    }
}
