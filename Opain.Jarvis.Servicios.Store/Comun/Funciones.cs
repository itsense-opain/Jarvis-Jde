using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.Store.Comun
{
    public class Funciones
    {
        public static DateTime TryParse(string text)
        {
            DateTime oDate;
            if (DateTime.TryParse(text, out oDate))
            {
                return oDate;
            }
            else
            {
                return DateTime.Now;
            }
        }
    }
}
