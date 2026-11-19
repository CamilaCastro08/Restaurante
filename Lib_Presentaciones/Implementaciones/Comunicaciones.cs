using System.Text;
using Newtonsoft.Json;
using Lib_Presentaciones.Interfaces;
using Lib_Presentaciones.Nucleo;

namespace Lib_Presentaciones.Implementaciones
{
    public class Comunicaciones : IComunicaciones
    {
        public async Task<T> Ejecutar<T>(string url, object? entidad)
        {
            url = Rutas.Url + url;

            var body = new StringContent(
                JsonConvert.SerializeObject(entidad ?? new object()),
                Encoding.UTF8, "application/json");

            using var cliente_http = new HttpClient();
            cliente_http.Timeout = new TimeSpan(0, 4, 0);

            var message = await cliente_http.PostAsync(url, body);
            var respuesta = await message.Content.ReadAsStringAsync();

            // El servicio responde 400 con el mensaje de negocio y la pantalla lo muestra tal cual.
            if (!message.IsSuccessStatusCode)
                throw new Exception(respuesta);

            return JsonConvert.DeserializeObject<T>(respuesta)!;
        }
    }
}
