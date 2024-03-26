using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices;

namespace Opain.Jarvis.Motor.Integracion
{
    public class ServicioApi : IServicioApi
    {
        public ServicioApi()
        {
            var server = System.Configuration.ConfigurationManager.AppSettings["RutaServicioLocal"].ToString();
            //"http://172.31.16.230:9000/OPAIN_ApiJDE/"
            //var server = configuration.GetSection("Rutas:BaseServicio").Value;

            Cliente = new HttpClient
            {
                BaseAddress = new Uri(server),
            };

            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public HttpClient Cliente { get; set; }
        public object HttpServerUtility { get; }

        public async Task<T> DeleteAsync<T>(string servicio)
        {
            HttpResponseMessage responseMessage = await Cliente.DeleteAsync(servicio).ConfigureAwait(false);
            if (responseMessage.IsSuccessStatusCode)
            {
                // return await responseMessage.Content.ReadAsAsync<T>();
            }

            return default(T);
        }

        public async Task<T> GetAsync<T>(string servicio)
        {
            HttpResponseMessage responseMessage = await Cliente.GetAsync(servicio).ConfigureAwait(false);
            if (responseMessage.IsSuccessStatusCode)
            {

                return await responseMessage.Content.ReadAsAsync<T>();
            }

            return default(T);
        }

        public async Task<T> PostAsync<T>(string servicio, object parametros)
        {
            StringContent postParameters = new StringContent(JsonConvert.SerializeObject(parametros), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = await Cliente.PostAsync(servicio, postParameters).ConfigureAwait(false);
            if (responseMessage.IsSuccessStatusCode)
            {
                // return await responseMessage.Content.ReadAsAsync<T>();
            }

            return default(T);
        }

        public async Task<T> PutAsync<T>(string servicio, object parametros)
        {
            StringContent postParameters = new StringContent(JsonConvert.SerializeObject(parametros), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = await Cliente.PutAsync(servicio, postParameters).ConfigureAwait(false);
            if (responseMessage.IsSuccessStatusCode)
            {
                // return await responseMessage.Content.ReadAsAsync<T>();
            }

            return default(T);
        }
    }
}
