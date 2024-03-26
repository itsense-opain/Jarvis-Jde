using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Motor
{
    static class Program
    {
        static void Main()
        {
            /*
             * Asignar aquí a una constante estatica que identifique el id-de-aplicación el valor que se obtiene mediante la propiedad
             * 'SX.Core.MotorBase.Configuracion.Administrador.IdentificacionServicio.IdAplicacion' que lee el valor parametrizado en
             * SX.Motor/IdentificacionServicio/@IdAplicacion            
            */

            if (!Environment.UserInteractive)
            {
                #region "Esta sección se ejecuta cuando es iniciado desde el panel de servicios de windows"

                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new MainServicio()
                };
                ServiceBase.Run(ServicesToRun);

                return;

                #endregion
            }

            #region "Esta sección se ejecuta cuando es iniciado directamente. Desde Visual Studio en modo debug o via doble clic"

            Console.WriteLine(string.Empty);
            Console.WriteLine(string.Format("Se dispone a ejecutar el servicio '{0}' en modo manual", SX.Core.MotorBase.Configuracion.Administrador.IdentificacionServicio.NombreUnicoVisible));

            ConsoleKey KeyPressed;

            while (true)
            {
                Console.WriteLine("-  Presione 'C' para continuar y ejecutar en modo manual");
                Console.WriteLine("-  Presione 'I' para instalar el servicio");
                Console.WriteLine("-  Presione 'U' para desinstalar el servicio");
                Console.WriteLine("-  Presione 'Escape' para salir");
                Console.WriteLine(string.Empty);
                KeyPressed = EsperarTecla();

                if (KeyPressed == ConsoleKey.Escape)
                    return;

                if (KeyPressed == ConsoleKey.C || KeyPressed == ConsoleKey.I || KeyPressed == ConsoleKey.U)
                    break;
            }

            #region "Ejecución manual"
            if (KeyPressed == ConsoleKey.C)
            {
                Console.Clear();
                Console.WriteLine("Comenzando...");

                Performance.Contadores.Instalar();
                Diagnostics.EventLog.Instalar();

                Console.WriteLine(string.Empty);
                Console.WriteLine("Ejecutando todas las tareas de este motor");
                Console.WriteLine("Para detener las tareas en cualquier momento presione 'Escape'");


                Principal Principal = new Principal();
                Principal.Iniciar();
                Principal.Monitorear();

                while (true)
                {
                    KeyPressed = EsperarTecla();

                    if (KeyPressed == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        Console.WriteLine("***************************************************");
                        Console.WriteLine("Deteniendo las tareas del motor. Espere un momento.");
                        Console.WriteLine("***************************************************");

                        Principal.Detener();
                        return;
                    }
                }
            }
            #endregion

            #region "Instalación"

            if (KeyPressed == ConsoleKey.I)
            {
                if (!EsAdministrador())
                {
                    Console.Clear();
                    Console.WriteLine("ATENCION: No puede continuar porque no es administrador.");
                    Console.WriteLine("Presione cualquier tecla para salir.");
                    EsperarTecla();

                    return;
                }

                if (EstaInstalado())
                {
                    Console.Clear();
                    Console.WriteLine(string.Format("ATENCION: El servicio '{0}' ya esta instalado. Por tanto no hace falta volver a instalar.", SX.Core.MotorBase.Configuracion.Administrador.IdentificacionServicio.NombreUnicoVisible));
                    Console.WriteLine("Presione cualquier tecla para salir.");
                    EsperarTecla();

                    return;
                }

                Console.Clear();
                ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                KeyPressed = EsperarTecla();
            }

            #endregion

            #region "Desinstalación"
            if (KeyPressed == ConsoleKey.U)
            {
                if (!EsAdministrador())
                {
                    Console.Clear();
                    Console.WriteLine("ATENCION: No puede continuar porque no es administrador");
                    Console.WriteLine("Presione cualquier tecla para salir.");
                    EsperarTecla();

                    return;
                }

                if (!EstaInstalado())
                {
                    Console.Clear();
                    Console.WriteLine(string.Format("ATENCION: El servicio '{0}' no esta instalado. Por tanto no hace falta desinstalar.", SX.Core.MotorBase.Configuracion.Administrador.IdentificacionServicio.NombreUnicoVisible));
                    Console.WriteLine("Presione cualquier tecla para salir.");
                    EsperarTecla();

                    return;
                }

                Console.Clear();
                ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                EsperarTecla();
            }

            #endregion

            #endregion

        } //static void Main()

        #region "Helper"
        static bool EsAdministrador()
        {
            WindowsIdentity User = WindowsIdentity.GetCurrent();
            WindowsPrincipal Principal = new WindowsPrincipal(User);
            return Principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static bool EstaInstalado()
        {
            ServiceController ServiceController = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName.Equals(SX.Core.MotorBase.Configuracion.Administrador.IdentificacionServicio.NombreUnicoInterno, StringComparison.OrdinalIgnoreCase));

            bool Instalado = false;
            if (ServiceController != null)
                Instalado = true;

            return Instalado;
        }

        static ConsoleKey EsperarTecla()
        {
            ConsoleKeyInfo ConsoleKeyInfo = Console.ReadKey(true);
            return ConsoleKeyInfo.Key;
        }
        #endregion

    } //static class Program
}
