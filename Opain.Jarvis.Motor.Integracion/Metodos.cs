using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Opain.Jarvis.Motor.Integracion
{
    public class Metodos
    {
       
        public async Task<int> ActualizarAerolineas()
        {
            
            ServicioApi servicio = new ServicioApi();
            //Logica
            IList<Aerolinea> respuesta = await servicio.GetAsync<IList<Aerolinea>>("api/Integracion/TraerAerolineasJDE");
            //conectarme a jarvis para preguntarle
            return respuesta.Count();
        }

        public async Task<int> ActualizarCiudades()
        {
            ServicioApi servicio = new ServicioApi();
            //Logica
            IList<DatosCiudades> respuesta = await servicio.GetAsync<IList<DatosCiudades>>("api/Integracion/TraerCiudades");
            //conectarme a jarvis para preguntarle
            return respuesta.Count();
            
        }

        public async Task<int> ActualizarOperacionesVuelos()
        {
            try
            {
                ServicioApi servicio = new ServicioApi();
                //Logica
                IList<DatosVuelo> respuesta = await servicio.GetAsync<IList<DatosVuelo>>("api/Integracion/TraerVuelos");
                //conectarme a jarvis para preguntarle
                return respuesta.Count();
            }
            catch (Exception ex)
            {
                Log("Error consumo de servicio api/Integracion/TraerVuelos mensaje: " + ex.Message + " completo: " + ex);
                return 0;
            }
          
        }

        public async Task<int> ActualizarVuelosValidados()
        {
            try
            {
                ServicioApi servicio = new ServicioApi();
                //Logica
                int respuesta = await servicio.GetAsync<int>("api/Integracion/ActualizarVuelosValidados");
                return respuesta;
            }
            catch (Exception)
            {
                return 0;
            }
          
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

    }
}
