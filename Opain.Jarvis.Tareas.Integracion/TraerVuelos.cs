using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Opain.Jarvis.Tareas
{
    public class TraerVuelos
    {
        protected static async Task<int> Iniciar(int Ejecucion)
        {
            try
            {
                const string NombreInternoTareaConfigurada = "TraerDatosJDE";
                List<Integracion.Dto.Vuelo> respuesta;
                Opain.Jarvis.Tareas.Integracion.Metodos servicio;
                SX.Core.MotorBase.Configuracion.Tarea Tarea = SX.Core.MotorBase.Configuracion.Administrador.Tareas.FirstOrDefault(tarea => tarea.NombreInterno.Equals(NombreInternoTareaConfigurada));
                if (Tarea == null)
                    throw new System.Configuration.ConfigurationErrorsException(string.Format("No se encuentra configurada una tarea con el nombre '{0}'", NombreInternoTareaConfigurada));

                int HoraActual = DateTime.Now.Hour;
                if (HoraActual.ToString() == Tarea.Extension)
                {
                    if (Ejecucion < Tarea.IntervaloCantidadAProcesar)
                    {
                        Ejecucion++;
                        List<Integracion.General.ConectorAPI.ObjectParams> Params = new List<Integracion.General.ConectorAPI.ObjectParams>();
                        //Params.Add(new Integracion.General.ConectorAPI.ObjectParams("fechaIntegracion", "01/11/2022"));
                        Params.Add(new Integracion.General.ConectorAPI.ObjectParams("fechaIntegracion", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")));
                        servicio = new Opain.Jarvis.Tareas.Integracion.Metodos();
                        //Logica
                        respuesta = await servicio.SincronizarVuelos("api/Integracion/TraerVuelos", Params); //+ DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") );                       
                        Integracion.General.SystemTrace.Save(1, JsonConvert.SerializeObject(respuesta), 9001);
                    }
                    else
                    {
                        Integracion.General.SystemTrace.Save(2, "Número de ejecuciones cumplidas para hoy", 9001);
                    }
                } 
                else
                {
                    Ejecucion = 0;
                    //Integracion.General.SystemTrace.Save(2, "Hora de ejecución no disponible", 9000);
                }
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
            await Task.Delay(TimeSpan.FromSeconds(5));
            Diagnostico.EventLog.EscribirInformacion("Inicio del hilo de traer los vuelos desde JDEdwars.");
            int NumeroEjecuciones = 0;
            while (!cancellationToken.IsCancellationRequested)
            {                
                NumeroEjecuciones = await Iniciar(NumeroEjecuciones);
            }
            Diagnostico.EventLog.EscribirInformacion("Fin del hilo de traer los vuelos desde JDEdwars.");
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}
