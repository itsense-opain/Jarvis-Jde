using System;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace Opain.Jarvis.Motor.Integracion
{
    public partial class Service1 : ServiceBase
    {
        Timer timerOut = null;
        Timer timerIn = null;
        DateTime? Ultima_Actualizacion;
        int ActAerolineas = 0;
        int ActCiudades = 0;
        int hora;
        int intervaloAct;
        public Service1()
        {
            InitializeComponent();
            // Para el debug , esto debe comentariarse
            
            Ultima_Actualizacion = null;
            hora = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["HoraEjecucion"].ToString());
            intervaloAct = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IntervaloActualizacion"].ToString());

            ActAerolineas = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ActualizaAerolinea"].ToString());
            ActCiudades = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ActualizaCiudades"].ToString());

            // Configuro el timerOut que es el que trae la info de opain y la inserta en nuestras tablas
            //timerOut = new Timer(10000);
            timerOut = new Timer(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timerOut"].ToString()));
            timerOut.Elapsed += new ElapsedEventHandler(timerOut_Elapsed);
            // Configuro el timerOut que es el que trae la info de opain y la inserta en nuestras tablas

            timerIn = new Timer(intervaloAct);
            timerIn.Elapsed += new ElapsedEventHandler(timerIn_Elapsed);
        }

        void timerOut_Elapsed(object sender, ElapsedEventArgs e)
        {
            timerOut.Enabled = false;
            
            if (Ultima_Actualizacion == null && DateTime.Now.Hour == Convert.ToInt32(hora))
            {
                Log("Va a iniciar la integración");
                EjecutarOperacionesOut();
            }
            else
            {
                if (Convert.ToDateTime(Ultima_Actualizacion).Day != DateTime.Now.Day && DateTime.Now.Hour == Convert.ToInt32(hora))
                {
                    EjecutarOperacionesOut();
                }
            }

            timerOut.Enabled = true;
        }
        private void EjecutarOperacionesOut()
        {
            // Debe ejecutar todo
            if (ActAerolineas == 0)
            {
                Log("Ejecuta integracion de aerolineas");
                ejecutarAerolineas();
                ActAerolineas = 1;
            }
            if (ActCiudades == 0)
            {
                Log("Ejecuta integracion de ciudades");
                ActualizarCiudades();
                ActCiudades = 1;
            }
            Log("Trae vuelos de JDE");
            ActualizarOperaciones();
            Ultima_Actualizacion = DateTime.Now;
        }

        void timerIn_Elapsed(object sender, ElapsedEventArgs e)
        {
            timerIn.Enabled = false;
            // Traigo informacion validada 
            Log("Actualiza los vuelos a JDE");
            ActualizarVuelosValidados();
            timerIn.Enabled = true;
        }


        public async Task<int> ejecutarAerolineas()
        {
            Metodos metodos = new Metodos();
            int aerolineasActualizadas = await metodos.ActualizarAerolineas();
            return aerolineasActualizadas;
        }
        public async Task<int> ActualizarCiudades()
        {
            Metodos metodos = new Metodos();
            int aerolineasActualizadas = await metodos.ActualizarCiudades();

            return aerolineasActualizadas;
        }
        public async Task<int> ActualizarOperaciones()
        {
            try
            {
                Metodos metodos = new Metodos();
                int vuelosAct = 0;

                while (vuelosAct == 0)
                {
                    vuelosAct = await metodos.ActualizarOperacionesVuelos();
                }
                
                Log("Trajo " + vuelosAct + " registros");
                return vuelosAct;
            }
            catch (Exception ex)
            {
                Log("Error actualoizar operaciones: " + ex.Message + " completo: " + ex);
                throw;
            }
        }
        public async Task<int> ActualizarVuelosValidados()
        {
            Metodos metodos = new Metodos();
            int resultado = await metodos.ActualizarVuelosValidados();
            return resultado;
        }

        public static void Log(string texto)
        {
            try
            {
                FileStream stram = null;
                string fileName = System.Configuration.ConfigurationManager.AppSettings["RutaLogs"].ToString() + "\\LogIntegracion.txt";
                if (!File.Exists(fileName))
                {
                    stram = new FileStream(fileName, FileMode.OpenOrCreate);
                    using (StreamWriter writer = new StreamWriter(stram, Encoding.UTF8))
                    {
                        writer.WriteLine(DateTime.Now.ToString() + " " + texto);
                    }
                }
                else
                {
                    StreamWriter fichero;
                    fichero = File.AppendText(fileName);
                    fichero.WriteLine(DateTime.Now.ToString() + " " + texto);
                    fichero.Close();
                }
            }
            catch (Exception)
            {
            }
        }
        protected override void OnStart(string[] args)
        {
            timerOut.Start();
            timerIn.Start();
        }

        protected override void OnStop()
        {
            timerOut.Stop();
            timerIn.Stop();
        }
    }
}
