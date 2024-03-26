using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opain.Jarvis.Tareas.Integracion;

namespace Opain.Jarvis.Tareas
{
    public class NotificacionSuspensiones
    {
        protected static async Task<int> Iniciar(int Ejecucion)
        {
            try
            {
                const string NombreInternoTareaConfigurada = "Notificaciones";
                bool respuesta;
                Opain.Jarvis.Tareas.Integracion.Metodos servicio;
                SX.Core.MotorBase.Configuracion.Tarea Tarea = SX.Core.MotorBase.Configuracion.Administrador.Tareas.FirstOrDefault(tarea => tarea.NombreInterno.Equals(NombreInternoTareaConfigurada));
                if (Tarea == null)
                    throw new System.Configuration.ConfigurationErrorsException(string.Format("No se encuentra configurada una tarea con el nombre '{0}'", NombreInternoTareaConfigurada));
                        
                Ejecucion++;
                List<Integracion.General.ConectorAPI.ObjectParams> Params = new List<Integracion.General.ConectorAPI.ObjectParams>();
                Params.Add(new Integracion.General.ConectorAPI.ObjectParams("fechaIntegracion", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")));
                servicio = new Opain.Jarvis.Tareas.Integracion.Metodos();
                servicio.server = Opain.Jarvis.Tareas.Integracion.General.Configuracion.ServerAPIJarvis();
                respuesta = await servicio.Notificaciones("api/vuelos/NotificacionesSuspensionesRetiradasAsync"); //+ DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") );                       
                Integracion.General.SystemTrace.Save(1, JsonConvert.SerializeObject(respuesta), 9002);

                return Ejecucion;
            }
            catch (Exception e)
            {
                Diagnostico.EventLog.EscribirExcepcionNoControlada(e);
                return 0;
            }
        }
        public static async Task Ejecutar(System.Threading.CancellationToken cancellationToken)
        {
            const string NombreInternoTareaConfigurada = "Notificaciones";
            SX.Core.MotorBase.Configuracion.Tarea Tarea = SX.Core.MotorBase.Configuracion.Administrador.Tareas.FirstOrDefault(tarea => tarea.NombreInterno.Equals(NombreInternoTareaConfigurada));
            Diagnostico.EventLog.EscribirInformacion("Inicio del hilo de notificación de suspensiones.");
            int NumeroEjecuciones = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(Tarea.IntervaloSegundos));
                NumeroEjecuciones = await Iniciar(NumeroEjecuciones);
            }
            Diagnostico.EventLog.EscribirInformacion("Fin del hilo de notificación de suspensiones.");
            await Task.Delay(TimeSpan.FromSeconds(Tarea.IntervaloCantidadAProcesar));
        }
    }
}
