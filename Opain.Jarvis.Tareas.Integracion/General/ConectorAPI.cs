using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Data;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Authentication;

namespace Opain.Jarvis.Tareas.Integracion.General
{
    public class ConectorAPI
    {

        string sNameSpace;
        public NetworkCredential aCredencial { get; set; }
        int nQuantityOfObjects;
        public string aUrl { get; set; }
        private bool soaPAction;
        private string prefObjSend;
        private int timeOut;
        public string aMethod { get; set; }
        public List<ObjectHeaders> aHeaders { get; set; }
        public List<ObjectContentType> aContent { get; set; }
        public List<ObjectParams> aParams { get; set; }
        private int aNumParams { get; set; }
        public string aDatos { get; set; }

        public class ObjectHeaders
        {
            public string aNombre { get; set; }
            public string aValor { get; set; }

            public ObjectHeaders(string nombre, string valor)
            {
                aNombre = nombre;
                aValor = valor;
            }
        }

        public class ObjectContentType
        {
            public string aNombre { get; set; }
            public string aValor { get; set; }

            public ObjectContentType(string valor)
            {
                aNombre = string.Empty;
                aValor = valor;
            }
        }

        public class ObjectParams
        {
            public string aNombre { get; set; }
            public string aValor { get; set; }

            public ObjectParams(string nombre, string valor)
            {
                aNombre = nombre;
                aValor = valor;
            }
        }

        public ConectorAPI()
        {
            aMethod = "POST";
            aHeaders = new List<ObjectHeaders>();
            aContent = new List<ObjectContentType>();
            aParams = new List<ObjectParams>();
            aDatos = string.Empty;
        }

        public DataSet CargarXmlEnDs(string pstrXml)
        {
            DataSet dsDataSource = new DataSet();
            StringReader sr = new StringReader(pstrXml);
            try
            {
                dsDataSource.ReadXml(sr);
            }
            catch (Exception Ex)
            {
                throw new ArgumentException(Ex.Message);
            }
            return dsDataSource;
        }

        private StreamReader Run(HttpWebRequest objReq, HttpWebResponse objResp)
        {
            SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            // SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            aNumParams = 0;
            foreach (ObjectParams oParam in aParams)
            {
                if (aMethod != "PUT")
                {
                    if (aNumParams == 0)
                    {
                        aUrl = aUrl + "?" + oParam.aNombre + "=" + oParam.aValor;
                        aNumParams++;
                    }
                    else
                    {
                        aUrl = aUrl + "&" + oParam.aNombre + "=" + oParam.aValor;
                        aNumParams++;
                    }
                }
                else
                    aUrl = aUrl + "\\" + oParam.aValor;
            }

            objReq = (HttpWebRequest)WebRequest.Create(aUrl);
            objReq.KeepAlive = false;
            objReq.Method = aMethod;
            objReq.UserAgent = ".NET Framework Test Client";

            foreach (ObjectContentType objContent in aContent)
            {
                objReq.ContentType = objContent.aValor;
            }
            foreach (ObjectHeaders objHeaders in aHeaders)
            {
                objReq.Headers.Add(objHeaders.aNombre, objHeaders.aValor);
            }
            if (aCredencial != null)
                objReq.Credentials = aCredencial;

            if (aDatos != string.Empty)
            {
                using (var streamWriter = new StreamWriter(objReq.GetRequestStream()))
                {
                    string json = aDatos;
                    streamWriter.Write(json);
                }
            }
            objResp = (HttpWebResponse)objReq.GetResponse();
            StreamReader reader = new StreamReader(objResp.GetResponseStream());
            return reader;
        }

        public DataSet Invoke(HttpWebRequest objReq, HttpWebResponse objResp)
        {
            StreamReader oReturn = Run(objReq, objResp);
            return CargarXmlEnDs(oReturn.ReadToEnd());
        }

        public JObject ObtenerObjectDynamico(HttpWebRequest objReq, HttpWebResponse objResp)
        {
            StreamReader oReturn = Run(objReq, objResp);
            JObject stuff = JObject.Parse(oReturn.ReadToEnd());
            return stuff;
        }

        public string GetString(HttpWebRequest objReq, HttpWebResponse objResp)
        {
            StreamReader oReturn = Run(objReq, objResp);
            return oReturn.ReadToEnd();
        }


    }
}
