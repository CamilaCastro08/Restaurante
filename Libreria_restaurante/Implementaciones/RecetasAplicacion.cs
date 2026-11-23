using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class RecetasAplicacion : IRecetasAplicacion
    {
        private IConexion IConexion;

        public RecetasAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Recetas Insertar(Recetas entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Recetas!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Recetas",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", instrucciones: " + entidad.instrucciones
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Recetas Actualizar(Recetas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Recetas!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Recetas>(entidad);
            entry.State = EntityState.Modified;

            var audit = new Auditorias
            {
                tabla = "Recetas",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", instrucciones: " + anterior.instrucciones,
                datosNuevos = "id: " + entidad.id + ", instrucciones: " + entidad.instrucciones
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Recetas> Consultar()
        {
            return this.IConexion.Recetas!
                .Include(x => x._producto).ToList();
        }

        public List<Recetas> Buscar(Recetas entidad)
        {
            return this.IConexion.Recetas!
                .Include(x => x._producto)
                .Where(x => x.instrucciones!.Contains(entidad.instrucciones!))
                .ToList();
        }

        public Recetas Borrar(Recetas entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var itemDb = this.IConexion.Recetas!.FirstOrDefault(x => x.id == entidad.id);
            if (itemDb == null)
                throw new Exception("El registro no existe o ya fue eliminado");

            try
            {
                this.IConexion.Recetas!.Remove(itemDb);

                var audit = new Auditorias
                {
                tabla = "Recetas",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", instrucciones: " + entidad.instrucciones,
                datosNuevos = ""
            };
                this.IConexion.Auditorias!.Add(audit);
                this.IConexion.SaveChanges();
                return itemDb;
            }
            catch (DbUpdateException)
            {
                throw new Exception("No se puede eliminar este registro porque tiene información asociada en otros módulos (primero elimine los registros relacionados).");
            }
        }

        private bool Validar(Recetas entidad)
        {
            return string.IsNullOrEmpty(entidad.instrucciones);
        }
    }
}

