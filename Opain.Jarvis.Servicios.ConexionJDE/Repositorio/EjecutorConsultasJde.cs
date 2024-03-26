using Dapper;
using Microsoft.Extensions.Configuration;
using Opain.Jarvis.Infraestructura.Datos.Entidades;
using Opain.Jarvis.Infraestructura.Datos.Entidades.Informes;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Repositorio
{
    public class EjecutorConsultasJde
    {
        #region Generico
        private readonly IConfiguration _config;
        public Exception Error { get; set; }

        public EjecutorConsultasJde(IConfiguration config)
        {
            this._config = config;
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

        public async Task<RespuestaGenerica> TraerExentos(string numVuelo,DateTime fecha)
        {
            string consulta = Query.TraerExentos.Consulta(numVuelo,fecha,this._config);
            return await Ejecutar<AerolineaInforme>(consulta);
        }
     

    }
}
