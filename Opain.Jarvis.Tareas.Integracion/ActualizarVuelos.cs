using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Tareas
{
    public class ActualizarVuelos
    {
        protected static async Task<int> Iniciar()
        {
            const string NombreInternoTareaConfigurada = "ActualizarVuelos";
            int respuesta = 0;
            Opain.Jarvis.Tareas.Integracion.Proxy servicio;            
            try
            {
                SX.Core.MotorBase.Configuracion.Tarea Tarea = SX.Core.MotorBase.Configuracion.Administrador.Tareas.FirstOrDefault(tarea => tarea.NombreInterno.Equals(NombreInternoTareaConfigurada));
                if (Tarea == null)
                    throw new System.Configuration.ConfigurationErrorsException(string.Format("No se encuentra configurada una tarea con el nombre '{0}'", NombreInternoTareaConfigurada));

                if (Tarea.Extension == "1")
                {
                    servicio = new Integracion.Proxy();
                    //Logica
                    respuesta = await servicio.GetAsync<int>("api/Integracion/ActualizarVuelosValidados");
                }
                return respuesta;
            }
            catch (Exception e)
            {
                Diagnostico.EventLog.EscribirExcepcionNoControlada(e);
                return 0;
            }
        }
        public static async Task Ejecutar(System.Threading.CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            Diagnostico.EventLog.EscribirInformacion("Inicio del hilo de envio de vuelos validados.");
            while (!cancellationToken.IsCancellationRequested)
            {
                int Result = await Iniciar();
            }
            Diagnostico.EventLog.EscribirInformacion("Fin del hijo de envio de vuelos validados.");
            await Task.Delay(TimeSpan.FromSeconds(10));
        }

    }
}
