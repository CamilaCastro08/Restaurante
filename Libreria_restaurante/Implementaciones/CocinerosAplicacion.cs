using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class CocinerosAplicacion : ICocinerosAplicacion
    {
        private IConexion IConexion;

        public CocinerosAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Cocineros Insertar(Cocineros entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Cocineros!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Cocineros",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", especialidad: " + entidad.especialidad
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Cocineros Actualizar(Cocineros entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Cocineros!.FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            var entry = this.IConexion.Entry<Cocineros>(entidad);
            entry.State = EntityState.Modified;

            var audit = new Auditorias
            {
                tabla = "Cocineros",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", especialidad: " + anterior.especialidad,
                datosNuevos = "id: " + entidad.id + ", especialidad: " + entidad.especialidad
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Cocineros> Consultar()
        {
            return this.IConexion.Cocineros!
                .Include(x => x._empleado).ToList();
        }

        public List<Cocineros> Buscar(Cocineros entidad)
        {
            return this.IConexion.Cocineros!
                .Include(x => x._empleado)
                .Where(x => x.especialidad!.Contains(entidad.especialidad!))
                .ToList();
        }

        public Cocineros Borrar(Cocineros entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var itemDb = this.IConexion.Cocineros!.FirstOrDefault(x => x.id == entidad.id);
            if (itemDb == null)
                throw new Exception("El registro no existe o ya fue eliminado");

            try
            {
                this.IConexion.Cocineros!.Remove(itemDb);

                var audit = new Auditorias
                {
                tabla = "Cocineros",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", especialidad: " + entidad.especialidad,
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

        private bool Validar(Cocineros entidad)
        {
            return string.IsNullOrEmpty(entidad.especialidad);
        }
    }
}

