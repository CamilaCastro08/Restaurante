using System.Text;
using Newtonsoft.Json;
using Lib_Presentaciones.Interfaces;
using Lib_Presentaciones.Nucleo;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class UsuariosPresentacion : IUsuariosPresentacion
    {
        private string url = Rutas.Url + "Usuarios/";

        public async Task<Usuarios> Insertar(Usuarios entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Insertar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Usuarios>(texto)!;
        }

        public async Task<Usuarios> Actualizar(Usuarios entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Actualizar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Usuarios>(texto)!;
        }

        public async Task<List<Usuarios>> Consultar()
        {
            var cliente = new HttpClient();
            var contenido = new StringContent("{}", Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Consultar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<List<Usuarios>>(texto)!;
        }

        public async Task<List<Usuarios>> Buscar(Usuarios entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Buscar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<List<Usuarios>>(texto)!;
        }

        public async Task<Usuarios> Borrar(Usuarios entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Borrar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Usuarios>(texto)!;
        }

        public async Task<Usuarios> Login(Usuarios entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Login", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Usuarios>(texto)!;
        }
    }
}
