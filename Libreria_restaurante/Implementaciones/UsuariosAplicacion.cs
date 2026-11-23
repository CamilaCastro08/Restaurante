using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Libreria_restaurante.Implementaciones
{
    public class UsuariosAplicacion : IUsuariosAplicacion
    {
        private IConexion IConexion;

        public UsuariosAplicacion(IConexion IConexion)
        {
            this.IConexion = IConexion;
        }

        public Usuarios Insertar(Usuarios entidad)
        {
            if (entidad.id != 0)
                throw new Exception("Ya se guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            if (string.IsNullOrEmpty(entidad.contraseña))
                throw new Exception("Falta informacion");

            if (this.IConexion.Usuarios!.Any(x => x.nombreUsuario == entidad.nombreUsuario))
                throw new Exception("Ya existe el usuario");

            entidad.fechaCreacion = DateTime.Now;

            this.IConexion.Usuarios!.Add(entidad);
            this.IConexion.SaveChanges();

            var audit = new Auditorias
            {
                tabla = "Usuarios",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "id: " + entidad.id + ", nombreUsuario: " + entidad.nombreUsuario + ", correo: " + entidad.correo + ", rol: " + entidad.rol,
                usuario = entidad.id
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();

            this.IConexion.Entry<Usuarios>(entidad).State = EntityState.Detached;
            entidad.contraseña = null;
            return entidad;
        }

        public Usuarios Actualizar(Usuarios entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            if (Validar(entidad))
                throw new Exception("Falta informacion");

            var anterior = this.IConexion.Usuarios!.AsNoTracking().FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("No existe");

            if (string.IsNullOrEmpty(entidad.contraseña))
                entidad.contraseña = anterior.contraseña;

            entidad.fechaCreacion = anterior.fechaCreacion;

            var entry = this.IConexion.Entry<Usuarios>(entidad);
            entry.State = EntityState.Modified;

            var audit = new Auditorias
            {
                tabla = "Usuarios",
                accion = "Actualizar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + anterior.id + ", nombreUsuario: " + anterior.nombreUsuario + ", correo: " + anterior.correo + ", rol: " + anterior.rol,
                datosNuevos = "id: " + entidad.id + ", nombreUsuario: " + entidad.nombreUsuario + ", correo: " + entidad.correo + ", rol: " + entidad.rol,
                usuario = entidad.id
            };
            this.IConexion.Auditorias!.Add(audit);
            this.IConexion.SaveChanges();

            this.IConexion.Entry<Usuarios>(entidad).State = EntityState.Detached;
            entidad.contraseña = null;
            return entidad;
        }

        public List<Usuarios> Consultar()
        {
            var lista = this.IConexion.Usuarios!
                .Include(x => x._rol).ToList();
            foreach (var item in lista)
                item.contraseña = null;
            return lista;
        }

        public List<Usuarios> Buscar(Usuarios entidad)
        {
            var lista = this.IConexion.Usuarios!
                .Include(x => x._rol)
                .Where(x => x.nombreUsuario!.Contains(entidad.nombreUsuario!))
                .ToList();
            foreach (var item in lista)
                item.contraseña = null;
            return lista;
        }

        public Usuarios Borrar(Usuarios entidad)
        {
            if (entidad.id == 0)
                throw new Exception("No se ha guardo");

            var itemDb = this.IConexion.Usuarios!.FirstOrDefault(x => x.id == entidad.id);
            if (itemDb == null)
                throw new Exception("El registro no existe o ya fue eliminado");

            try
            {
                this.IConexion.Usuarios!.Remove(itemDb);

                var audit = new Auditorias
                {
                tabla = "Usuarios",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = "id: " + entidad.id + ", nombreUsuario: " + entidad.nombreUsuario + ", correo: " + entidad.correo + ", rol: " + entidad.rol,
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

        public Usuarios Login(Usuarios entidad)
        {
            var usuario = this.IConexion.Usuarios!
                .FirstOrDefault(x => x.nombreUsuario == entidad.nombreUsuario && x.activo == true);

            if (usuario == null || string.IsNullOrEmpty(entidad.contraseña) || usuario.contraseña != entidad.contraseña)
                throw new Exception("Usuario o contraseña incorrectos");

            this.IConexion.Auditorias!.Add(new Auditorias
            {
                tabla = "Usuarios",
                accion = "Login",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = "Inicio de sesiÃ³n: " + usuario.nombreUsuario,
                usuario = usuario.id
            });
            this.IConexion.SaveChanges();

            usuario.contraseña = null;
            return usuario;
        }

        private bool Validar(Usuarios entidad)
        {
            return string.IsNullOrEmpty(entidad.nombreUsuario) ||
                string.IsNullOrEmpty(entidad.correo) ||
                entidad.rol == null || entidad.rol == 0;
        }
    }
}
