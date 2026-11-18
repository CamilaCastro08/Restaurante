using System.Text;
using Newtonsoft.Json;
using Lib_Presentaciones.Interfaces;
using Lib_Presentaciones.Nucleo;
using Libreria_restaurante.Entidades;

namespace Lib_Presentaciones.Implementaciones
{
    public class Detalles_PedidosPresentacion : IDetalles_PedidosPresentacion
    {
        private string url = Rutas.Url + "Detalles_Pedidos/";

        public async Task<Detalles_Pedidos> Insertar(Detalles_Pedidos entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Insertar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Detalles_Pedidos>(texto)!;
        }

        public async Task<Detalles_Pedidos> Actualizar(Detalles_Pedidos entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Actualizar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Detalles_Pedidos>(texto)!;
        }

        public async Task<List<Detalles_Pedidos>> Consultar()
        {
            var cliente = new HttpClient();
            var contenido = new StringContent("{}", Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Consultar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<List<Detalles_Pedidos>>(texto)!;
        }

        public async Task<List<Detalles_Pedidos>> Buscar(Detalles_Pedidos entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Buscar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<List<Detalles_Pedidos>>(texto)!;
        }

        public async Task<Detalles_Pedidos> Borrar(Detalles_Pedidos entidad)
        {
            var cliente = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(entidad), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync(this.url + "Borrar", contenido);
            var texto = await respuesta.Content.ReadAsStringAsync();
            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(texto);
            return JsonConvert.DeserializeObject<Detalles_Pedidos>(texto)!;
        }
    }
}
