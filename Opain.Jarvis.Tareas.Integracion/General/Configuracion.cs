using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Opain.Jarvis.Tareas.Integracion.General
{
    public class Configuracion
    {
        private static string Seccion(string Item)
        {
            try
            {
                NameValueCollection Config = new NameValueCollection();
                Config = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection("SX.OPAIN");
                return Config[Item].ToString();
            }
            catch (ApplicationException Excepcion)
            {
                throw new Exception("No se identifico dentro de la sección SX.ITAU el item de configuracion " + Item + " Generando la sigueinte excepcion " + Excepcion.Message);
            }
        }

        public static string ServerAPI()
        {
            return Seccion("RutaServicioLocal");
        }

        public static string ServerAPIJarvis()
        {
            return Seccion("RutaServicioJarvis");
        }
        public static string CredencialesUser()
        {
            return Seccion("User");
        }
        public static string CredencialesPass()
        {
            return Seccion("Pass");
        }

    }
}
