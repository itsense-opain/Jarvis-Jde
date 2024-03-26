using Dapper;
using Microsoft.Extensions.Configuration;
using Opain.Jarvis.Infraestructura.Datos.Entidades;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Opain.Jarvis.Servicios.ConexionJDE.Controllers;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Opain.Jarvis.Infraestructura.Datos.Entidades.Informes;

namespace Opain.Jarvis.Servicios.ConexionJDE.Repositorio
{
    public class EjecutorInforme
    {
        #region Generico
        private readonly IConfiguration _config;
        private readonly IEmailSender emailSender;
        private readonly ILogger<EjecutorInforme> _logger;

        public Exception Error { get; set; }

        public EjecutorInforme(IConfiguration config, IEmailSender email, ILogger<EjecutorInforme> logger)
        {
            this._config = config;
            emailSender = email;
            _logger = logger;
        }

        private IDbConnection Conexion
        {
            get
            {
                return new OracleConnection(_config.GetSection("ConnectionStrings").GetSection("JDEConnection").Value);
            }
        }

        private async Task<List<T>> EjecutarQuery<T>(string consulta)
        {
            this.Error = null;
            try
            {
                using (IDbConnection conn = Conexion)
                {
                    conn.Open();

                    if (conn.State == ConnectionState.Open)
                    {
                        var respuesta = await SqlMapper.QueryAsync<T>(conn, consulta, commandType: CommandType.Text);
                        return respuesta.ToList();
                    }
                    conn.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                //Trace Log ??
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                this.Error = ex;
                return null;
            }
        }

        private async Task<RespuestaGenerica> EjecutarRespuesta(object Resultado)
        {
            RespuestaGenerica Respuesta;

            if (Resultado != null)
                Respuesta = new RespuestaGenerica(Resultado);
            else
                Respuesta = new RespuestaGenerica(Error.Message);

            return Respuesta;
        }

        private async Task<RespuestaGenerica> Ejecutar<T>(string consulta)
        {
            List<T> Resultado = await this.EjecutarQuery<T>(consulta);
            return await EjecutarRespuesta(Resultado);
        }
        #endregion

        public async Task<RespuestaGenerica> TraerAerolineas()
        {
            string consulta = Query.TraerAerolineas.Consulta(this._config);
            return await Ejecutar<AerolineaInforme>(consulta);
        }
        public async Task<RespuestaGenerica> TraerAerolineas2()
        {
            string consulta = Query.TraerAerolineas.Consulta2(this._config);
            return await Ejecutar<AerolineaInforme>(consulta);
        }
        public async Task<RespuestaGenerica> Informe1(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe1.Consulta(filtro,this._config);
            return await Ejecutar<Anexo1>(consulta);
        }

        public async Task<RespuestaGenerica> Informe2(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe2.Consulta(filtro, this._config);
            return await Ejecutar<Anexo2>(consulta);
        }

        public async Task<RespuestaGenerica> Informe3(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe3.Consulta(filtro, this._config);
            return await Ejecutar<Anexo3>(consulta);
        }

        public async Task<RespuestaGenerica> Informe4(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe4.Consulta(filtro, this._config);
            return await Ejecutar<Anexo4>(consulta);
        }

        public async Task<RespuestaGenerica> Informe5(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe5.Consulta(filtro, this._config);
            return await Ejecutar<Anexo5>(consulta);
        }

        public async Task<RespuestaGenerica> Informe6(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe6.Consulta(filtro,this._config);
            return await Ejecutar<Anexo6>(consulta);
        }

        public async Task<RespuestaGenerica> Informe7A()
        {
            string consulta = Query.Informe7A.Consulta(this._config);
            return await Ejecutar<Anexo7A>(consulta);
        }

        public async Task<RespuestaGenerica> Informe7B(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe7B.Consulta(filtro, this._config);
            return await Ejecutar<Anexo7B>(consulta);
        }

        public async Task<RespuestaGenerica> Informe8(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe8.Consulta(filtro, this._config);
            return await Ejecutar<Anexo8>(consulta);
        }

        public async Task<RespuestaGenerica> Informe9(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe9.Consulta(filtro, this._config);
            return await Ejecutar<Anexo9>(consulta);
        }

        public async Task<RespuestaGenerica> Informe10(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe10.Consulta(filtro,this._config);
            return await Ejecutar<Anexo10>(consulta);
        }

        #region Generico No Trabajados
        public async Task<RespuestaGenerica> Informe11(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe11.Consulta(filtro,this._config);
            return await Ejecutar<Anexo11>(consulta);
        }

        public async Task<RespuestaGenerica> Informe12(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe12.Consulta(filtro,this._config);
            return await Ejecutar<Anexo12>(consulta);
        }

        public async Task<RespuestaGenerica> Informe13(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe13.Consulta(filtro, this._config);
            return await Ejecutar<Anexo13>(consulta);
        }

        public async Task<RespuestaGenerica> Informe14(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe14.Consulta(filtro, this._config);
            return await Ejecutar<Anexo14>(consulta);
        }

        public async Task<RespuestaGenerica> Informe15(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe15.Consulta(filtro, this._config);
            return await Ejecutar<Anexo15>(consulta);
        }

        public async Task<RespuestaGenerica> Informe16(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe16.Consulta(filtro, this._config);
            return await Ejecutar<Anexo16>(consulta);
        }

        public async Task<RespuestaGenerica> Informe17(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe17.Consulta(filtro, this._config);
            return await Ejecutar<Anexo17>(consulta);
        }
        #endregion

        public async Task<RespuestaGenerica> Informe19(FiltroBusqueda filtro)
        {
            string consulta = Query.Informe19.Consulta(filtro, this._config);
            return await Ejecutar<Anexo19>(consulta);
        }

        public async Task<RespuestaGenerica> TraerExentos(string numVuelo, DateTime fecha)
        {
            string consulta = Query.TraerExentos.Consulta(numVuelo, fecha,this._config);
            return await Ejecutar<Opain.Jarvis.Infraestructura.Datos.EntidadesIntegracion.DatosExentos>(consulta);
        }

        public async Task<bool> TraerAeroJDE()
        {
            EjecutorIntegracion ejecutor = new EjecutorIntegracion(this._config, emailSender, _logger);

            IntegracionController objIntegracion = new IntegracionController(ejecutor, this._config);

            var Respuesta = await objIntegracion.TraerAerolineasJDE();

            if (((Microsoft.AspNetCore.Mvc.ObjectResult)Respuesta.Result).StatusCode.ToString() != "200")
                return false;

            return true;
        }

        public async Task<bool> TraerVuelosJDE()
        {
            EjecutorIntegracion ejecutor = new EjecutorIntegracion(this._config, emailSender, _logger);

            IntegracionController objIntegracion = new IntegracionController(ejecutor, this._config);

            var Respuesta = await objIntegracion.TraerVuelos();

            if (((Microsoft.AspNetCore.Mvc.ObjectResult)Respuesta.Result).StatusCode.ToString() != "200")
                return false;

            return true;
        }

        public async Task<bool> TraerCiudadesJDE()
        {
            EjecutorIntegracion ejecutor = new EjecutorIntegracion(this._config, emailSender, _logger);

            IntegracionController objIntegracion = new IntegracionController(ejecutor, this._config);

            var Respuesta = await objIntegracion.TraerCiudades();

            if (((Microsoft.AspNetCore.Mvc.ObjectResult)Respuesta.Result).StatusCode.ToString() != "200")
                return false;

            return true;
        }

        public async Task<bool> UpdVueloValidJDE()
        {
            EjecutorIntegracion ejecutor = new EjecutorIntegracion(this._config, emailSender, _logger);

            IntegracionController objIntegracion = new IntegracionController(ejecutor, this._config);

            var Respuesta = await objIntegracion.ActualizarVuelosValidados();

            if (((Microsoft.AspNetCore.Mvc.ObjectResult)Respuesta.Result).StatusCode.ToString() != "200")
                return false;

            return true;
        }

        public async Task<bool> UpdVueloValidJDEVuelo(string vuelo)
        {
            EjecutorIntegracion ejecutor = new EjecutorIntegracion(this._config, emailSender, _logger);

            IntegracionController objIntegracion = new IntegracionController(ejecutor, this._config);

            var Respuesta = await objIntegracion.ActualizarVueloValidado(vuelo);

            if (((Microsoft.AspNetCore.Mvc.ObjectResult)Respuesta.Result).StatusCode.ToString() != "200")
                return false;

            return true;
        }
    }
}
