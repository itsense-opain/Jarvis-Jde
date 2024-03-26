using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Tareas.Integracion
{
    partial class Agente : ServiceBase
    {
        Principal Principal = new Principal();
        public Agente()
        {
            //InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Principal = new Principal();
            Principal.Iniciar();
            Principal.Monitorear();
        }

        protected override void OnStop()
        {
            Principal.Detener();
        }
    }
}
