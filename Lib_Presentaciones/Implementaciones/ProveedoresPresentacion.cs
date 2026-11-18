using System.Text;
using Newtonsoft.Json;
using Lib_Presentaciones.Interfaces;
using Lib_Presentaciones.Nucleo;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class ProveedoresPresentacion : IProveedoresPresentacion
    {
        private string url = Rutas.Url + "Proveedores/";

        public async Task<Proveedores> Insertar(Proveedores entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Insertar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Proveedores>(texto)!;
        }

        public async Task<Proveedores> Actualizar(Proveedores entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Actualizar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Proveedores>(texto)!;
        }

        public async Task<List<Proveedores>> Consultar()
        {
            var cliente = new HttpClient();
            var contenido = new StringContent("{}", Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Consultar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<List<Proveedores>>(texto)!;
        }

        public async Task<List<Proveedores>> Buscar(Proveedores entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Buscar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<List<Proveedores>>(texto)!;
        }

        public async Task<Proveedores> Borrar(Proveedores entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Borrar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Proveedores>(texto)!;
        }
    }
}
