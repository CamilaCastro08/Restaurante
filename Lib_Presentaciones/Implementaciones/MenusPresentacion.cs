using System.Text;
using Newtonsoft.Json;
using Lib_Presentaciones.Interfaces;
using Lib_Presentaciones.Nucleo;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class MenusPresentacion : IMenusPresentacion
    {
        private string url = Rutas.Url + "Menus/";

        public async Task<Menus> Insertar(Menus entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Insertar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Menus>(texto)!;
        }

        public async Task<Menus> Actualizar(Menus entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Actualizar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Menus>(texto)!;
        }

        public async Task<List<Menus>> Consultar()
        {
            var cliente = new HttpClient();
            var contenido = new StringContent("{}", Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Consultar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<List<Menus>>(texto)!;
        }

        public async Task<List<Menus>> Buscar(Menus entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Buscar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<List<Menus>>(texto)!;
        }

        public async Task<Menus> Borrar(Menus entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Borrar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Menus>(texto)!;
        }
    }
}
