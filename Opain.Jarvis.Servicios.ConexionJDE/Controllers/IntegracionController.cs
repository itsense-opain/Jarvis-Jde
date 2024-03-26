using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Opain.Jarvis.Infraestructura.Datos.Entidades;
using Opain.Jarvis.Infraestructura.Datos.EntidadesIntegracion;
using System.Data;
using System.Globalization;
using Newtonsoft.Json;
using System.IO;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.Extensions.Logging;
using Opain.Jarvis.Infraestructura.Datos.Entidades.Informes;

namespace Opain.Jarvis.Servicios.ConexionJDE.Controllers
{
    [Route("api/[controller]/[action]")]
    public class IntegracionController : Controller
    {
        private readonly Repositorio.EjecutorIntegracion Integracion;
        private readonly IConfiguration _configuration;
      

        public IntegracionController(Repositorio.EjecutorIntegracion integracion, IConfiguration configuration)
        {
            this.Integracion = integracion;
            _configuration = configuration;
         
        }

        //[HttpGet]
        //public async Task<IActionResult> InsertarDatosOracle(string nombreAerolinea, int cantidadPasajeros, DateTime fechaCarga)
        //{
        //    if (string.IsNullOrEmpty(nombreAerolinea))
        //        return BadRequest();

        //    await this.Integracion.InsertarDatosJarvis(nombreAerolinea, "", "");
        //    List<object> Resultado = new List<object>();
        //    return Ok(Resultado);
        //}
        #region "Metodos para extraer informacion de Oracle hacia Jarvis"
        [HttpGet]
        public async Task<ActionResult<IList<AerolineaInforme>>> TraerAerolineasJDE()
        {
            List<AerolineaInforme> resultado;
            Repositorio.RespuestaGenerica Respuesta = await this.Integracion.TraerAerolineasJDE();

            if (!Respuesta.Exito)
            {
                this.Integracion.SendCorreo(Respuesta.Exito, "TraerAerolineasJDE");
                return NotFound();
            }
            else
            {
                this.Integracion.SendCorreo(Respuesta.Exito, "TraerAerolineasJDE");
            }
            resultado = Respuesta.Resultado<AerolineaInforme>();

            // Debo insertarlo en mis tablas Mysql
            if (resultado.Count > 0)
            {
                int res = await this.Integracion.GrabarAerolineas(resultado);
            }
            return Ok(resultado);
        }

        /// <summary>
        /// Traer vuelos de JDE
        /// </summary>
        /// <param name="fechaIntegracion">Formato (dd/MM/yyyy)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IList<DatosVuelo>>> TraerVuelos(string fechaIntegracion = null)
        {
            DateTime fechaBusqueda = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaIntegracion))
            {
                fechaBusqueda = DateTime.ParseExact(fechaIntegracion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            DateTime FechaAnterior = fechaBusqueda.AddDays(-1);

            // traer iddaily de vuelos pendientes del dia anterior, 
            List<string> datosIddaily;
            datosIddaily = await this.Integracion.Traeriddailypendiente();

            string respuestaeliminacion = await this.Integracion.EliminaridDailyProcesados();

            List<DatosVuelo> resultado;
            Repositorio.RespuestaGenerica Respuesta = await this.Integracion.TraerdatosVuelos(fechaBusqueda, FechaAnterior, datosIddaily);

            if (!Respuesta.Exito)
            {
                this.Integracion.SendCorreo(Respuesta.Exito, "TraerVuelos");
                return NotFound();
            }

            this.Integracion.SendCorreo(Respuesta.Exito, "TraerVuelos");
            resultado = Respuesta.Resultado<DatosVuelo>();

            // Traigo los Iddaily que estén pendientes
            List<DatosVuelo> resultadoPendiente;
            Repositorio.RespuestaGenerica RespuestaPendiente = await this.Integracion.TraerdatosVuelosPendientes(fechaBusqueda, FechaAnterior);
            if (!RespuestaPendiente.Exito)
            {
                resultadoPendiente = null;
            }
            else { resultadoPendiente = RespuestaPendiente.Resultado<DatosVuelo>(); }

            
            // Inserto en Operaciones Vuelo
            if (resultado.Count >0 )
            { 
                int res = await this.Integracion.GrabarVuelos(resultado);
                 
            }
            if (resultadoPendiente.Count >0 && resultadoPendiente != null)
            {
                // saco los iddayly pendientes y los grabo en una tabla
                int res = await this.Integracion.GrabarVuelosPendientes(resultadoPendiente);
            }
            return Ok(resultado);
        }

        [HttpGet]
        public async Task<ActionResult<IList<DatosCiudades>>> TraerCiudades()
        {
            List<DatosCiudades> resultado;
            Repositorio.RespuestaGenerica Respuesta = await this.Integracion.TraerdatosCiudades();
            if (!Respuesta.Exito)
            {
                this.Integracion.SendCorreo(Respuesta.Exito, "TraerCiudades");
                return NotFound();
            }
            else
            {
                this.Integracion.SendCorreo(Respuesta.Exito, "TraerCiudades");
            }
            resultado = Respuesta.Resultado<DatosCiudades>();
            if (resultado.Count > 0)
            {
                int res = await this.Integracion.GrabarCiudades(resultado);
            }
            return Ok(resultado);
        }
        #endregion

        #region "Metodos de actualizacion a la BD oracle"
        [HttpGet]
        public async Task<ActionResult<int>> ActualizarVuelosValidados()
        {

            int res = await this.Integracion.ActualizarVuelosValidados();

            if (res == 1)
            {
                this.Integracion.SendCorreo(true, "ActualizarVuelosValidados");
            }
            else
            {
                this.Integracion.SendCorreo(false, "ActualizarVuelosValidados");
            }

            return Ok(res);
        }

        [HttpGet]
        public async Task<ActionResult<int>> ActualizarVueloValidado( string vuelo)
        {

            int res = await this.Integracion.ActualizarVueloValidado(vuelo);

        

            return Ok(res);
        }

        #endregion

        #region Metodos Privados

        private async void GuardarLogVuelos(Repositorio.RespuestaGenerica respuestaGenerica)
        {
            try
            {
                string json = JsonConvert.SerializeObject(respuestaGenerica);
                string fileName = _configuration.GetSection("LogConsulta:Ubicacion").Value;
                if (!System.IO.File.Exists(fileName))
                {
                    FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                    using (StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        writer.WriteLine(DateTime.Now.ToString() + " " + json + Environment.NewLine);
                    }
                }
                else
                {
                    StreamWriter fichero;
                    fichero = System.IO.File.AppendText(fileName);
                    fichero.WriteLine(DateTime.Now.ToString() + " " + json + Environment.NewLine);
                    fichero.Close();
                }
            }
            catch (Exception ex)
            {                
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));                
            }

        }

        #endregion


    }
}