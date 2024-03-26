using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Motor.Diagnostics
{
    public static class EventLog
    {
        static string Origen = SX.Core.MotorBase.Configuracion.Administrador.IdentificacionServicio.NombreUnicoInterno;
        static string NombreLog = "Application";

        public static void Instalar()
        {
            if (System.Diagnostics.EventLog.SourceExists(Origen))
                System.Diagnostics.EventLog.DeleteEventSource(Origen);

            if (!System.Diagnostics.EventLog.SourceExists(Origen))
                System.Diagnostics.EventLog.CreateEventSource(Origen, NombreLog);
        }

        public static void DesInstalar()
        {
            if (System.Diagnostics.EventLog.SourceExists(Origen))
                System.Diagnostics.EventLog.DeleteEventSource(Origen);
        }

        public static void EscribirExcepcionNoControlada(System.Exception ex)
        {
            System.Diagnostics.EventLog EventLogReal = new System.Diagnostics.EventLog();
            EventLogReal.Source = Origen;
            EventLogReal.WriteEntry(ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
        }

        public static void EscribirInformacion(string mensaje)
        {
            System.Diagnostics.EventLog EventLogReal = new System.Diagnostics.EventLog();
            EventLogReal.Source = Origen;
            EventLogReal.WriteEntry(mensaje, System.Diagnostics.EventLogEntryType.Information);
        }
    }
}
