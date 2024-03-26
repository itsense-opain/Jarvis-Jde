using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Tareas.Integracion.General
{
    public class SystemTrace
    {
        public static void Save(int Severidad, string Mensaje, int IdEvento)
        {            
            Integracion.Dto.MensajeTrace OTD = new Integracion.Dto.MensajeTrace();
            OTD.IDEvento = IdEvento;
            OTD.Severidad = Severidad;
            OTD.Usuario = "System";
            OTD.Mensaje = Mensaje;
            string server = General.Configuracion.ServerAPIJarvis().ToString();
            try
            {
                Integracion.Metodos.Trace(server + "api/Trace/SaveAsync", OTD);
            }
            catch (Exception Excepcion)
            {
                throw;
            }
        }
    }
}
