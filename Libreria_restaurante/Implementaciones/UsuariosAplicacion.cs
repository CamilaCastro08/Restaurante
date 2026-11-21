using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Libreria_restaurante.Implementaciones
{
    public class UsuariosAplicacion : IUsuariosAplicacion
    {
        private IConexion? IConexion = null;

        public UsuariosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Usuarios Login(Usuarios entidad)
        {
            var usuario = this.IConexion!.Usuarios!.FirstOrDefault(x => x.nombreUsuario == entidad.nombreUsuario && x.activo == true);
            if (usuario == null || string.IsNullOrEmpty(entidad.contraseña) || entidad.contraseña != usuario.contraseña)
                throw new Exception("Usuario o contraseña incorrectos");

            usuario.contraseña = null;
            return usuario;
        }

        private bool Validar(Usuarios entidad)
        {
            return string.IsNullOrEmpty(entidad.nombreUsuario) ||
                string.IsNullOrEmpty(entidad.correo) ||
                entidad.rol == null || entidad.rol == 0;
        }

        public Usuarios? Insertar(Usuarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.id != 0)
                throw new Exception("lbYaSeGuardo");
            if (Validar(entidad) || string.IsNullOrEmpty(entidad.contraseña))
                throw new Exception("lbFaltaInformacion");

            var existe = this.IConexion!.Usuarios!.Any(x => x.nombreUsuario == entidad.nombreUsuario || x.correo == entidad.correo);
            if (existe)
                throw new Exception("El usuario o correo ya existe");

            entidad.fechaCreacion = DateTime.Now;
            this.IConexion!.Usuarios!.Add(entidad);
            this.IConexion!.SaveChanges();

            RegistrarAuditoria("Insertar", null, entidad);
            entidad.contraseña = null;
            return entidad;
        }

        public Usuarios? Actualizar(Usuarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.id == 0)
                throw new Exception("lbNoSeGuardo");
            if (Validar(entidad))
                throw new Exception("lbFaltaInformacion");

            var anterior = this.IConexion!.Usuarios!.AsNoTracking().FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("Usuario no encontrado");

            if (string.IsNullOrEmpty(entidad.contraseña))
            {
                entidad.contraseña = anterior.contraseña;
            }

            entidad.fechaCreacion = anterior.fechaCreacion;

            var entry = this.IConexion!.Entry<Usuarios>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion!.SaveChanges();

            RegistrarAuditoria("Actualizar", anterior, entidad);
            entidad.contraseña = null;
            return entidad;
        }

        public Usuarios? Borrar(Usuarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.id == 0)
                throw new Exception("lbNoSeGuardo");

            var anterior = this.IConexion!.Usuarios!.AsNoTracking().FirstOrDefault(x => x.id == entidad.id);

            this.IConexion!.Usuarios!.Remove(entidad);
            this.IConexion!.SaveChanges();

            RegistrarAuditoria("Borrar", anterior, null);
            return entidad;
        }

        public List<Usuarios> Consultar()
        {
            var lista = this.IConexion!.Usuarios!
                .Include(x => x._rol)
                .Take(20)
                .ToList();

            lista.ForEach(x => x.contraseña = null);
            return lista;
        }

        public List<Usuarios> Buscar(Usuarios? entidad)
        {
            if (entidad == null)
                return Consultar();

            var lista = this.IConexion!.Usuarios!
                .Include(x => x._rol)
                .Where(x => x.nombreUsuario!.Contains(entidad.nombreUsuario ?? "") &&
                            x.correo!.Contains(entidad.correo ?? ""))
                .Take(20)
                .ToList();

            lista.ForEach(x => x.contraseña = null);
            return lista;
        }

        private void RegistrarAuditoria(string accion, Usuarios? anterior, Usuarios? nuevo)
        {
            var antCopia = anterior == null ? null : new { anterior.id, anterior.nombreUsuario, anterior.correo, anterior.activo, anterior.rol };
            var nueCopia = nuevo == null ? null : new { nuevo.id, nuevo.nombreUsuario, nuevo.correo, nuevo.activo, nuevo.rol };

            var auditoria = new Auditorias
            {
                tabla = "Usuarios",
                accion = accion,
                fecha = DateTime.Now,
                datosAnteriores = antCopia == null ? "" : JsonConvert.SerializeObject(antCopia),
                datosNuevos = nueCopia == null ? "" : JsonConvert.SerializeObject(nueCopia)
            };
            this.IConexion!.Auditorias!.Add(auditoria);
            this.IConexion!.SaveChanges();
        }
    }
}