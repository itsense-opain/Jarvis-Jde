using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

using System.ServiceProcess;

namespace Opain.Jarvis.Motor
{
    [RunInstaller(true)]
    public partial class Instalador : System.Configuration.Install.Installer
    {
        public Instalador()
        {
            Initialize();
        }

        private void Initialize()
        {
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller()
            {
                Account = ServiceAccount.LocalSystem

            };

            ServiceInstaller serviceInstaller = new ServiceInstaller()
            {
                ServiceName = SX.Core.MotorBase.Configuracion.Administrador.IdentificacionServicio.NombreUnicoInterno,
                DisplayName = SX.Core.MotorBase.Configuracion.Administrador.IdentificacionServicio.NombreUnicoVisible,
                Description = SX.Core.MotorBase.Configuracion.Administrador.IdentificacionServicio.DescripcionVisible,
                StartType = ServiceStartMode.Manual
            };

            this.Installers.AddRange(new Installer[] { serviceProcessInstaller, serviceInstaller });
        }

        public override void Install(IDictionary stateSaver)
        {
            Performance.Contadores.Instalar();
            Diagnostics.EventLog.Instalar();
            base.Install(stateSaver);

            string MensajeAvisoInstalacionExitosa = string.Concat(
                "Se instalo con exito el servicio: ",
                SX.Core.MotorBase.Configuracion.Administrador.IdentificacionServicio.NombreUnicoVisible);

            Console.WriteLine(string.Empty);
            Console.WriteLine("".PadRight(MensajeAvisoInstalacionExitosa.Length, '*'));
            Console.WriteLine(MensajeAvisoInstalacionExitosa);
            Console.WriteLine("".PadRight(MensajeAvisoInstalacionExitosa.Length, '*'));
            Console.WriteLine(string.Empty);
        }

        public override void Uninstall(IDictionary savedState)
        {
            Performance.Contadores.DesInstalar();
            Diagnostics.EventLog.DesInstalar();
            base.Uninstall(savedState);

            string MensajeAvisoDesInstalacionExitosa = string.Concat(
                "Se desinstalo con exito el servicio: ",
                SX.Core.MotorBase.Configuracion.Administrador.IdentificacionServicio.NombreUnicoVisible);

            Console.WriteLine(string.Empty);
            Console.WriteLine("".PadRight(MensajeAvisoDesInstalacionExitosa.Length, '*'));
            Console.WriteLine(MensajeAvisoDesInstalacionExitosa);
            Console.WriteLine("".PadRight(MensajeAvisoDesInstalacionExitosa.Length, '*'));
            Console.WriteLine(string.Empty);
        }
    }
}
