using System.Text;
using Newtonsoft.Json;
using Lib_Presentaciones.Interfaces;
using Lib_Presentaciones.Nucleo;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class PermisosPresentacion : IPermisosPresentacion
    {
        private string url = Rutas.Url + "Permisos/";

        public async Task<Permisos> Insertar(Permisos entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Insertar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Permisos>(texto)!;
        }

        public async Task<Permisos> Actualizar(Permisos entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Actualizar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Permisos>(texto)!;
        }

        public async Task<List<Permisos>> Consultar()
        {
            var cliente = new HttpClient();
            var contenido = new StringContent("{}", Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Consultar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<List<Permisos>>(texto)!;
        }

        public async Task<List<Permisos>> Buscar(Permisos entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Buscar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<List<Permisos>>(texto)!;
        }

        public async Task<Permisos> Borrar(Permisos entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Borrar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Permisos>(texto)!;
        }
    }
}
