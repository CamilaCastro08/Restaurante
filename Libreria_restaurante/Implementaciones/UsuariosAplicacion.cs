using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
            var usuario = this.IConexion!.Usuarios!
                .FirstOrDefault(u => u.nombreUsuario == entidad.nombreUsuario
                                  && u.contraseña == entidad.contraseña
                                  && u.activo == true);

            if (usuario == null)
                throw new Exception("Usuario o contraseña incorrectos");

            usuario.contraseña = null;
            return usuario;
        }

        public List<Usuarios> Consultar()
        {
            var lista = this.IConexion!.Usuarios!
                .Include(x => x._rol)
                .Take(50)
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
                .Take(50)
                .ToList();

            lista.ForEach(x => x.contraseña = null);
            return lista;
        }

        public Usuarios? Insertar(Usuarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.id != 0)
                throw new Exception("lbYaSeGuardo");
            if (string.IsNullOrEmpty(entidad.nombreUsuario) ||
                string.IsNullOrEmpty(entidad.correo) ||
                string.IsNullOrEmpty(entidad.contraseña) ||
                entidad.rol == null || entidad.rol == 0)
                throw new Exception("lbFaltaInformacion");

            entidad.fechaCreacion = DateTime.Now;
            this.IConexion!.Usuarios!.Add(entidad);
            this.IConexion.SaveChanges();

            this.IConexion!.UsuariosAuditorias!.Add(new UsuariosAuditorias
            {
                tabla = "Usuarios",
                accion = "Insertar",
                fecha = DateTime.Now,
                datosAnteriores = "",
                datosNuevos = JsonSerializer.Serialize(new { entidad.id, entidad.nombreUsuario, entidad.correo, entidad.activo, entidad.rol })
            });
            this.IConexion.SaveChanges();

            entidad.contraseña = null;
            return entidad;
        }

        public Usuarios? Actualizar(Usuarios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.id == 0)
                throw new Exception("lbNoSeGuardo");

            var anterior = this.IConexion!.Usuarios!.AsNoTracking().FirstOrDefault(x => x.id == entidad.id);
            if (anterior == null)
                throw new Exception("Usuario no encontrado");

            if (string.IsNullOrEmpty(entidad.contraseña))
                entidad.contraseña = anterior.contraseña;

            entidad.fechaCreacion = anterior.fechaCreacion;

            this.IConexion!.Usuarios!.Update(entidad);
            this.IConexion.SaveChanges();

            this.IConexion!.UsuariosAuditorias!.Add(new UsuariosAuditorias
            {
                tabla = "Usuarios",
                accion = "Modificar",
                fecha = DateTime.Now,
                datosAnteriores = JsonSerializer.Serialize(new { anterior.id, anterior.nombreUsuario, anterior.correo, anterior.activo, anterior.rol }),
                datosNuevos = JsonSerializer.Serialize(new { entidad.id, entidad.nombreUsuario, entidad.correo, entidad.activo, entidad.rol })
            });
            this.IConexion.SaveChanges();

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
            this.IConexion.SaveChanges();

            this.IConexion!.UsuariosAuditorias!.Add(new UsuariosAuditorias
            {
                tabla = "Usuarios",
                accion = "Borrar",
                fecha = DateTime.Now,
                datosAnteriores = anterior == null ? "" : JsonSerializer.Serialize(new { anterior.id, anterior.nombreUsuario, anterior.correo, anterior.activo, anterior.rol }),
                datosNuevos = ""
            });
            this.IConexion.SaveChanges();

            return entidad;
        }
    }
}