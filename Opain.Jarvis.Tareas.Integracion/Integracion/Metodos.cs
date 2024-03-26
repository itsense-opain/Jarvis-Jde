using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

using Microsoft.Extensions.Configuration;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;


using System.Runtime.InteropServices;

namespace Opain.Jarvis.Tareas.Integracion
{
    public class Metodos
    {
        public string server { get; set; }
        public Metodos()
        {
            server = General.Configuracion.ServerAPI().ToString();            
        }
        public async Task<List<Integracion.Dto.Vuelo>> SincronizarVuelos(string endPoint, List<General.ConectorAPI.ObjectParams> Param)
        {
            Opain.Jarvis.Tareas.Integracion.General.ConectorAPI Conector = new General.ConectorAPI();
            Conector.aMethod = "GET";
            Conector.aUrl = server + endPoint;
            Conector.aContent.Add(new General.ConectorAPI.ObjectContentType("application/json"));
            Conector.aParams = Param;
            HttpWebRequest Request = null;
            HttpWebResponse Response = null;
            JObject ResultJson = new JObject();
            string Result = "";
            List<Integracion.Dto.Vuelo> ObjetoResultado;
            try
            {
                System.Data.DataSet ODs = new System.Data.DataSet();
                Result = Conector.GetString(Request, Response);
                ObjetoResultado = JsonConvert.DeserializeObject<List<Opain.Jarvis.Tareas.Integracion.Dto.Vuelo>>(Result);
            }
            catch (WebException ex)
            {
                var Body = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(Conector.aMethod + " >> " + Body.ToString());
            }
            catch (Exception Exception)
            {
                throw new Exception(Conector.aMethod + " > " + Exception.Message);

            }
            return ObjetoResultado;
        }

        public async Task<bool> SincronizarMaestra(string endPoint)
        {
            Opain.Jarvis.Tareas.Integracion.General.ConectorAPI Conector = new General.ConectorAPI();
            Conector.aMethod = "GET";
            Conector.aUrl = server + endPoint;
            Conector.aContent.Add(new General.ConectorAPI.ObjectContentType("application/json"));            
            HttpWebRequest Request = null;
            HttpWebResponse Response = null;
            JObject ResultJson = new JObject();
            string Result = "";            
            try
            {
                System.Data.DataSet ODs = new System.Data.DataSet();
                Result = Conector.GetString(Request, Response);
                return true;
            }
            catch (WebException ex)
            {
                var Body = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(Conector.aMethod + " >> " + Body.ToString());
            }
            catch (Exception Exception)
            {
                throw new Exception(Conector.aMethod + " > " + Exception.Message);

            }            
        }
        public async Task<bool> Notificaciones(string endPoint)
        {
            string token;
            try
            {
                string user = General.Configuracion.CredencialesUser();
                string pass = General.Configuracion.CredencialesPass();
                token = Token(server + "api/Cuenta/ObtenerTokenJwt?usr=" + user + "&psw=" + pass);
            }
            catch(Exception e)
            {
                throw;
            }            
            Opain.Jarvis.Tareas.Integracion.General.ConectorAPI Conector = new General.ConectorAPI();
            Conector.aMethod = "GET";
            Conector.aUrl = server + endPoint;
            Conector.aContent.Add(new General.ConectorAPI.ObjectContentType("application/json"));
            Conector.aHeaders.Add(new General.ConectorAPI.ObjectHeaders("Authorization", "Bearer " + token));
            HttpWebRequest Request = null;
            HttpWebResponse Response = null;
            JObject ResultJson = new JObject();
            string Result = "";
            bool ObjetoResultado;
            try
            {
                System.Data.DataSet ODs = new System.Data.DataSet();
                Result = Conector.GetString(Request, Response);
                ObjetoResultado = JsonConvert.DeserializeObject<bool>(Result);
            }
            catch (WebException ex)
            {
                var Body = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(Conector.aMethod + " >> " + Body.ToString());
            }
            catch (Exception Exception)
            {
                throw new Exception(Conector.aMethod + " > " + Exception.Message);

            }
            return ObjetoResultado;
        }

        public static void Trace(string endPoint, Dto.MensajeTrace Mensaje )
        {   
            General.ConectorAPI Conector = new General.ConectorAPI();
            Conector.aMethod = "POST";
            Conector.aUrl = endPoint;
            Conector.aContent.Add(new General.ConectorAPI.ObjectContentType("application/json"));
            Conector.aDatos = JsonConvert.SerializeObject(Mensaje); 

            HttpWebRequest Request = null;
            HttpWebResponse Response = null;
            JObject ResultJson = new JObject();
            string Result = "";
            List<Integracion.Dto.Vuelo> ObjetoResultado;
            try
            {
                System.Data.DataSet ODs = new System.Data.DataSet();
                Result = Conector.GetString(Request, Response);
                ObjetoResultado = JsonConvert.DeserializeObject<List<Opain.Jarvis.Tareas.Integracion.Dto.Vuelo>>(Result);
            }
            catch (WebException ex)
            {
                var Body = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(Conector.aMethod + " >> " + Body.ToString());
            }
            catch (Exception Exception)
            {
                throw new Exception(Conector.aMethod + " > " + Exception.Message);

            }            
        }
        private static string Token(string endPoint)
        {
            General.ConectorAPI Conector = new General.ConectorAPI();
            Conector.aMethod = "GET";
            Conector.aUrl = endPoint;
            Conector.aContent.Add(new General.ConectorAPI.ObjectContentType("application/json"));
            
            HttpWebRequest Request = null;
            HttpWebResponse Response = null;
            JObject ResultJson = new JObject();
            string Result = "";            
            try
            {
                System.Data.DataSet ODs = new System.Data.DataSet();
                Result = Conector.GetString(Request, Response);                
                return Result;
            }
            catch (WebException ex)
            {
                var Body = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(Conector.aMethod + " >> " + Body.ToString());
            }
            catch (Exception Exception)
            {
                throw new Exception(Conector.aMethod + " > " + Exception.Message);

            }
        }
    }
}
