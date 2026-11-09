using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Implementaciones
{
    public class MenusAplicacion : IMenusAplicacion
    {
        private IConexion IConexion;

        public MenusAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Menus Actualizar(Menus entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var entry = this.IConexion!.Entry<Menus>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Menus Borrar(Menus entidad)
        {
            if (entidad.id== 0)
                throw new Exception("No se ha guardo");

            this.IConexion.Menus!.Remove(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Menus> Buscar(Menus entidad)
        {
            return this.IConexion.Menus!
                .Where(x => x.nombre!.Contains(entidad.nombre!))
                .ToList();
        }

        public List<Menus> Consultar()
        {
            return this.IConexion.Menus!.ToList();
        }

        public Menus Insertar(Menus entidad)
        {
            if (entidad.id!= 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            this.IConexion.Menus!.Add(entidad!);
            this.IConexion.SaveChanges();
            return entidad;
        }

        private bool Validar(Menus entidad)
        {
            if (string.IsNullOrEmpty(entidad.nombre))
                return false;
            return true;
        }
    }
}