using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Tareas.Integracion
{
    public class Principal : SX.Core.MotorBase.Base.MotorBase
    {
        public void Iniciar()
        {
            List<Func<System.Threading.CancellationToken, Task>> Tareas = new List<Func<System.Threading.CancellationToken, Task>>
            {
                 //Opain.Jarvis.Tareas.ActualizarVuelos.Ejecutar,
                 Opain.Jarvis.Tareas.TraerVuelos.Ejecutar,
                 Opain.Jarvis.Tareas.NotificacionSuspensiones.Ejecutar,
                 Opain.Jarvis.Tareas.Aerolineas.Ejecutar,
                 Opain.Jarvis.Tareas.Ciudades.Ejecutar,
            };

            base.IniciarTareas(Tareas);
        }
        
        public void Detener()
        {
            //Instruir a las tareas que se detengan
            this.DetenerTareasPrincipalesMasMonitorizacion();
        }

        public void Monitorear()
        {
            base.TareaMonitor = Task.Run(() =>
            {
                while (!base.CancellationTokenTareaMonitorizacion.IsCancellationRequested)
                {
                    foreach (Task laTarea in this.Tareas)
                    {
                        if (!laTarea.IsFaulted)
                            continue;

                        #region "Trace -> Si Personalizar"

                        //HACER: Dejar registro aqui de la tarea que fallo

                        #endregion

                        #region "Trace visual (No modificar)"
                        System.Diagnostics.Trace.WriteLine(
                            string.Format("{0} Monitor detecto error no controlado '{1}'",
                            DateTime.Now.ToLongTimeString(),
                            laTarea.Exception.InnerException.ToString())
                        );
                        #endregion


                        #region "Re iniciar las tareas (No modificar)"
                        //Instruir a las tareas que se detengan (No modificar)
                        base.DetenerTareasPrincipales();

                        //Volver a iniciar las tareas (No modificar)
                        this.Iniciar();
                        #endregion

                        #region "Performance (No personalizar / No modificar)"
                        //Performance.Contadores.Aumentar(Performance.Tipo.TareaMonitorErroresNoControladosCantidad);
                        #endregion

                        #region "Diagnostic (No personalizar / No modificar)"
                        //if (laTarea.Exception.InnerException != null)
                        //    Diagnostico.EventLog.EscribirExcepcionNoControlada(laTarea.Exception.InnerException);
                        //else
                        //    Diagnostico.EventLog.EscribirExcepcionNoControlada(laTarea.Exception);
                        #endregion

                        //Salir del ciclo foreach (No modificar)
                        break;
                    }

                    #region "Performance (No personalizar / No modificar)"
                    //Performance.Contadores.Aumentar(Performance.Tipo.TareaMonitorCiclosCompletados);
                    #endregion

                    //Esperar 5 segundos antes de volver a monitorizar (No modificar)
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

                    #region "Trace visual (No modificar)"
                    System.Diagnostics.Trace.WriteLine(string.Format("{0} Monitor", DateTime.Now.ToLongTimeString()));
                    #endregion
                }

            });
        }
    }
}
