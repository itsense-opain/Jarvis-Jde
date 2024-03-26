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

namespace Opain.Jarvis.Tareas.Integracion
{
    public class Proxy : IProxy
    {
        public HttpClient Cliente { get; set; }
        public object HttpServerUtility { get; }
        public Proxy()
        {
            var server = General.Configuracion.ServerAPI().ToString();
           
            Cliente = new HttpClient
            {
                BaseAddress = new Uri(server),
            };

            Cliente.DefaultRequestHeaders.Accept.Clear();
            Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
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

                //return await responseMessage.Content.ReadAsStringAsync<T>();
            }
            return default(T);


            //HttpResponseMessage response = Cliente.GetAsync(servicio).Result; 
            //try
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var message = response.Content.ReadAsStringAsync().Result;
            //        var result1 = JsonConvert.DeserializeObject<T>(message);
            //        var o = (T)Convert.ChangeType(result1, typeof(T));
            //        //var result2 = response.Content.ReadAsAsync<T>().Result;
            //    }
            //    else
            //    {
            //        throw new Exception(response.Content.ToString());
            //    }
            //}
            //catch(Exception o)
            //{
            //    throw;
            //}
            //return default(T);
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
