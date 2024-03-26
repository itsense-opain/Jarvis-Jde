using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Motor.Tareas
{
    public static class TareaEjemplo
    {

        public static async Task Ejecutar(System.Threading.CancellationToken cancellationToken)
        {
            const string NombreInternoTareaConfigurada = "TareaEjemplo";

            #region "Obtener configuración de la tarea (No personalizar / No modificar)"
            SX.Core.MotorBase.Configuracion.Tarea Tarea = SX.Core.MotorBase.Configuracion.Administrador.Tareas.FirstOrDefault(tarea => tarea.NombreInterno.Equals(NombreInternoTareaConfigurada));
            if (Tarea == null)
                throw new System.Configuration.ConfigurationErrorsException(string.Format("No se encuentra configurada una tarea con el nombre '{0}'", NombreInternoTareaConfigurada));
            #endregion

            while (!cancellationToken.IsCancellationRequested)
            {
                // Programar aquí la lógica o invocar otra función con la lógica 
                System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();

                await Negocio.Oracle.Ejecutar();

                #region "Performance -> Si Personalizar"
                Performance.Contadores.Aumentar(Performance.Tipo.TareaMonitorCiclosCompletados);
                #endregion

                #region "Trace visual (No personalizar / No modificar)"
                System.Diagnostics.Trace.WriteLine(string.Format("{0} {1}.Core.Ejecutar {2}/{3}", DateTime.Now.ToLongTimeString(), Tarea.NombreInterno, Tarea.IntervaloCantidadAProcesar, Tarea.IntervaloSegundos));
                #endregion


                //Descansar segundos antes de continuar nuevamente (No modificar)             
                await Task.Delay(TimeSpan.FromSeconds(Tarea.IntervaloSegundos));
            }
        }

    }
}
