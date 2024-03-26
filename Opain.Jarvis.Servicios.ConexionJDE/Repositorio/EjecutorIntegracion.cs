using Dapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos.Entidades;
using Opain.Jarvis.Infraestructura.Datos.Entidades.Informes;
using Opain.Jarvis.Infraestructura.Datos.EntidadesIntegracion;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;

namespace Opain.Jarvis.Servicios.ConexionJDE.Repositorio
{
    public class EjecutorIntegracion
    {
        private readonly IConfiguration _config;
        private readonly Store.Helper.Ejecutor Ejecutor;
        private readonly IEmailSender emailSender;
        private IConfiguration config;
        private readonly ILogger _logger;

        public EjecutorIntegracion(IConfiguration config, IEmailSender email, ILogger<EjecutorInforme> logger)
        {
            this._config = config;
            emailSender = email;
            _logger = logger;

        }

        public Exception Error { get; set; }

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
                    try
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            var respuesta = await SqlMapper.QueryAsync<T>(conn, consulta, commandType: CommandType.Text);
                            conn.Close();
                            return respuesta.ToList();
                        }
                        conn.Close();
                        return null;
                    }
                    catch
                    {
                        conn.Close();
                        return null;
                    }
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

        public void SendCorreo(bool resultado, string operacion)
        {
            var emailUser = _config.GetSection("Config").GetSection("mailIntegracion").Value;
            //string correo = "";
            if (resultado)
            {
                try
                {
                    //emailSender.SendEmailAsync()
                    emailSender.SendEmailAsync(
                    emailUser,
                    "Notificación de Integracion " + operacion,
                    "Llamado exito a metodo: " + operacion);
                    // correo = "";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                    this.Error = ex;
                }

            }
            else
            {
                try
                {
                    //correo = "";
                    
                    emailSender.SendEmailAsync(
                    emailUser,
                    "Notificación de Integracion " + operacion,
                    "Llamado fallido a metodo: " + operacion);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                    this.Error = ex;
                }

            }
        }

        private async Task<RespuestaGenerica> Ejecutar<T>(string consulta)
        {
            List<T> Resultado = await this.EjecutarQuery<T>(consulta);
            return await EjecutarRespuesta(Resultado);
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
        private async Task EjecutarQuery(string consulta)
        {
            this.Error = null;
            try
            {
                int totalTransitosunsertados = 0;
                using (IDbConnection conn = Conexion)
                {
                    conn.Open();
                    IDbCommand command = conn.CreateCommand();
                    var oTransaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    // Assign transaction object for a pending local transaction
                    command.Transaction = oTransaction;
                    try
                    {
                        command.CommandText = consulta;
                        command.ExecuteNonQuery();
                        oTransaction.Commit();
                        totalTransitosunsertados = totalTransitosunsertados + 1;
                        _logger.LogInformation($"/*********/transitos insertados correctos/*********/ : {totalTransitosunsertados} ");
                    }
                    catch (Exception e)
                    {
                        _logger.LogInformation("ERRR. al insertar transitos1 : " + e.Message);
                        oTransaction.Rollback();

                    }
                    finally
                    {
                        conn.Close();

                    }
                   

                   
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ERROR. al insertar transitos : " + ex.Message);
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));



                this.Error = ex;
            }
                   }

        public async Task InsertarDatosJarvis(string parametro1, string parametro2, string etc)
        {
            string Query = string.Format("INSERT TABLAJARVIS INTO (campo1, campor2, campo3) VALUES('{0}', '{1}')", parametro1, parametro2);
            await EjecutarQuery(Query);
        }
        public async Task<RespuestaGenerica> TraerAerolineasJDE()
        {
            string consulta = Query.TraerAerolineas.Consulta2(this._config);
            return await Ejecutar<Aerolinea>(consulta);
        }

        public async Task<RespuestaGenerica> TraerdatosVuelos(DateTime FechaBusqueda, DateTime FechaAnterior, List<string> iddailys)
        {
            _logger.LogInformation("2. TraerdatosVuelos "  );
            string consulta = Query.TraeVuelos.ConsultaJDE(FechaBusqueda, FechaAnterior, iddailys, this._config);
            _logger.LogInformation("3. consulta a ejecutar: " + consulta);
            return await Ejecutar<DatosVuelo>(consulta);

        }
        public async Task<List<string>> Traeriddailypendiente()
        {
            _logger.LogInformation("1. Traer IdDailypendiente");
            List<string> listaiddaily = new List<string>();
            Store.Helper.Ejecutor ejecutor = new Store.Helper.Ejecutor(this._config);
            DataTable resultado = ejecutor.Conexion("TraerVuelosPendientes");
            for (int i = 0; i < resultado.Rows.Count; i++)
            {
                listaiddaily.Add(resultado.Rows[i]["IdDaily"].ToString());
            }
            return listaiddaily;
        }

        public async Task<string> EliminaridDailyProcesados()
        {
            _logger.LogInformation("Elimina los ya procesados");
         
            Store.Helper.Ejecutor ejecutor = new Store.Helper.Ejecutor(this._config);
            DataTable resultado = ejecutor.Conexion("EliminaVuelosProcesados");
           
            return "ok";
        }

        public async Task<RespuestaGenerica> TraerdatosVuelosPendientes(DateTime FechaBusqueda, DateTime FechaAnterior)
        {
            _logger.LogInformation("2. TraerdatosVuelosPendientes");
            string consulta = Query.TraeVuelos.ConsultaJDEPendientes(FechaBusqueda, FechaAnterior, this._config);
            _logger.LogInformation("3. consulta a ejecutar: " +  consulta);
            return await Ejecutar<DatosVuelo>(consulta);
        }
        public async Task<RespuestaGenerica> TraerdatosCiudades()
        {
            string consulta = Query.TraeCiudades.Consulta(this._config);
            return await Ejecutar<DatosCiudades>(consulta);
        }

        public async Task<int> GrabarAerolineas(List<AerolineaInforme> lstAerolineas)
        {

            foreach (var item in lstAerolineas)
            {
                try
                {
                    Store.Helper.Ejecutor ejecutor = new Store.Helper.Ejecutor(this._config);
                    ejecutor.AgregarCampoIn("_Nombre", (item.Texto == null ? "" : item.Texto));
                    ejecutor.AgregarCampoIn("_Codigo", (item.Valor == null ? "" : item.Valor));
                    ejecutor.AgregarCampoIn("_Sigla", (item.Sigla == null ? "" : item.Sigla));
                    int resultado = ejecutor.ConexionEx("Insertar_Aerolineas");
                    _logger.LogInformation("Actualización exitosa aerolinea : " + item.Sigla + " " + item.Valor + " " + item.Texto);
                }
                catch(Exception Exception)
                {
                    _logger.LogInformation("Actualización fallida aerolinea : " + item.Sigla + " " + item.Valor + " " + item.Texto + " " + Exception.Message);
                }
            }
            return 1;
        }

        public async Task<int> ActualizarVuelosValidados()
        {

            int resReturn = 1;

            _logger.LogInformation("1. Inicio Integracion");

            _logger.LogInformation("1. Iniciar traer vuelos validados");
            Store.Helper.Ejecutor ejecutor = new Store.Helper.Ejecutor(this._config);
            DataTable resultado = ejecutor.Conexion("TraerVuelosValidados");
            _logger.LogInformation("1. fin traer vuelos validados");
            _logger.LogInformation("1. total vuelos validados" + resultado.Rows.Count);
            //VuelosValidados vuelosValidados22 = new VuelosValidados();
            //string consultaxxx = Query.ActualizaTotales.ActualizaTotalesJDE(vuelosValidados22);
            // Actualizo la tabla de "Pasajeros"
            // Luego de que traigo los resultados inserto en la tabla de oracle de "pasajeros"
            // Claves que forman la llave primaria
            _logger.LogInformation("1. inicia for vuelos validados");
            for (int i = 0; i < resultado.Rows.Count; i++)
            {
                _logger.LogInformation("2. Encontro vuelos a enviar a JDE");
                try
                {
                    // preparacion de data previa para evitar break en la logica relacionada....
                    //resultado.Rows[i]["FechaVuelo"].ToString()
                    if (!DateTime.TryParse(resultado.Rows[i]["FechaVuelo"].ToString(), out DateTime fchVuelo))
                    {
                        fchVuelo = DateTime.Now;
                    }
                    //resultado.Rows[i]["Adultos"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["Adultos"].ToString(), out int Adultos))
                    {
                        Adultos = 0;
                    }
                    //resultado.Rows[i]["Infantes"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["Infantes"].ToString(), out int Infantes))
                    {
                        Infantes = 0;
                    }
                    //resultado.Rows[i]["PagoCOP"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["PagoCOP"].ToString(), out int PagoCOP))
                    {
                        PagoCOP = 0;
                    }
                    //resultado.Rows[i]["PagoUSD"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["PagoUSD"].ToString(), out int PagoUSD))
                    {
                        PagoUSD = 0;
                    }
                    //resultado.Rows[i]["TotalConexion"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["TotalConexion"].ToString(), out int TotalConexion))
                    {
                        TotalConexion = 0;
                    }
                    //resultado.Rows[i]["TotalLinea"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["TotalLinea"].ToString(), out int TotalLinea))
                    {
                        TotalLinea = 0;
                    }
                    //resultado.Rows[i]["Tripulacion"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["Tripulacion"].ToString(), out int Tripulacion))
                    {
                        Tripulacion = 0;
                    }
                    //resultado.Rows[i]["TotalEmbarcados"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["TotalEmbarcados"].ToString(), out int TotalEmbarcados))
                    {
                        TotalEmbarcados = 0;
                    }
                    //resultado.Rows[i]["IdOperacionVuelo"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["IdOperacionVuelo"].ToString(), out int IdOperacionVuelo))
                    {
                        IdOperacionVuelo = 0;
                    }

                    _logger.LogInformation("2. informarcion vuelos a enviar a JDE");
                    // nueva instancia de vuelos validados
                    VuelosValidados vuelosValidados = new VuelosValidados();
                    // seteo de params relacionados
                    vuelosValidados.HoraVuelo = (resultado.Rows[i]["HoraVuelo"] == null ? "" : resultado.Rows[i]["HoraVuelo"].ToString());
                    vuelosValidados.FechaVuelo = Convert.ToDateTime(fchVuelo);
                    vuelosValidados.NumeroVuelo = (resultado.Rows[i]["NumeroVuelo"] == null ? "" : resultado.Rows[i]["NumeroVuelo"].ToString());
                    vuelosValidados.Aerolinea = (resultado.Rows[i]["Aerolina"] == null ? "" : resultado.Rows[i]["Aerolina"].ToString());
                    vuelosValidados.Matricula = (resultado.Rows[i]["Matricula"] == null ? "" : resultado.Rows[i]["Matricula"].ToString());
                    vuelosValidados.Adultos = Convert.ToInt32(Adultos);
                    vuelosValidados.Infantes = Convert.ToInt32(Infantes);
                    vuelosValidados.PagoCOP = Convert.ToInt32(PagoCOP);
                    vuelosValidados.PagoUSD = Convert.ToInt32(PagoUSD);
                    vuelosValidados.TotalConexion = Convert.ToInt32(TotalConexion);
                    vuelosValidados.TotalLinea = Convert.ToInt32(TotalLinea);
                    vuelosValidados.Tripulacion = Convert.ToInt32(Tripulacion);
                    vuelosValidados.TotalEmbarcados = Convert.ToInt32(TotalEmbarcados);
                    vuelosValidados.TipoVuelo = (resultado.Rows[i]["TipoVuelo"] == null ? "" : resultado.Rows[i]["TipoVuelo"].ToString());
                    vuelosValidados.IdOperacionVuelo = Convert.ToInt32(IdOperacionVuelo);
                    vuelosValidados.Id_daily = (resultado.Rows[i]["IdDaily"] == null ? "-1" : resultado.Rows[i]["IdDaily"].ToString());

                    _logger.LogInformation("2. fin informacion vuelos a enviar a JDE");
                    // selecciono los totales actuales
                    string consulta2 = Query.ActualizaTotales.ConsultaSelect(vuelosValidados, this._config);

                    //consulta vuelos validados
                    _logger.LogInformation($"3. Vuelos validados: {consulta2} ");

                    var respuesta2 = await Ejecutar<VuelosValidados>(consulta2);
                    _logger.LogInformation("2. Antes de actualizar totales en JDE" + consulta2);

                    // Actualizo los totales en pasajeros
                    string consulta = Query.ActualizaTotales.ActualizaTotalesJDE(vuelosValidados,this._config);
                    _logger.LogInformation($"4 Consulta actualizar totales JDE: {consulta} ");

                    await EjecutarQuery(consulta);

                    // Actualizo totales actuales en auditoria

                    List<VuelosValidados> datosdaylyypax;

                    datosdaylyypax = respuesta2.Resultado<VuelosValidados>();

                    try
                    {
                        if (datosdaylyypax.Count > 0)
                        {
                            if (datosdaylyypax[0] != null)
                            {
                                _logger.LogInformation("5. Inserta Auditoria");
                                string consultaAudit;
                                DatosAuditoria datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 1000;
                                datosAuditoria.Campo_Modificado = "PEQ70TTA";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].Adultos.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.Adultos.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria,this._config);
                                await EjecutarQuery(consultaAudit);

                                datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 2000;
                                datosAuditoria.Campo_Modificado = "PEQ70TTI";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].Infantes.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.Infantes.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria, this._config);
                                await EjecutarQuery(consultaAudit);

                                datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 3000;
                                datosAuditoria.Campo_Modificado = "PEQ70EC";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].Tripulacion.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.Tripulacion.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria, this._config);
                                await EjecutarQuery(consultaAudit);

                                datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 4000;
                                datosAuditoria.Campo_Modificado = "PEQ70TTC";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].PagoCOP.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.PagoCOP.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria, this._config);
                                await EjecutarQuery(consultaAudit);

                                datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 5000;
                                datosAuditoria.Campo_Modificado = "PEQ70TTU";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].PagoUSD.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.PagoUSD.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria, this._config);
                                await EjecutarQuery(consultaAudit);

                                datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 6000;
                                datosAuditoria.Campo_Modificado = "PEQ70TRP";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].TotalLinea.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.TotalLinea.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria, this._config);
                                await EjecutarQuery(consultaAudit);

                                datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 7000;
                                datosAuditoria.Campo_Modificado = "PEQ70TRF";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].TotalConexion.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.TotalConexion.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria, this._config);
                                await EjecutarQuery(consultaAudit);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Trace Log ??
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));

                        _logger.LogInformation($" 1. Error Auditoria: {Newtonsoft.Json.JsonConvert.SerializeObject(ex)} ");
                        this.Error = ex;
                        resReturn = 0;
                    }

                    // segmento para actualizar exentos en JDE
                    //**********************************************************************************

                    DatosExento exento = new DatosExento();
                    // preparar data para enviar a UpdateExento
                    exento.PEQ70CVO = vuelosValidados.NumeroVuelo;
                    ////DateTime fechaprueba = new DateTime(2017, 08, 10);
                    exento.PEQ70FRL = vuelosValidados.FechaVuelo;


                    /*
                    // traigo los pasajeros exentos de jarvis que realizaron el viaje
                    Store.Helper.Ejecutor ejecutorEx = new Store.Helper.Ejecutor(this._config);
                    ejecutorEx.AgregarCampoIn("_NumeroVuelo", vuelosValidados.NumeroVuelo);
                    ejecutorEx.AgregarCampoIn("_fecha", vuelosValidados.FechaVuelo);
                    DataTable exentosvalidados = ejecutorEx.Conexion("TraerExentosValidados");
                    */
                    try
                    {

                        // consulta data de exento en JDE
                        List<ExentoODT> exentoODT = new List<ExentoODT>();
                        exentoODT = TraerExento(vuelosValidados.NumeroVuelo, vuelosValidados.FechaVuelo);

                        if (exentoODT != null)
                        {
                            for (int pp = 0; pp < exentoODT.Count; pp++)
                            {
                                _logger.LogInformation("6. encuentra exentos");
                                // solo actualizo si en jarvis lo colocaron como realiza el viaje

                                //exento.PEQ70IDP = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                exento.PEQ70IDP = exentoODT[pp].idpax1;
                                exento.PEQ70EST = "REP";
                                exento.PEQ70IDX = exentoODT[pp].Id.ToString();
                                exento.PEQ70TER = exentoODT[pp].Terminal;
                                exento.PEQ70IDPPAX1 = exentoODT[pp].idpax1;
                                exento.PEQ70IDPPAX2 = exentoODT[pp].idpax2;
                                exento.viajo = Int32.Parse(exentoODT[pp].realiza_viaje);

                                // ejecucion de UpdateExento
                                string UpdateExento = Query.ActualizaExento.QueryUPDExento(exento,this._config);
                                _logger.LogInformation(UpdateExento);
                                await EjecutarQuery(UpdateExento);
                                // preparar data para enviar a InsAuditoriaEx
                                exento.AEQ70IDX = exento.PEQ70IDX; // Número de Exento 

                                // conslta para sber el ultimo row de la tbl FQ70594F
                                string consultaUltRow = Query.ActualizaExento.ConsultaUtlRow("FQ70594F",this._config);
                                var rptaUtlRow1 = await Ejecutar<UltRowsTblsEX>(consultaUltRow);
                                List<UltRowsTblsEX> resultSetUtlRow;
                                resultSetUtlRow = rptaUtlRow1.Resultado<UltRowsTblsEX>();

                                exento.PEQ70TER = exentoODT[pp].Terminal;
                                exento.AELNID = resultSetUtlRow[0].AELNID; // Número de Linea, “Se inserta el ultimo +1”

                                exento.AEQ70EST = exento.PEQ70EST; // Estado que se Actualiza
                                exento.AEURCD = ""; // En Blanco
                                exento.AEURDT = "0"; // Vaor en 0
                                exento.AEURAT = "0"; // Valor en 0
                                exento.AEURAB = "0"; // Valor en 0
                                exento.AEURRF = ""; // En Blanco
                                exento.AEUSER = "JARVIS"; // Usuario Asignado
                                exento.AEPID = "JARVIS"; // Programa que inserta los datos (Defecto RQ70591DU)
                                exento.AEJOBN = "SOPAIN36"; // Servidor (Defecto SOPAIN36)
                                exento.AEUPMJ = Query.ActualizaExento.ConvertToJulian(DateTime.Now).ToString();//Fechas de Actualización(FechaJuliana)
                                exento.AEUPMT = DateTime.Now.ToString("hhmmss");// Hora de Actualización(formato hhmmss)
                                                                                // ejecucion de InsAuditoriaEx
                                string InsAuditoriaEx = Query.ActualizaExento.QueryINSAuditEX(exento,this._config);
                                await EjecutarQuery(InsAuditoriaEx);

                                // preparar data para enviar a UPDAuditRegVuelo
                                exento.APQ70IDP = exento.PEQ70IDP; // Número de ID del registro de Vuelo

                                // conslta para sber el ultimo row de la tbl FQ70594F
                                string consultaUltRow2 = Query.ActualizaExento.ConsultaUtlRow("FQ70594C",this._config);
                                var rptaUtlRow2 = await Ejecutar<UltRowsTblsEX>(consultaUltRow2);
                                List<UltRowsTblsEX> resultSetUtlRow2;
                                resultSetUtlRow2 = rptaUtlRow2.Resultado<UltRowsTblsEX>();
                                exento.APLNID = resultSetUtlRow2[0].APLNID; // Número de Linea(Tomar la ultima +1)

                                exento.APQ70CAU = ""; // En blanco
                                exento.APDL011 = ""; // En blanco
                                exento.APEV01 = ""; // En blanco
                                exento.APDSC1 = "Adiciona Exentos"; // Agregar el texto  ‘Adiciona Exentos              ’
                                exento.APDSC2 = "-"; // Agreagar el texto ‘-                            ’
                                exento.APDSC3 = ""; // Número de Documento del exento ‘74375480                      ’
                                exento.APQ70EST = "06"; // Estado del Vuelo ‘06  ’
                                exento.APURCD = ""; // En blanco
                                exento.APURAT = "0"; // Campo en valor(0)
                                exento.APURAB = "0"; // Campo en valor(0)
                                exento.APURRF = ""; // Campo en Blanco
                                exento.APTORG = "JARVIS"; // Usuario que inserta Ejm Jarvis
                                exento.APUSER = "JARVIS"; // Usuario que inserta Ejm Jarvis
                                exento.APPID = "PQ70594A"; // Programa que inserta la Linea(PQ70594A  )
                                exento.APJOBN = "sopain29"; // Servidor sopain29
                                exento.APUPMJ = Query.ActualizaExento.ConvertToJulian(DateTime.Now).ToString(); // Fecha en juliana  ‘120009’
                                exento.APUPMT = DateTime.Now.ToString("hhmmss"); // Hora Formato(hhmmss)
                                                                                 // ejecucion de UPDAuditRegVuelo
                                string UPDAuditRegVuelo = Query.ActualizaExento.QueryUPDAuditRegVuelo(exento,this._config);

                                await EjecutarQuery(UPDAuditRegVuelo);
                            }
                        }
                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {
                        //Trace Log ??
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                        _logger.LogInformation($" 2. Error Exentos: {Newtonsoft.Json.JsonConvert.SerializeObject(ex)} ");

                        this.Error = ex;
                        resReturn = 0;
                    }

                    //**********************************************************************************

                    // Con el id de daily y pasajero actualizo tabla tránsito FQ70594B
                    try
                    {
                        _logger.LogInformation("/--------/7. Inicio Transito/--------/");
                        _logger.LogInformation("71. va a traer los transitos desde el id " + vuelosValidados.IdOperacionVuelo.ToString());
                        Store.Helper.Ejecutor ejecutor1 = new Store.Helper.Ejecutor(this._config);
                        ejecutor1.AgregarCampoIn("_IdOperacionVuelo", vuelosValidados.IdOperacionVuelo);
                        DataTable ListadoTransito = ejecutor1.Conexion("TraerDatosTransito");
                        _logger.LogInformation("72. trajo esta cantidad de transitos " + ListadoTransito.Rows.Count);
                        await this.EnviarTransitosJDE(ListadoTransito);
                        _logger.LogInformation("/--------/Fin transitos/--------/");
                    }
                    catch (Exception ex)
                    {
                        //Trace Log ??
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));

                        _logger.LogInformation($" 3. Error en transitos: {JsonConvert.SerializeObject(ex)} ");

                        this.Error = ex;
                        resReturn = 0;
                    }

                    try
                    {
                        _logger.LogInformation("10. Inicio Causales");
                        // Inserto en tabla de causales FQ70594G
                        Store.Helper.Ejecutor ejecutor2 = new Store.Helper.Ejecutor(this._config);
                        ejecutor2.AgregarCampoIn("_IdOperacionesVuelo", vuelosValidados.IdOperacionVuelo);
                        DataTable ListadoCausales = ejecutor2.Conexion("TraerCausales"); // Estas causales son validaciones Automaticas y manuales
                        int lineaCausal = 1;
                        Causales datosCausales;
                        string consultaCausal;
                        for (int causal = 0; causal < ListadoCausales.Rows.Count; causal++)
                        {
                            if (ListadoCausales.Rows[causal]["Causal"].ToString().Trim() != "")
                            {
                                // instancia de objeto causales
                                datosCausales = new Causales();
                                // preparacion previa de data para el exec
                                //ListadoCausales.Rows[causal]["Antiguo"]
                                if (!int.TryParse(ListadoCausales.Rows[causal]["Antiguo"].ToString(), out int Antiguo))
                                {
                                    Antiguo = 0;
                                }
                                //ListadoCausales.Rows[causal]["Nuevo"]
                                if (!int.TryParse(ListadoCausales.Rows[causal]["Nuevo"].ToString(), out int Nuevo))
                                {
                                    Nuevo = 0;
                                }
                                // seteo de obj de causales relacionado...
                                datosCausales.Causal = (ListadoCausales.Rows[causal]["Causal"] == null ? "" : ListadoCausales.Rows[causal]["Causal"].ToString());
                                /*datosCausales.TotalPax = Math.Abs(Convert.ToInt32(Antiguo) - Convert.ToInt32(Nuevo));*/
                                datosCausales.TotalPax = Convert.ToInt32(Nuevo);
                                datosCausales.GeneraCobro = 1;
                                datosCausales.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosCausales.Linea = lineaCausal * 1000;
                                datosCausales.TipoVuelo = vuelosValidados.TipoVuelo;
                                //armado de la consulta
                                consultaCausal = Query.ActualizarCausales.Consulta(datosCausales,this._config);
                                // exec de la consulta armada previamente...
                                await EjecutarQuery(consultaCausal);
                                lineaCausal = lineaCausal + 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Trace Log ??
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));

                        _logger.LogInformation($" 4. Error en Causales: {Newtonsoft.Json.JsonConvert.SerializeObject(ex)} ");
                        this.Error = ex;
                        resReturn = 0;
                    }

                    // Actualizo y coloco en 6 la tabla de operaciones vuelos
                    ejecutor = new Store.Helper.Ejecutor(this._config);
                    ejecutor.AgregarCampoIn("_IdOperacionesVuelo", vuelosValidados.IdOperacionVuelo);
                    int resultadofinal = ejecutor.ConexionEx("ActualizarEstadoVuelo");
                    _logger.LogInformation("11. Cambia a estado 6 los vuelos");
                    //return 1;
                }
                catch (Exception ex)
                {
                    //Trace Log ??
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                    _logger.LogInformation($"5. Error : {Newtonsoft.Json.JsonConvert.SerializeObject(ex)} ");
                    this.Error = ex;
                    resReturn = 0;
                }
            }
            _logger.LogInformation("1. fin for vuelos validados");
            _logger.LogInformation("12. Fin");
            return resReturn;
        }



        public async Task<int> ActualizarVueloValidado(string vuelo)
        {

            int resReturn = 1;

            _logger.LogInformation("1. Inicio Integracion para el vuelo con id " + vuelo);

            _logger.LogInformation("1. Iniciar traer vuelo validado id " + vuelo);
            Store.Helper.Ejecutor ejecutor = new Store.Helper.Ejecutor(this._config);
            ejecutor.AgregarCampoIn("_NumeroVuelo", vuelo);
            DataTable resultado = ejecutor.Conexion("TraerVueloValidado");
            _logger.LogInformation("1. fin traer vuelos validados ");
            _logger.LogInformation("1. total vuelos validados: " + resultado.Rows.Count);
            //VuelosValidados vuelosValidados22 = new VuelosValidados();
            //string consultaxxx = Query.ActualizaTotales.ActualizaTotalesJDE(vuelosValidados22);
            // Actualizo la tabla de "Pasajeros"
            // Luego de que traigo los resultados inserto en la tabla de oracle de "pasajeros"
            // Claves que forman la llave primaria
            _logger.LogInformation("1. inicia for vuelos validado :" + vuelo);
            for (int i = 0; i < resultado.Rows.Count; i++)
            {
                _logger.LogInformation("2. Encontro vuelos a enviar a JDE id :"+  vuelo);
                try
                {
                    // preparacion de data previa para evitar break en la logica relacionada....
                    //resultado.Rows[i]["FechaVuelo"].ToString()
                    if (!DateTime.TryParse(resultado.Rows[i]["FechaVuelo"].ToString(), out DateTime fchVuelo))
                    {
                        fchVuelo = DateTime.Now;
                    }
                    //resultado.Rows[i]["Adultos"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["Adultos"].ToString(), out int Adultos))
                    {
                        Adultos = 0;
                    }
                    //resultado.Rows[i]["Infantes"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["Infantes"].ToString(), out int Infantes))
                    {
                        Infantes = 0;
                    }
                    //resultado.Rows[i]["PagoCOP"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["PagoCOP"].ToString(), out int PagoCOP))
                    {
                        PagoCOP = 0;
                    }
                    //resultado.Rows[i]["PagoUSD"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["PagoUSD"].ToString(), out int PagoUSD))
                    {
                        PagoUSD = 0;
                    }
                    //resultado.Rows[i]["TotalConexion"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["TotalConexion"].ToString(), out int TotalConexion))
                    {
                        TotalConexion = 0;
                    }
                    //resultado.Rows[i]["TotalLinea"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["TotalLinea"].ToString(), out int TotalLinea))
                    {
                        TotalLinea = 0;
                    }
                    //resultado.Rows[i]["Tripulacion"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["Tripulacion"].ToString(), out int Tripulacion))
                    {
                        Tripulacion = 0;
                    }
                    //resultado.Rows[i]["TotalEmbarcados"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["TotalEmbarcados"].ToString(), out int TotalEmbarcados))
                    {
                        TotalEmbarcados = 0;
                    }
                    //resultado.Rows[i]["IdOperacionVuelo"].ToString()
                    if (!int.TryParse(resultado.Rows[i]["IdOperacionVuelo"].ToString(), out int IdOperacionVuelo))
                    {
                        IdOperacionVuelo = 0;
                    }

                    _logger.LogInformation("2. informarcion vuelos a enviar a JDE: " + vuelo);
                    // nueva instancia de vuelos validados
                    VuelosValidados vuelosValidados = new VuelosValidados();
                    // seteo de params relacionados
                    vuelosValidados.HoraVuelo = (resultado.Rows[i]["HoraVuelo"] == null ? "" : resultado.Rows[i]["HoraVuelo"].ToString());
                    vuelosValidados.FechaVuelo = Convert.ToDateTime(fchVuelo);
                    vuelosValidados.NumeroVuelo = (resultado.Rows[i]["NumeroVuelo"] == null ? "" : resultado.Rows[i]["NumeroVuelo"].ToString());
                    vuelosValidados.Aerolinea = (resultado.Rows[i]["Aerolina"] == null ? "" : resultado.Rows[i]["Aerolina"].ToString());
                    vuelosValidados.Matricula = (resultado.Rows[i]["Matricula"] == null ? "" : resultado.Rows[i]["Matricula"].ToString());
                    vuelosValidados.Adultos = Convert.ToInt32(Adultos);
                    vuelosValidados.Infantes = Convert.ToInt32(Infantes);
                    vuelosValidados.PagoCOP = Convert.ToInt32(PagoCOP);
                    vuelosValidados.PagoUSD = Convert.ToInt32(PagoUSD);
                    vuelosValidados.TotalConexion = Convert.ToInt32(TotalConexion);
                    vuelosValidados.TotalLinea = Convert.ToInt32(TotalLinea);
                    vuelosValidados.Tripulacion = Convert.ToInt32(Tripulacion);
                    vuelosValidados.TotalEmbarcados = Convert.ToInt32(TotalEmbarcados);
                    vuelosValidados.TipoVuelo = (resultado.Rows[i]["TipoVuelo"] == null ? "" : resultado.Rows[i]["TipoVuelo"].ToString());
                    vuelosValidados.IdOperacionVuelo = Convert.ToInt32(IdOperacionVuelo);
                    vuelosValidados.Id_daily = (resultado.Rows[i]["IdDaily"] == null ? "-1" : resultado.Rows[i]["IdDaily"].ToString());

                    _logger.LogInformation("2. fin informacion vuelos a enviar a JDE");
                    // selecciono los totales actuales
                    string consulta2 = Query.ActualizaTotales.ConsultaSelect(vuelosValidados, this._config);

                    //consulta vuelos validados
                    _logger.LogInformation($"3. Vuelos validados: {consulta2} ");

                    var respuesta2 = await Ejecutar<VuelosValidados>(consulta2);
                    _logger.LogInformation("2. Antes de actualizar totales en JDE" + consulta2);

                    // Actualizo los totales en pasajeros
                    string consulta = Query.ActualizaTotales.ActualizaTotalesJDE(vuelosValidados, this._config);
                    _logger.LogInformation($"4 Consulta actualizar totales JDE: {consulta} ");

                    await EjecutarQuery(consulta);

                    // Actualizo totales actuales en auditoria

                    List<VuelosValidados> datosdaylyypax;

                    datosdaylyypax = respuesta2.Resultado<VuelosValidados>();

                    try
                    {
                        if (datosdaylyypax.Count > 0)
                        {
                            if (datosdaylyypax[0] != null)
                            {
                                _logger.LogInformation("5. Inserta Auditoria");
                                string consultaAudit;
                                DatosAuditoria datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 1000;
                                datosAuditoria.Campo_Modificado = "PEQ70TTA";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].Adultos.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.Adultos.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria, this._config);
                                await EjecutarQuery(consultaAudit);

                                datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 2000;
                                datosAuditoria.Campo_Modificado = "PEQ70TTI";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].Infantes.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.Infantes.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria, this._config);
                                await EjecutarQuery(consultaAudit);

                                datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 3000;
                                datosAuditoria.Campo_Modificado = "PEQ70EC";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].Tripulacion.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.Tripulacion.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria, this._config);
                                await EjecutarQuery(consultaAudit);

                                datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 4000;
                                datosAuditoria.Campo_Modificado = "PEQ70TTC";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].PagoCOP.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.PagoCOP.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria, this._config);
                                await EjecutarQuery(consultaAudit);

                                datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 5000;
                                datosAuditoria.Campo_Modificado = "PEQ70TTU";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].PagoUSD.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.PagoUSD.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria, this._config);
                                await EjecutarQuery(consultaAudit);

                                datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 6000;
                                datosAuditoria.Campo_Modificado = "PEQ70TRP";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].TotalLinea.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.TotalLinea.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria, this._config);
                                await EjecutarQuery(consultaAudit);

                                datosAuditoria = new DatosAuditoria();
                                datosAuditoria.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosAuditoria.Genera_Cobro = 1;
                                datosAuditoria.Linea = 7000;
                                datosAuditoria.Campo_Modificado = "PEQ70TRF";
                                datosAuditoria.Valor_Anterior = datosdaylyypax[0].TotalConexion.ToString();
                                datosAuditoria.Valor_Nuevo = vuelosValidados.TotalConexion.ToString();
                                consultaAudit = Query.InsertoAuditoria.Consulta(datosAuditoria, this._config);
                                await EjecutarQuery(consultaAudit);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Trace Log ??
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));

                        _logger.LogInformation($" 1. Error Auditoria: {Newtonsoft.Json.JsonConvert.SerializeObject(ex)} ");
                        this.Error = ex;
                        resReturn = 0;
                    }

                    // segmento para actualizar exentos en JDE
                    //**********************************************************************************

                    DatosExento exento = new DatosExento();
                    // preparar data para enviar a UpdateExento
                    exento.PEQ70CVO = vuelosValidados.NumeroVuelo;
                    ////DateTime fechaprueba = new DateTime(2017, 08, 10);
                    exento.PEQ70FRL = vuelosValidados.FechaVuelo;


                    /*
                    // traigo los pasajeros exentos de jarvis que realizaron el viaje
                    Store.Helper.Ejecutor ejecutorEx = new Store.Helper.Ejecutor(this._config);
                    ejecutorEx.AgregarCampoIn("_NumeroVuelo", vuelosValidados.NumeroVuelo);
                    ejecutorEx.AgregarCampoIn("_fecha", vuelosValidados.FechaVuelo);
                    DataTable exentosvalidados = ejecutorEx.Conexion("TraerExentosValidados");
                    */
                    try
                    {

                        // consulta data de exento en JDE
                        List<ExentoODT> exentoODT = new List<ExentoODT>();
                        exentoODT = TraerExento(vuelosValidados.NumeroVuelo, vuelosValidados.FechaVuelo);

                        if (exentoODT != null)
                        {
                            for (int pp = 0; pp < exentoODT.Count; pp++)
                            {
                                _logger.LogInformation("6. encuentra exentos");
                                // solo actualizo si en jarvis lo colocaron como realiza el viaje

                                //exento.PEQ70IDP = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                exento.PEQ70IDP = exentoODT[pp].idpax1;
                                exento.PEQ70EST = "REP";
                                exento.PEQ70IDX = exentoODT[pp].Id.ToString();
                                exento.PEQ70TER = exentoODT[pp].Terminal;
                                exento.PEQ70IDPPAX1 = exentoODT[pp].idpax1;
                                exento.PEQ70IDPPAX2 = exentoODT[pp].idpax2;
                                exento.viajo = Int32.Parse(exentoODT[pp].realiza_viaje);

                                // ejecucion de UpdateExento
                                string UpdateExento = Query.ActualizaExento.QueryUPDExento(exento, this._config);
                                _logger.LogInformation(UpdateExento);
                                await EjecutarQuery(UpdateExento);
                                // preparar data para enviar a InsAuditoriaEx
                                exento.AEQ70IDX = exento.PEQ70IDX; // Número de Exento 

                                // conslta para sber el ultimo row de la tbl FQ70594F
                                string consultaUltRow = Query.ActualizaExento.ConsultaUtlRow("FQ70594F", this._config);
                                var rptaUtlRow1 = await Ejecutar<UltRowsTblsEX>(consultaUltRow);
                                List<UltRowsTblsEX> resultSetUtlRow;
                                resultSetUtlRow = rptaUtlRow1.Resultado<UltRowsTblsEX>();

                                exento.PEQ70TER = exentoODT[pp].Terminal;
                                exento.AELNID = resultSetUtlRow[0].AELNID; // Número de Linea, “Se inserta el ultimo +1”

                                exento.AEQ70EST = exento.PEQ70EST; // Estado que se Actualiza
                                exento.AEURCD = ""; // En Blanco
                                exento.AEURDT = "0"; // Vaor en 0
                                exento.AEURAT = "0"; // Valor en 0
                                exento.AEURAB = "0"; // Valor en 0
                                exento.AEURRF = ""; // En Blanco
                                exento.AEUSER = "JARVIS"; // Usuario Asignado
                                exento.AEPID = "JARVIS"; // Programa que inserta los datos (Defecto RQ70591DU)
                                exento.AEJOBN = "SOPAIN36"; // Servidor (Defecto SOPAIN36)
                                exento.AEUPMJ = Query.ActualizaExento.ConvertToJulian(DateTime.Now).ToString();//Fechas de Actualización(FechaJuliana)
                                exento.AEUPMT = DateTime.Now.ToString("hhmmss");// Hora de Actualización(formato hhmmss)
                                                                                // ejecucion de InsAuditoriaEx
                                string InsAuditoriaEx = Query.ActualizaExento.QueryINSAuditEX(exento, this._config);
                                await EjecutarQuery(InsAuditoriaEx);

                                // preparar data para enviar a UPDAuditRegVuelo
                                exento.APQ70IDP = exento.PEQ70IDP; // Número de ID del registro de Vuelo

                                // conslta para sber el ultimo row de la tbl FQ70594F
                                string consultaUltRow2 = Query.ActualizaExento.ConsultaUtlRow("FQ70594C", this._config);
                                var rptaUtlRow2 = await Ejecutar<UltRowsTblsEX>(consultaUltRow2);
                                List<UltRowsTblsEX> resultSetUtlRow2;
                                resultSetUtlRow2 = rptaUtlRow2.Resultado<UltRowsTblsEX>();
                                exento.APLNID = resultSetUtlRow2[0].APLNID; // Número de Linea(Tomar la ultima +1)

                                exento.APQ70CAU = ""; // En blanco
                                exento.APDL011 = ""; // En blanco
                                exento.APEV01 = ""; // En blanco
                                exento.APDSC1 = "Adiciona Exentos"; // Agregar el texto  ‘Adiciona Exentos              ’
                                exento.APDSC2 = "-"; // Agreagar el texto ‘-                            ’
                                exento.APDSC3 = ""; // Número de Documento del exento ‘74375480                      ’
                                exento.APQ70EST = "06"; // Estado del Vuelo ‘06  ’
                                exento.APURCD = ""; // En blanco
                                exento.APURAT = "0"; // Campo en valor(0)
                                exento.APURAB = "0"; // Campo en valor(0)
                                exento.APURRF = ""; // Campo en Blanco
                                exento.APTORG = "JARVIS"; // Usuario que inserta Ejm Jarvis
                                exento.APUSER = "JARVIS"; // Usuario que inserta Ejm Jarvis
                                exento.APPID = "PQ70594A"; // Programa que inserta la Linea(PQ70594A  )
                                exento.APJOBN = "sopain29"; // Servidor sopain29
                                exento.APUPMJ = Query.ActualizaExento.ConvertToJulian(DateTime.Now).ToString(); // Fecha en juliana  ‘120009’
                                exento.APUPMT = DateTime.Now.ToString("hhmmss"); // Hora Formato(hhmmss)
                                                                                 // ejecucion de UPDAuditRegVuelo
                                string UPDAuditRegVuelo = Query.ActualizaExento.QueryUPDAuditRegVuelo(exento, this._config);

                                await EjecutarQuery(UPDAuditRegVuelo);
                            }
                        }
                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {
                        //Trace Log ??
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                        _logger.LogInformation($" 2. Error Exentos: {Newtonsoft.Json.JsonConvert.SerializeObject(ex)} ");

                        this.Error = ex;
                        resReturn = 0;
                    }

                    //**********************************************************************************

                    // Con el id de daily y pasajero actualizo tabla tránsito FQ70594B
                    try
                    {
                        _logger.LogInformation("/--------/7. Inicio Transito/--------/");
                        _logger.LogInformation("71. va a traer los transitos desde el id " + vuelosValidados.IdOperacionVuelo.ToString());
                        Store.Helper.Ejecutor ejecutor1 = new Store.Helper.Ejecutor(this._config);
                        ejecutor1.AgregarCampoIn("_IdOperacionVuelo", vuelosValidados.IdOperacionVuelo);
                        DataTable ListadoTransito = ejecutor1.Conexion("TraerDatosTransito");
                        _logger.LogInformation("72. trajo esta cantidad de transitos " + ListadoTransito.Rows.Count);
                        await this.EnviarTransitosJDE(ListadoTransito);
                        _logger.LogInformation("/--------/Fin transitos/--------/");
                    }
                    catch (Exception ex)
                    {
                        //Trace Log ??
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));

                        _logger.LogInformation($" 3. Error en transitos: {JsonConvert.SerializeObject(ex)} ");

                        this.Error = ex;
                        resReturn = 0;
                    }

                    try
                    {
                        _logger.LogInformation("10. Inicio Causales");
                        // Inserto en tabla de causales FQ70594G
                        Store.Helper.Ejecutor ejecutor2 = new Store.Helper.Ejecutor(this._config);
                        ejecutor2.AgregarCampoIn("_IdOperacionesVuelo", vuelosValidados.IdOperacionVuelo);
                        DataTable ListadoCausales = ejecutor2.Conexion("TraerCausales"); // Estas causales son validaciones Automaticas y manuales
                        int lineaCausal = 1;
                        Causales datosCausales;
                        string consultaCausal;
                        for (int causal = 0; causal < ListadoCausales.Rows.Count; causal++)
                        {
                            if (ListadoCausales.Rows[causal]["Causal"].ToString().Trim() != "")
                            {
                                // instancia de objeto causales
                                datosCausales = new Causales();
                                // preparacion previa de data para el exec
                                //ListadoCausales.Rows[causal]["Antiguo"]
                                if (!int.TryParse(ListadoCausales.Rows[causal]["Antiguo"].ToString(), out int Antiguo))
                                {
                                    Antiguo = 0;
                                }
                                //ListadoCausales.Rows[causal]["Nuevo"]
                                if (!int.TryParse(ListadoCausales.Rows[causal]["Nuevo"].ToString(), out int Nuevo))
                                {
                                    Nuevo = 0;
                                }
                                // seteo de obj de causales relacionado...
                                datosCausales.Causal = (ListadoCausales.Rows[causal]["Causal"] == null ? "" : ListadoCausales.Rows[causal]["Causal"].ToString());
                                /*datosCausales.TotalPax = Math.Abs(Convert.ToInt32(Antiguo) - Convert.ToInt32(Nuevo));*/
                                datosCausales.TotalPax = Convert.ToInt32(Nuevo);
                                datosCausales.GeneraCobro = 1;
                                datosCausales.Id_Pasajero = (datosdaylyypax[0].Id_pasajero == null ? "" : datosdaylyypax[0].Id_pasajero);
                                datosCausales.Linea = lineaCausal * 1000;
                                datosCausales.TipoVuelo = vuelosValidados.TipoVuelo;
                                //armado de la consulta
                                consultaCausal = Query.ActualizarCausales.Consulta(datosCausales, this._config);
                                // exec de la consulta armada previamente...
                                await EjecutarQuery(consultaCausal);
                                lineaCausal = lineaCausal + 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Trace Log ??
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));

                        _logger.LogInformation($" 4. Error en Causales: {Newtonsoft.Json.JsonConvert.SerializeObject(ex)} ");
                        this.Error = ex;
                        resReturn = 0;
                    }

                    // Actualizo y coloco en 6 la tabla de operaciones vuelos
                    ejecutor = new Store.Helper.Ejecutor(this._config);
                    ejecutor.AgregarCampoIn("_IdOperacionesVuelo", vuelosValidados.IdOperacionVuelo);
                    int resultadofinal = ejecutor.ConexionEx("ActualizarEstadoVuelo");
                    _logger.LogInformation("11. Cambia a estado 6 los vuelos");
                    //return 1;
                }
                catch (Exception ex)
                {
                    //Trace Log ??
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                    _logger.LogInformation($"5. Error : {Newtonsoft.Json.JsonConvert.SerializeObject(ex)} ");
                    this.Error = ex;
                    resReturn = 0;
                }
            }
            _logger.LogInformation("1. fin for vuelos validados");
            _logger.LogInformation("12. Fin");
            return resReturn;
        }


        //enviamos transito a JDE

        public async Task<int> EnviarTransitosJDE(DataTable ListadoTransito)
        {

            try
            {
                _logger.LogInformation("/--------/8. encontro transitos inicio/--------/");
                int linea = 1;
                int TotalTransitos = 0;
                int TotalReportados = 0;
                if (ListadoTransito.Rows.Count>0)
                {
                    TotalTransitos = int.Parse(ListadoTransito.Rows[0]["Total"].ToString());
                }
                ;
                for (int k = 0; k < ListadoTransito.Rows.Count; k++)
                {
                    try
                    {
                        _logger.LogInformation("/--------/8.1 encontro transitos inicio/--------/");

                        DatosTransito datosTransito = new DatosTransito();
                        if (!DateTime.TryParse(ListadoTransito.Rows[k]["FechaVuelo"].ToString(), out DateTime fchLlegada))
                        {
                            fchLlegada = DateTime.Now;
                        }
                        _logger.LogInformation($" Numero Vuelo de salida OJO: {ListadoTransito.Rows[k]["NumeroVueloSalida"].ToString()} ");
                        _logger.LogInformation($" Numero Vuelo de llegada OJO: {ListadoTransito.Rows[k]["NumeroVueloLlegada"].ToString()} ");
                        // esta consulta trae el iddaily real

                        _logger.LogInformation($" Numero de vuelo Salida consulta transitos a enviar JDE OJO: {ListadoTransito.Rows[k]["NumeroVueloSalida"].ToString()} ");
                        _logger.LogInformation($" Fecha Vuelo consulta transitos a enviar JDE OJO: {ListadoTransito.Rows[k]["FechaVuelo"].ToString()} ");
                        _logger.LogInformation($" Numero Id_Daily OJO: {Convert.ToInt32(ListadoTransito.Rows[k]["Id_Daily"].ToString())} ");

                        string consultaTransitosIdPasajero = Query.ActualizaTotales.ConsultaSelecTransitoDatosLlegada(Convert.ToInt32(ListadoTransito.Rows[k]["Id_Daily"].ToString()), ListadoTransito.Rows[k]["NumeroVueloSalida"].ToString(), Convert.ToDateTime(ListadoTransito.Rows[k]["FechaVuelo"]), this._config);

                        _logger.LogInformation($"*********** Consulta transitos IdPasajero a enviar JDE OJO: {consultaTransitosIdPasajero} ***************");
                        var respuestaTransitoIdPasajero = await Ejecutar<TransitosJDE>(consultaTransitosIdPasajero);
                        List<TransitosJDE> transitosIdPasajero;
                        transitosIdPasajero = respuestaTransitoIdPasajero.Resultado<TransitosJDE>();

                        _logger.LogInformation($" Ejecuta consulta transitos IdPasajero en JDE ....");

                        string consultaTransitos = Query.ActualizaTotales.ConsultaSelecTransito(
                            ListadoTransito.Rows[k]["NumeroVueloLlegada"].ToString(),
                            Convert.ToDateTime(Convert.ToDateTime(ListadoTransito.Rows[k]["fechallegadaTransitos"]).ToString("yyyy-MM-dd") + " " + ListadoTransito.Rows[k]["HoraLlegada"].ToString()), this._config);

                        _logger.LogInformation($"*********** Consulta transitos General a enviar JDE OJO: {consultaTransitos} ***************");
                        var respuestaTransito = await Ejecutar<TransitosJDE>(consultaTransitos);
                        List<TransitosJDE> transitos;
                        transitos = respuestaTransito.Resultado<TransitosJDE>();

                        _logger.LogInformation($" Ejecuta consulta transitos General en JDE ....");

                        // inserto en la tabla de tránsito
                        // generacion de instancia para tránsitos

                        if (!int.TryParse(ListadoTransito.Rows[k]["TotalPasajero"].ToString(), out int TotalPasajero))
                        {
                            TotalPasajero = 0;
                        }

                        if (TotalPasajero > TotalTransitos)
                        {
                            TotalPasajero = TotalTransitos;
                        }
                        TotalReportados = TotalReportados + TotalPasajero;
                        TotalTransitos = TotalTransitos - TotalPasajero;


                        datosTransito.Aerolinea = (transitos[0].ID_AEROLINEA_LLEGADA == null ? "" : transitos[0].ID_AEROLINEA_LLEGADA.ToString());
                        _logger.LogInformation($"******* Fecha Llegada OJO: {(datosTransito.Aerolinea)} ********");
                        //datosTransito.Fecha_Llegada = Convert.ToDateTime(fchLlegada); 
                        //datosTransito.Hora_Llegada = transitos[0].HORA_LLEGADA_POSICION; 


                        datosTransito.Fecha_Llegada = (transitos[0].FECHA_LLEGADA.ToString() == "" ? DateTime.Now : Convert.ToDateTime(FechaJulianAGregor(transitos[0].FECHA_LLEGADA.ToString())));
                        _logger.LogInformation($"******* Fecha Llegada pendiente OJO: {(transitos[0].FECHA_LLEGADA.ToString() == "" ? DateTime.Now : Convert.ToDateTime(FechaJulianAGregor(transitos[0].FECHA_LLEGADA.ToString())))} ********");
                        datosTransito.Hora_Llegada = (transitos[0].HORA_LLEGADA.ToString() == "" ? "" : transitos[0].HORA_LLEGADA.ToString());
                        _logger.LogInformation($"******* Hora Llegada pendiente OJO: {(datosTransito.Hora_Llegada)} *******");

                        string consultaDestino = Query.ActualizaTotales.ConsultaSelecDestino(transitos[0].ORIGEN.ToString().Trim(), "", this._config);
                        var respuestaDestino = await Ejecutar<DatosCiudades>(consultaDestino);
                        List<DatosCiudades> destino;
                        destino = respuestaDestino.Resultado<DatosCiudades>();

                        datosTransito.id_aeropuerto = (transitos[0].IDA_LLEGADA.ToString().Trim());
                        _logger.LogInformation($"******* Origen pendiente OJO: {(datosTransito.id_aeropuerto)} ********");


                        datosTransito.Id_Linea = linea * 1000;
                        _logger.LogInformation($"******* Numero Id_Linea OJO: {datosTransito.Id_Linea} ********");
                        //datosTransito.Id_Pasajero = (transitosIdPasajero[0].ID_PAX == null ? "" : transitosIdPasajero[0].ID_PAX.ToString());
                        datosTransito.Id_Pasajero = (ListadoTransito.Rows[k]["IdPasajero"] == null ? "" : ListadoTransito.Rows[k]["IdPasajero"].ToString());

                        _logger.LogInformation($"******* Numero ID_PASAJERO OJO: {(datosTransito.Id_Pasajero)} *******");
                        datosTransito.Id_Vuelo = (transitos[0].ID_VUELO == null ? "" : transitos[0].IDV_VUELO.ToString());
                        _logger.LogInformation($"******* Numero Vuelo OJO: {datosTransito.Id_Vuelo} ********");
                        datosTransito.IATACODE = (transitos[0].CODIGO_IATA_LLE == null ? "" : transitos[0].CODIGO_IATA_LLE.ToString());
                        _logger.LogInformation($"******* IATACODE OJO: {datosTransito.IATACODE} ********");
                        datosTransito.OACICODE = (transitos[0].CODIGO_OACI_LLE == null ? "" : transitos[0].CODIGO_OACI_LLE.ToString());
                        _logger.LogInformation($"******* OACICODE OJO: {datosTransito.OACICODE} ********");
                        datosTransito.Numero_Vuelo = (transitos[0].NUM_VUELO_LLEGANDO == null ? "" : transitos[0].NUM_VUELO_LLEGANDO.ToString().Trim());
                        _logger.LogInformation($"******* Numero Vuelo Saliendo OJO: {datosTransito.Numero_Vuelo} ********");
                        datosTransito.Tipo_Transito = (ListadoTransito.Rows[k]["Categoria"] == null ? "" : (ListadoTransito.Rows[k]["Categoria"].ToString() == "TTC") ? "C" : "L");
                        _logger.LogInformation($"******* Tipo_Transito OJO: {datosTransito.Tipo_Transito} ********");
                        datosTransito.Tipo_Vuelo = (transitos[0].TIPO_VUELO == null ? "" : transitos[0].TIPO_VUELO.ToString());
                        _logger.LogInformation($"******* Tipo_Vuelo OJO: {datosTransito.Tipo_Vuelo} ********");
                        datosTransito.Total_Pasajeros = Convert.ToInt32(TotalPasajero);
                        _logger.LogInformation($"******* Saliendo OJO: {datosTransito.Total_Pasajeros} ********");
                        datosTransito.descripcion = "VALIDADO";

                        if (datosTransito.Tipo_Vuelo == "INT")
                        {
                            datosTransito.descripcion2 = "< 24 horas";
                        }
                        else
                        {
                            datosTransito.descripcion2 = "< 6 horas";
                        }

                        _logger.LogInformation($"Cantidad pasajeros: {Convert.ToInt32(TotalPasajero)} ");
                        _logger.LogInformation($"------Inicia proceso de insert-----");

                        string consulta1 = Query.ActualizaTransito.Consulta(datosTransito, this._config);


                        _logger.LogInformation($"9. consultar insertar Transitos: {consulta1} ");
                        await EjecutarQuery(consulta1);
                        linea = linea + 1;
                        _logger.LogInformation($"------Fin proceso de insert, se ejecuta exitosamente-----");

                        _logger.LogInformation("/--------/11. encontro transitos Fin/--------/");
                    }
                    catch (Exception exFor)
                    {
                        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exFor.Message));
                        _logger.LogInformation($"Error Tránsitos EjecutorIntegracion FOR...: {Newtonsoft.Json.JsonConvert.SerializeObject(exFor.Message)} ");
                    }
                }


                return 1;
            }
            catch (Exception ex)
            {
                //Trace Log ??
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex + ex.Message));
                _logger.LogInformation($"Error Transitos EjecutorIntegracion: {Newtonsoft.Json.JsonConvert.SerializeObject(ex + ex.Message)} ");
                this.Error = ex;
                return 0;
            }
        }



        public async Task<int> GrabarCiudades(List<DatosCiudades> lstCiudades)
        {
            foreach (var item in lstCiudades)
            {
                try
                {
                    Store.Helper.Ejecutor ejecutor = new Store.Helper.Ejecutor(this._config);
                    ejecutor.AgregarCampoIn("_Nombre", (item.CIUDAD == null ? "" : item.CIUDAD.Trim()));
                    ejecutor.AgregarCampoIn("_IdPais", (item.PAIS == null ? "" : item.PAIS.Trim()));
                    ejecutor.AgregarCampoIn("_Codigo", (item.COD_IATA == null ? "" : item.COD_IATA.Trim()));
                    int resultado = ejecutor.ConexionEx("Insertar_Ciudades");
                }
                catch (Exception ex)
                {
                    //Trace Log ??
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));

                    this.Error = ex;
                }

            }
            return 1;
        }

        /// <summary>
        ///     Funcionalidad de Grabar los Vuelos con fecha de 1 dia de antelacion consultada por jde
        /// </summary>
        /// <param name="lstVuelos">Parametro de entrada para d_Daily</param>
        /// <returns></returns>
        public async Task<int> GrabarVuelos(List<DatosVuelo> lstVuelos)
        {
            int Cont = 0;
            _logger.LogInformation("5. va a grabar los vuelos en jarvis: Cantidad  " + lstVuelos.Count());
            foreach (var item in lstVuelos)
            {
                try
                {
                    DateTime fchVuelo = DateTime.Now;
                    DateTime fchVueloLlegada = DateTime.Now;

                    //if (item.ID_DAILY == "980118")
                    //{
                        if (item.FECHA_SALIDA != null)
                            fchVuelo = FechaJulianAGregor(item.FECHA_SALIDA);

                        if (item.FECHA_LLEGADA != null)
                            fchVueloLlegada = FechaJulianAGregor(item.FECHA_LLEGADA);

                        Store.Helper.Ejecutor ejecutor = new Store.Helper.Ejecutor(this._config);
                        ejecutor.AgregarCampoIn("_IdVuelo", (item.ID_VUELO == null ? "" : item.ID_VUELO.Trim()));
                        ejecutor.AgregarCampoIn("_FechaVuelo", (fchVuelo));
                        ejecutor.AgregarCampoIn("_MatriculaVuelo", (item.MATRICULA == null ? "" : item.MATRICULA.Trim()));
                        ejecutor.AgregarCampoIn("_HoraVuelo", (item.HORA_SALIDA == null ? "" : item.HORA_SALIDA.Trim()));
                        ejecutor.AgregarCampoIn("_IdAerolinea", (item.ID_AEROLINEA_LLEGADA == null ? "" : item.ID_AEROLINEA_LLEGADA.Trim()));
                        ejecutor.AgregarCampoIn("_Destino", (item.DESTINO == null ? "" : item.DESTINO.Trim()));
                        ejecutor.AgregarCampoIn("_NumeroVuelo", (item.NUM_VUELO_SALIENDO == null ? "" : item.NUM_VUELO_SALIENDO.Trim()));
                        ejecutor.AgregarCampoIn("_TipoVuelo", (item.TIPO_VUELO == null ? "" : item.TIPO_VUELO.Trim()));
                        ejecutor.AgregarCampoIn("_Id_Daily", (item.ID_DAILY == null ? "" : item.ID_DAILY.Trim()));
                        ejecutor.AgregarCampoIn("_IdPasajero", (item.ID_PASAJERO == null ? "" : item.ID_PASAJERO.Trim()));
                        ejecutor.AgregarCampoIn("_Origen", (item.ORIGEN == null ? "" : item.ORIGEN.Trim()));
                        ejecutor.AgregarCampoIn("_OrigenDes", (item.ORIGEN_DES == null ? "" : item.ORIGEN_DES.Trim()));
                        ejecutor.AgregarCampoIn("_NumeroVueloLlegada", (item.NUM_VUELO_LLEGANDO == null ? "" : item.NUM_VUELO_LLEGANDO.Trim()));
                        ejecutor.AgregarCampoIn("_FechaLlegada", fchVueloLlegada);
                        ejecutor.AgregarCampoIn("_HoraLlegada", (item.HORA_LLEGADA == null ? "" : item.HORA_LLEGADA.Trim()));

                        int resultado = ejecutor.ConexionEx("InsertarOperacionesVuelos");

                    string consultaExentos = Query.TraerExentos.Consulta(item.NUM_VUELO_SALIENDO, fchVuelo, this._config);
                    var listaExentos = await Ejecutar<DatosExentos>(consultaExentos);
                    GuardarExentos(listaExentos);
                }
                catch (Exception ex)
                {
                    //Trace Log ??

                    _logger.LogInformation("ERROR.  " + ex.Message.ToString()+"--->"+ ex.StackTrace.ToString());
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));

                    this.Error = ex;
                }
            }
            _logger.LogInformation("5. Vuelos grabado : " + Cont.ToString());
            return 1;


        }


        public async Task<int> GrabarVuelosPendientes(List<DatosVuelo> lstVuelos)
        {
            foreach (var item in lstVuelos)
            {

                DateTime fchVueloLlegada = DateTime.Now;

                if (item.FECHA_LLEGADA != null)
                    fchVueloLlegada = FechaJulianAGregor(item.FECHA_LLEGADA);

                try
                {

                    Store.Helper.Ejecutor ejecutor = new Store.Helper.Ejecutor(this._config);
                    ejecutor.AgregarCampoIn("_Id_Daily", (item.ID_DAILY == null ? "" : item.ID_DAILY.Trim()));
                    ejecutor.AgregarCampoIn("_FECHA_LLEGADA", fchVueloLlegada);
                    ejecutor.AgregarCampoIn("_HORA_LLEGADA", (item.HORA_LLEGADA == null ? "" : item.HORA_LLEGADA.Trim()));
                    ejecutor.AgregarCampoIn("_ORIGEN", (item.ORIGEN == null ? "" : item.ORIGEN.Trim()));
                    ejecutor.AgregarCampoIn("_ID_AEROLINEA_LLEGADA", (item.ID_AEROLINEA_LLEGADA == null ? "" : item.ID_AEROLINEA_LLEGADA.Trim()));
                    int resultado =  ejecutor.ConexionEx("InsertarVuelosPendientes");

                }
                catch (Exception ex)
                {
                    //Trace Log ??
                    _logger.LogInformation("ERROR.  " + ex.Message.ToString() + "--->" + ex.StackTrace.ToString());
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));

                    this.Error = ex;
                }
            }
            return 1;
        }

        public DateTime FechaJulianAGregor(string Julian)
        {
            int jDate = Convert.ToInt32(Julian);
            int day = jDate % 1000;
            int year = (jDate - day) / 1000;
            year = (year >= 17) ? year += 1900 : year += 2000;
            var tempDate = new DateTime(year, 1, 1);
            return tempDate.AddDays(day - 1);
        }

        public async Task<int> ActualizarVuelosFirma(int Id)
        {

            Store.Helper.Ejecutor ejecutor = new Store.Helper.Ejecutor(this._config);
            ejecutor.AgregarCampoIn("_Id", Id);
            int resultado = ejecutor.ConexionEx("ActualizarEstadoFirmadoVuelo");

            return 1;
        }


        private void GuardarExentos(RespuestaGenerica listaExentos)
        {

            foreach (var exento in listaExentos.Resultado<DatosExentos>())
            {
                try
                {

                    var ejecutor = new Store.Helper.Ejecutor(this._config);
                    ejecutor.AgregarCampoIn("_IdExento", exento.IDEXENTO);
                    ejecutor.AgregarCampoIn("_IdVuelo", exento.VUELO.Trim());
                    ejecutor.AgregarCampoIn("_Matricula", exento.MATRICULA.Trim());
                    ejecutor.AgregarCampoIn("_FechaVuelo", exento.FECHA.Trim());
                    ejecutor.AgregarCampoIn("_Pasajero", exento.PASAJERO.Trim());
                    ejecutor.AgregarCampoIn("_Terminal", exento.TERMINAL.Trim());
                    ejecutor.AgregarCampoIn("_pax1", exento.IDPASAJERO);
                    ejecutor.AgregarCampoIn("_pax2", exento.IDPAX2);

                    ejecutor.ConexionEx("Insertar_Exentos");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));

                    this.Error = ex;
                }
            }

        }

        public List<ExentoODT> TraerExento(string idVuelo, DateTime FechaVuelo)
        {
            var Ejecutor = new Store.Helper.Ejecutor(this._config);
            Ejecutor.AgregarCampoIn("_IdVuelo", idVuelo);
            Ejecutor.AgregarCampoIn("_FechaVuelo", FechaVuelo);

            DataTable dt = Ejecutor.Conexion("TraerExentos");

            List<ExentoODT> listExento = new List<ExentoODT>();

            foreach (DataRow dr in dt.Rows)
            {

                ExentoODT exento = new ExentoODT();
                exento.Id = int.Parse(dr[0].ToString());
                exento.id_vuelo = dr[1].ToString();
                exento.Matricula = dr[2].ToString();
                exento.Fecha = Convert.ToDateTime(dr[3].ToString());
                exento.realiza_viaje = dr[4].ToString();
                exento.Pasajero = dr[5].ToString();
                exento.Terminal = dr[6].ToString();
                exento.idpax1 = dr[7].ToString();
                exento.idpax2 = dr[8].ToString();
                listExento.Add(exento);
            }

            return listExento;
        }



    }
}

