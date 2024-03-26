using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Opain.Jarvis.Servicios.Store
{
    public class OperacionesVuelo
    {
        private readonly Helper.Ejecutor Ejecutor;
        private readonly IConfiguration _config;

        public OperacionesVuelo(Helper.Ejecutor ejecutor, IConfiguration config)
        {
            this.Ejecutor = ejecutor;
            this._config = config;
        }

        public void Test()
        {
            DataTable dt = this.Ejecutor.Conexion("test");
        }

        public List<NovedadOtd> TraerNovedadesVuelosErr(int idOpVuelo)
        {


            this.Ejecutor.AgregarCampoIn("_IdOpVuelo", idOpVuelo);
            DataTable dt = this.Ejecutor.Conexion("TraerErrNovedadesVuelo");

            List<NovedadOtd> ErrNovedadesVuelo = new List<NovedadOtd>();

            foreach (DataRow dr in dt.Rows)
            {
                ErrNovedadesVuelo.Add(Helper.ParseDataTableObject.drNovedadOtd(dr));
            }


            return ErrNovedadesVuelo;
        }


        public List<NovedadesAgrupadasOtd> TraerNovedadesAgrupadas(string idAerolinea, string fechaInicio, string fechaFinal)
        {
            this.Ejecutor.AgregarCampoIn("_idAerolinea", idAerolinea);
            this.Ejecutor.AgregarCampoIn("_Fechainicial", fechaInicio);
            this.Ejecutor.AgregarCampoIn("_FechaFinal", fechaFinal);
            DataTable dt = this.Ejecutor.Conexion("novedadesAgrupadas");

            List<NovedadesAgrupadasOtd> _ListaNovedadesAgrupadasOtd = new List<NovedadesAgrupadasOtd>();

            foreach (DataRow dr in dt.Rows)
            {
                _ListaNovedadesAgrupadasOtd.Add(Helper.ParseDataTableObject.drNovedadesAgrupadasOtd(dr));
            }


            return _ListaNovedadesAgrupadasOtd;
        }


        public List<NotificacionODT> TraerNotificaciones(string rolUsr, string idAerolinea, string id_responsable)
        {

            this.Ejecutor.AgregarCampoIn("_IdRol", rolUsr);
            this.Ejecutor.AgregarCampoIn("_Id_aerolinea", idAerolinea);
            this.Ejecutor.AgregarCampoIn("_Id_responsable", id_responsable);
            DataTable dt = this.Ejecutor.Conexion("TraerNotificaciones");

            List<NotificacionODT> NotificacionesUsr = new List<NotificacionODT>();

            foreach (DataRow dr in dt.Rows)
            {
                NotificacionesUsr.Add(Helper.ParseDataTableObject.drNotificacionOdt(dr));
            }


            return NotificacionesUsr;
        }


        public AerolineaOtd TraerAero(string alias)
        {
            this.Ejecutor.AgregarCampoIn("_sigla", alias);

            DataTable dt = this.Ejecutor.Conexion("TraerAerolinea");

            AerolineaOtd aerolinea = new AerolineaOtd();

            if (dt.Rows.Count > 0)
            {
                aerolinea.Id = int.Parse(dt.Rows[0]["Id"].ToString());
                aerolinea.Nombre = dt.Rows[0]["Nombre"].ToString();
                aerolinea.Sigla = dt.Rows[0]["sigla"].ToString();
            }

            return aerolinea;
        }



        public List<OperacionVueloOtd> TraerPendientesConfirmar(int idAerolinea, int pagina, int registrosPorPagina, out int cantidadRegistros)
        {
            cantidadRegistros = 0;

            this.Ejecutor.AgregarCampoIn("_IdAerolinea", idAerolinea);
            this.Ejecutor.AgregarCampoIn("_Pagina", pagina);
            this.Ejecutor.AgregarCampoIn("_RegistrosPorPagina", registrosPorPagina);
            this.Ejecutor.AgregarCampoOut("_CantidadRegistros", DbType.Int32, cantidadRegistros);

            DataTable dt = this.Ejecutor.Conexion("OperacionVuelos_NoConfirmados");

            cantidadRegistros = this.Ejecutor.ObtenerCampoSalida<int>("_CantidadRegistros");

            List<OperacionVueloOtd> Vuelos = new List<OperacionVueloOtd>();
            foreach (DataRow dr in dt.Rows)
            {
                Vuelos.Add(Helper.ParseDataTableObject.drOperacionVueloOtd(dr));
            }

            return Vuelos;
        }
        public List<OperacionVueloOtd> TraerPendientesConfirmarSoporte(int idAerolinea, int pagina, int registrosPorPagina, out int cantidadRegistros)
        {
            cantidadRegistros = 0;

            this.Ejecutor.AgregarCampoIn("_IdAerolinea", idAerolinea);
            this.Ejecutor.AgregarCampoIn("_Pagina", pagina);
            this.Ejecutor.AgregarCampoIn("_RegistrosPorPagina", registrosPorPagina);
            this.Ejecutor.AgregarCampoOut("_CantidadRegistros", DbType.Int32, cantidadRegistros);

            DataTable dt = this.Ejecutor.Conexion("OperacionVuelos_NoConfirmadosSoporte");
            //DataTable dt = this.Ejecutor.Conexion("OperacionVuelos_NoConfirmados");

            cantidadRegistros = this.Ejecutor.ObtenerCampoSalida<int>("_CantidadRegistros");

            List<OperacionVueloOtd> Vuelos = new List<OperacionVueloOtd>();
            foreach (DataRow dr in dt.Rows)
            {
                Vuelos.Add(Helper.ParseDataTableObject.drOperacionVueloOtd(dr));
            }

            return Vuelos;
        }

        public void InsOrUpdExento(ExentoODT ex)
        {
            this.Ejecutor.AgregarCampoIn("_id_vuelo", ex.id_vuelo);
            this.Ejecutor.AgregarCampoIn("_nombre", ex.nombre);
            this.Ejecutor.AgregarCampoIn("_tipo_exento", ex.tipo_exento);
            this.Ejecutor.AgregarCampoIn("_realiza_viaje", ex.realiza_viaje);
            this.Ejecutor.AgregarCampoIn("_motivo_exencion", ex.motivo_exencion);

            this.Ejecutor.Conexion("InsOrUpd_Exento");

        }

        public void TraerDatosValidacionManual(int idVuelo, out int cantPasajeros, out int cantTransitos, out int cantInfantes, out int cantTTL, out int cantTTC, out int cantEX, out int cantTRIP, out decimal tasaCOP, out decimal tasaUSD)
        {
            cantPasajeros = 0;
            cantTransitos = 0;
            cantInfantes = 0;
            cantTTL = 0;
            cantTTC = 0;
            cantEX = 0;
            cantTRIP = 0;
            tasaCOP = 0;
            tasaUSD = 0;

            this.Ejecutor.AgregarCampoIn("_idVuelo", idVuelo);
            this.Ejecutor.AgregarCampoOut("_cantPasajeros", DbType.Int32, cantPasajeros);
            this.Ejecutor.AgregarCampoOut("_cantTransitos", DbType.Int32, cantTransitos);
            this.Ejecutor.AgregarCampoOut("_cantInfantes", DbType.Int32, cantInfantes);
            this.Ejecutor.AgregarCampoOut("_cantTTL", DbType.Int32, cantTTL);
            this.Ejecutor.AgregarCampoOut("_cantTTC", DbType.Int32, cantTTC);
            this.Ejecutor.AgregarCampoOut("_cantEX", DbType.Int32, cantEX);
            this.Ejecutor.AgregarCampoOut("_cantTRIP", DbType.Int32, cantTRIP);
            this.Ejecutor.AgregarCampoOut("_tasaCOP", DbType.Decimal, tasaCOP);
            this.Ejecutor.AgregarCampoOut("_tasaUSD", DbType.Decimal, tasaUSD);


            this.Ejecutor.Conexion("TraerDatosValidacionManual");

            cantPasajeros = this.Ejecutor.ObtenerCampoSalida<int>("_cantPasajeros");
            cantTransitos = this.Ejecutor.ObtenerCampoSalida<int>("_cantTransitos");
            cantInfantes = this.Ejecutor.ObtenerCampoSalida<int>("_cantInfantes");
            cantTTL = this.Ejecutor.ObtenerCampoSalida<int>("_cantTTL");
            cantTTC = this.Ejecutor.ObtenerCampoSalida<int>("_cantTTC");
            cantEX = this.Ejecutor.ObtenerCampoSalida<int>("_cantEX");
            cantTRIP = this.Ejecutor.ObtenerCampoSalida<int>("_cantTRIP");
            tasaCOP = this.Ejecutor.ObtenerCampoSalida<decimal>("_tasaCOP");
            tasaUSD = this.Ejecutor.ObtenerCampoSalida<decimal>("_tasaUSD");

        }
        // llamado al sp OperacionVuelos_Validar para validaciones automaticas....
        public void OperacionVuelos_Validar(int id)
        {
            this.Ejecutor.AgregarCampoIn("_Id", id);
            this.Ejecutor.Conexion("OperacionVuelos_Validar");
        }

        public CargueOtd ConsultarIdCargue(int IdCargue)
        {
            this.Ejecutor.AgregarCampoIn("_IdCargue", IdCargue);
            CargueOtd Dto = new CargueOtd();
            DataTable dt = this.Ejecutor.Conexion("ConsultarIdCargue");
            if (dt.Rows.Count > 0)
            {
                Dto.Id = int.Parse(dt.Rows[0]["Id"].ToString());
                Dto.Fecha = DateTime.Parse(dt.Rows[0]["Fecha"].ToString());
                Dto.Usuario = dt.Rows[0]["Usuario"].ToString();
                Dto.NombreArchivo = dt.Rows[0]["NombreArchivo"].ToString();
                Dto.TipoArchivo = dt.Rows[0]["TipoArchivo"].ToString();
                Dto.NombreCompletoUsuario = dt.Rows[0]["NombreCompletoUsuario"].ToString();
            }

            return Dto;
        }



        public void ActualizaExentos(int id_pax)
        {
            this.Ejecutor.AgregarCampoIn("_id_pax", id_pax);
            this.Ejecutor.Conexion("ActualizaExentos");
        }

        public void NotificacionesINS(NotificacionODT notify)
        {
            this.Ejecutor.AgregarCampoIn("_titulo", notify.titulo);
            this.Ejecutor.AgregarCampoIn("_descripcion", notify.descripcion);
            this.Ejecutor.AgregarCampoIn("_id_item", notify.id_item);
            this.Ejecutor.AgregarCampoIn("_id_responsable", notify.id_responsable);
            this.Ejecutor.AgregarCampoIn("_rol_responsable", notify.rol_responsable);
            this.Ejecutor.AgregarCampoIn("_id_aerolinea", notify.id_aerolinea);

            this.Ejecutor.Conexion("Insertar_Notificacion");



        }

        public void InsertValidaManual(int idVuelo, string CantPasajeros_Old, string CantPasajeros_New, string CausalPasajeros,
                                        string CantTransitos_Old, string CantTransitos_New, string CausalTransitos,
                                        string CantInfantes_Old, string CantInfantes_New, string CausalInfantes,
                                        string CantTTL_Old, string CantTTL_New, string CausalTTL,
                                        string CantTTC_Old, string CantTTC_New, string CausalTTC,
                                        string CantEX_Old, string CantEX_New, string CausalEX,
                                        string CantTRIP_Old, string CantTRIP_New, string CausalTRIP,
                                        string CantPagoCOP_Old, string CantPagoCOP_New, string CausalPagoCOP,
                                        string CantPagoUSD_Old, string CantPagoUSD_New, string CausalPagoUSD)
        {

            // se recogen los parametros relacionados para el sp de insert validacion manual
            this.Ejecutor.AgregarCampoIn("idVuelo", idVuelo);
            this.Ejecutor.AgregarCampoIn("CantPasajeros_Old", CantPasajeros_Old);
            this.Ejecutor.AgregarCampoIn("CantPasajeros_New", CantPasajeros_New);
            this.Ejecutor.AgregarCampoIn("CausalPasajeros", CausalPasajeros);
            this.Ejecutor.AgregarCampoIn("CantTransitos_Old", CantTransitos_Old);
            this.Ejecutor.AgregarCampoIn("CantTransitos_New", CantTransitos_New);
            this.Ejecutor.AgregarCampoIn("CausalTransitos", CausalTransitos);
            this.Ejecutor.AgregarCampoIn("CantInfantes_Old", CantInfantes_Old);
            this.Ejecutor.AgregarCampoIn("CantInfantes_New", CantInfantes_New);
            this.Ejecutor.AgregarCampoIn("CausalInfantes", CausalInfantes);
            this.Ejecutor.AgregarCampoIn("CantTTL_Old", CantTTL_Old);
            this.Ejecutor.AgregarCampoIn("CantTTL_New", CantTTL_New);
            this.Ejecutor.AgregarCampoIn("CausalTTL", CausalTTL);
            this.Ejecutor.AgregarCampoIn("CantTTC_Old", CantTTC_Old);
            this.Ejecutor.AgregarCampoIn("CantTTC_New", CantTTC_New);
            this.Ejecutor.AgregarCampoIn("CausalTTC", CausalTTC);
            this.Ejecutor.AgregarCampoIn("CantEX_Old", CantEX_Old);
            this.Ejecutor.AgregarCampoIn("CantEX_New", CantEX_New);
            this.Ejecutor.AgregarCampoIn("CausalEX", CausalEX);
            this.Ejecutor.AgregarCampoIn("CantTRIP_Old", CantTRIP_Old);
            this.Ejecutor.AgregarCampoIn("CantTRIP_New", CantTRIP_New);
            this.Ejecutor.AgregarCampoIn("CausalTRIP", CausalTRIP);
            this.Ejecutor.AgregarCampoIn("CantPagoCOP_Old", CantPagoCOP_Old);
            this.Ejecutor.AgregarCampoIn("CantPagoCOP_New", CantPagoCOP_New);
            this.Ejecutor.AgregarCampoIn("CausalPagoCOP", CausalPagoCOP);
            this.Ejecutor.AgregarCampoIn("CantPagoUSD_Old", CantPagoUSD_Old);
            this.Ejecutor.AgregarCampoIn("CantPagoUSD_New", CantPagoUSD_New);
            this.Ejecutor.AgregarCampoIn("CausalPagoUSD", CausalPagoUSD);


            // se ejecuta el sp relacionado
            this.Ejecutor.ConexionEx("InsertValidaManual");

            //Esto se debe modificar para que solo ejecute la causal 69
            //Se debe apagar no de debe aplicar ninguna causal
            //OperacionVuelos_Validar_TT(idVuelo);
        }

        public List<OperacionVueloOtd> TraerVuelosConfirmados(int idAerolinea, int pagina, int registrosPorPagina, out int cantidadRegistros)
        {
            cantidadRegistros = 0;

            this.Ejecutor.AgregarCampoIn("_IdAerolinea", idAerolinea);
            this.Ejecutor.AgregarCampoIn("_Pagina", pagina);
            this.Ejecutor.AgregarCampoIn("_RegistrosPorPagina", registrosPorPagina);
            this.Ejecutor.AgregarCampoOut("_CantidadRegistros", DbType.Int32, cantidadRegistros);

            DataTable dt = this.Ejecutor.Conexion("OperacionVuelos_Confirmados");

            cantidadRegistros = this.Ejecutor.ObtenerCampoSalida<int>("_CantidadRegistros");

            List<OperacionVueloOtd> Vuelos = new List<OperacionVueloOtd>();
            foreach (DataRow dr in dt.Rows)
            {
                Vuelos.Add(Helper.ParseDataTableObject.drOperacionVueloOtd(dr));
            }

            return Vuelos;
        }

        public void ValidarVuelo(int id)
        {
            try
            {
                //validacion TT

                this.Ejecutor.LimpiarCampos();
                this.Ejecutor.AgregarCampoIn("_Id", id);
                this.Ejecutor.Conexion("OperacionVuelos_Validar");

                this.OperacionVuelos_Validar_TT(id);
                this.OperacionVuelosValidarExentos(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void OperacionVuelos_Validar_TT(int idVuelo)
        {
            try
            {
                this.Ejecutor.LimpiarCampos();
                this.Ejecutor.AgregarCampoIn("_Id", idVuelo);
                this.Ejecutor.Conexion("OperacionVuelos_Validar_TT");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void OperacionVuelos_AplicarLiquidacion(int idVuelo)
        {
            try
            {
                this.Ejecutor.LimpiarCampos();
                this.Ejecutor.AgregarCampoIn("_Id", idVuelo);
                this.Ejecutor.Conexion("FuncionAplicarLiquidacion");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void OperacionVuelosValidarExentos(int idVuelo)
        {
            try
            {
                this.Ejecutor.LimpiarCampos();
                this.Ejecutor.AgregarCampoIn("_Id", idVuelo);
                this.Ejecutor.Conexion("OperacionVuelos_Validar_Exe");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public List<OperacionVueloOtd> VuelosPendientesConfirmarObtenerTodos(int idAerolinea)
        {
            List<OperacionVueloOtd> Vuelos = new List<OperacionVueloOtd>();
            try
            {
                this.Ejecutor.AgregarCampoIn("_IdAerolinea", idAerolinea);
                DataTable dt = this.Ejecutor.Conexion("OperacionVuelos_VuelosPendientesConfirmarObtenerTodos");


                foreach (DataRow dr in dt.Rows)
                {
                    Vuelos.Add(Helper.ParseDataTableObject.drOperacionVueloOtd(dr));
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }

            return Vuelos;

        }

        public int VuelosPendientesProcesados(int idAerolinea, string fecha)
        {
            int cantidadVuelos = 0;
            try
            {
                this.Ejecutor.AgregarCampoIn("_IdAerolinea", idAerolinea);
                this.Ejecutor.AgregarCampoIn("_FechaVuelo", Convert.ToDateTime(fecha).ToString("yyyy-MM-dd"));
                DataTable dt = this.Ejecutor.Conexion("OperacionVuelos_VuelosPendientesProcesados");
                cantidadVuelos = dt.Rows.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }

            return cantidadVuelos;
        }

        public DataTable archivosCargaVuelo(int idVueloOp)
        {
            DataTable dt = new DataTable();
            try
            {
                this.Ejecutor.AgregarCampoIn("_Id", idVueloOp);
                dt = this.Ejecutor.Conexion("TraerOperacionVuelos");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }

            return dt;
        }

        public DataTable ArchivosTransitos(int idVueloOp)
        {
            DataTable dt = new DataTable();
            try
            {
                this.Ejecutor.AgregarCampoIn("_IdTrVuelo", idVueloOp);
                dt = this.Ejecutor.Conexion("TraerTransitosVuelos");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }

            return dt;
        }


        public void ActualizaEstadoVueloFirmado(int idOperacionVuelo)
        {
            // se recogen los parametros relacionados para el sp de insert validacion manual
            this.Ejecutor.AgregarCampoIn("_Id", idOperacionVuelo);
            // se ejecuta el sp relacionado
            this.Ejecutor.Conexion("ActualizarEstadoFirmadoVuelo");
        }
        public void ActualizarechazoVuelo(int id, string observacion)
        {
            // se recogen los parametros relacionados para el sp de insert validacion manual
            this.Ejecutor.AgregarCampoIn("_Id", id);
            this.Ejecutor.AgregarCampoIn("_observacion", observacion);
            // se ejecuta el sp relacionado
            this.Ejecutor.Conexion("GuardoRechazarTransito");
        }

        public void AtualizarCausalesTransito(int id, int contador, int CantidadTTC, int CantidadTTL)
        {

            try
            {
                this.Ejecutor.AgregarCampoIn("_Id", id);
                this.Ejecutor.AgregarCampoIn("_Contador", contador);
                this.Ejecutor.AgregarCampoIn("_NoCausal", "48");
                this.Ejecutor.AgregarCampoIn("CantidadTTC", CantidadTTC);
                this.Ejecutor.AgregarCampoIn("CantidadTTL", CantidadTTL);
                // se ejecuta el sp relacionado
                this.Ejecutor.Conexion("ActualizarCausalTransito");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }
        }


        public int ObtenerHallazgosVuelos(int idOperacionVuelo)
        {
            int cantidad = 0;
            this.Ejecutor.AgregarCampoIn("_IdVuelo", idOperacionVuelo);
            DataTable dt = this.Ejecutor.Conexion("TraerHallazgosPorVuelo");
            if (dt.Rows.Count > 0)
            {
                cantidad = Convert.ToInt32(dt.Rows[0]["CANT"].ToString());
            }
            return cantidad;

        }

        public string ObtenerObservacionRechazoTrans(int id)
        {
            string obsr = "";
            this.Ejecutor.AgregarCampoIn("_Id", id);
            DataTable dt = this.Ejecutor.Conexion("TraerObservacionRechazo");
            if (dt.Rows.Count > 0)
            {
                obsr = dt.Rows[0]["Observaciones"].ToString();
            }
            return obsr;

        }

        public bool NotificacionesUPD(int notify)
        {

            if (notify <= 0)
            {

                return false;
            }

            this.Ejecutor.AgregarCampoIn("_notify", notify);
            this.Ejecutor.Conexion("ActualizarNotificacion");

            return true;

        }

        public bool InsertaCargueVuelos(OperacionVueloOtd operacionVuelo, string contingencia)
        {
            // se recogen los parametros relacionados para el sp de insert validacion manual
            this.Ejecutor.AgregarCampoIn("_FechaVuelo", operacionVuelo.Fecha);
            this.Ejecutor.AgregarCampoIn("_MatriculaVuelo", operacionVuelo.Matricula);
            this.Ejecutor.AgregarCampoIn("_HoraVuelo", operacionVuelo.Hora);
            this.Ejecutor.AgregarCampoIn("_IdAerolinea", operacionVuelo.IdAerolinea);
            this.Ejecutor.AgregarCampoIn("_NumeroVuelo", operacionVuelo.Vuelo);
            this.Ejecutor.AgregarCampoIn("_IdVuelo", operacionVuelo.IdVuelo);
            this.Ejecutor.AgregarCampoIn("_TotalEmbarcados", operacionVuelo.TotalEmbarcados);
            this.Ejecutor.AgregarCampoIn("_ConfirmacionPasajeros", operacionVuelo.ConfirmacionPasajeros);
            this.Ejecutor.AgregarCampoIn("_ConfirmacionTransitos", operacionVuelo.ConfirmacionTransitos);
            this.Ejecutor.AgregarCampoIn("_ConfirmacionGenDec", operacionVuelo.ConfirmacionGenDec);
            this.Ejecutor.AgregarCampoIn("_CanfirmacionManifiesto", operacionVuelo.ConfirmacionManifiesto);
            this.Ejecutor.AgregarCampoIn("_ConfirmacionOperacion", operacionVuelo.ConfirmacionOperacion);
            this.Ejecutor.AgregarCampoIn("_INF", operacionVuelo.INF);
            this.Ejecutor.AgregarCampoIn("_TTL", operacionVuelo.TTL);
            this.Ejecutor.AgregarCampoIn("_TTC", operacionVuelo.TTC);
            this.Ejecutor.AgregarCampoIn("_EX", operacionVuelo.EX);
            this.Ejecutor.AgregarCampoIn("_TRIP", operacionVuelo.TRIP);
            this.Ejecutor.AgregarCampoIn("_PAX", operacionVuelo.PAX);
            this.Ejecutor.AgregarCampoIn("_PagoCOP", operacionVuelo.PagoCOP);
            this.Ejecutor.AgregarCampoIn("_PagoUSD", operacionVuelo.PagoUSD);
            this.Ejecutor.AgregarCampoIn("_TipoVuelo", operacionVuelo.Tipo);
            this.Ejecutor.AgregarCampoIn("_Destino", operacionVuelo.Destino);
            this.Ejecutor.AgregarCampoIn("_IdCargue", operacionVuelo.IdConsecutivoCargue);
            this.Ejecutor.AgregarCampoIn("_Validado", 0);
            this.Ejecutor.AgregarCampoIn("_ErroresAutomaticos", 0);
            if (contingencia.Equals("SI"))
            {
                this.Ejecutor.AgregarCampoIn("_Id_Daily", operacionVuelo.Id_Daily);
                this.Ejecutor.AgregarCampoIn("_EstadoProceso", operacionVuelo.EstadoProceso);
                this.Ejecutor.AgregarCampoIn("_bandera", "SI");
            }
            else if (contingencia.Equals("NO"))
            {
                this.Ejecutor.AgregarCampoIn("_Id_Daily", "");
                this.Ejecutor.AgregarCampoIn("_EstadoProceso", "");
                this.Ejecutor.AgregarCampoIn("_bandera", "NO");
            }

            // se ejecuta el sp relacionado
            this.Ejecutor.Conexion("CargarVuelos");
            return true;
        }

        public List<ExentoODT> TraerExento(string idVuelo, string FechaVuelo, bool Edit=false)
        {
            string SP = "TraerExentos";
            if (Edit==true)
            {
                SP = "TraerExentosEdit";
            }
            this.Ejecutor.AgregarCampoIn("_IdVuelo", idVuelo);
            this.Ejecutor.AgregarCampoIn("_FechaVuelo", Convert.ToDateTime(FechaVuelo));
            DataTable dt = this.Ejecutor.Conexion(SP);
            List<ExentoODT> listExento = new List<ExentoODT>();
            foreach (DataRow dr in dt.Rows)
            {
                ExentoODT exento = new ExentoODT();
                exento.Id = int.Parse(dr[0].ToString());  // int.Parse(dt.Rows[0]["IdExento"].ToString());
                exento.id_vuelo = dr[1].ToString();
                exento.Matricula = dr[2].ToString();
                exento.Fecha = Convert.ToDateTime(dr[3].ToString());
                exento.realiza_viaje = dr[4].ToString();
                exento.Pasajero = dr[5].ToString();
                listExento.Add(exento);
            }

            return listExento;
        }

        public void ActualizarSpExento(PasajeroOtd pasajeroOtd)
        {
            bool realizarVuelo = false;
            if (pasajeroOtd.realiza_viaje == "1")
            {
                realizarVuelo = true;
            }

            this.Ejecutor.AgregarCampoIn("_Id", pasajeroOtd.Id);
            this.Ejecutor.AgregarCampoIn("_Realiza_Vuelo", realizarVuelo);
            this.Ejecutor.Conexion("ActualizarExento");
        }

        public DataTable TraerInformeCobro(int idAerolinea, string numVuelo, DateTime fechaIni, DateTime fechaFin)
        {
            DateTime fechaInicial = DateTime.Now;
            fechaInicial = new DateTime(fechaIni.Year, fechaIni.Month, fechaIni.Day, 0, 0, 0);

            DateTime fechaFinal = DateTime.Now;
            fechaFinal = new DateTime(fechaFin.Year, fechaFin.Month, fechaFin.Day, 0, 0, 0);


            this.Ejecutor.AgregarCampoIn("_IdAerolinea", idAerolinea);
            this.Ejecutor.AgregarCampoIn("_NumVuelo", numVuelo);
            this.Ejecutor.AgregarCampoIn("_FechaIni", fechaInicial);
            this.Ejecutor.AgregarCampoIn("_FechaFin", fechaFinal);
            DataTable dt = this.Ejecutor.Conexion("TraerInformeCobro");

            return dt;
        }

        public void GuardarAuditoriaCargue(string usuario, string Evento, int Idvuelo)
        {
            this.Ejecutor.AgregarCampoIn("_IdVuelo", Idvuelo);
            this.Ejecutor.AgregarCampoIn("_Usuario", usuario);
            this.Ejecutor.AgregarCampoIn("_Evento", Evento);
            this.Ejecutor.Conexion("Insertar_auditoria");
        }
        public DataTable TraerInformeTickets(int idAerolinea, DateTime fechaIni, DateTime fechaFin, int tipoticket)
        {
            this.Ejecutor.AgregarCampoIn("_IdAerolinea", idAerolinea);
            this.Ejecutor.AgregarCampoIn("_FechaIni", fechaIni);
            this.Ejecutor.AgregarCampoIn("_FechaFin", fechaFin);
            this.Ejecutor.AgregarCampoIn("_TipoTicket", tipoticket);

            DataTable dt = this.Ejecutor.Conexion("TraerInformeTickets");
            return dt;


        }


        public bool EjecutarTripulantes(IList<TripulantesOTD> tripulatesOtd)
        {

            foreach (var tripulante in tripulatesOtd)
            {
                try
                {

                    Store.Helper.Ejecutor ejecutor1 = new Store.Helper.Ejecutor(this._config);

                    ejecutor1.AgregarCampoIn("_NomTripulante", (tripulante.NomTripulante == null ? "" : tripulante.NomTripulante.Trim()));
                    ejecutor1.AgregarCampoIn("_LicTripulantes", (tripulante.LicTripulante == null ? "" : tripulante.LicTripulante.Trim()));
                    ejecutor1.AgregarCampoIn("_FunTripulante", (tripulante.FunTripulante == null ? "" : tripulante.FunTripulante.Trim()));
                    ejecutor1.AgregarCampoIn("_CodAreolinea", tripulante.CodAreolinea);
                    ejecutor1.Conexion("Insertar_Tripulantes");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);

                }
            }


            return true;
        }

        public bool EjecutarEliminarTripulantes(int idAreolinea)
        {

            try
            {
                this.Ejecutor.AgregarCampoIn("_CodAreolinea", idAreolinea);
                this.Ejecutor.Conexion("Eliminar_Tripulantes");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }


            return true;
        }

        public List<AerolineaOtd> HorarioAerolineasObtenerIdAerolinea(int idAerolinea)
        {
            List<AerolineaOtd> Vuelos = new List<AerolineaOtd>();
            try
            {
                this.Ejecutor.AgregarCampoIn("_IdAerolinea", idAerolinea);
                DataTable dt = this.Ejecutor.Conexion("HorarioAerolineasIdAerolinea");

                foreach (DataRow dr in dt.Rows)
                {
                    Vuelos.Add(Helper.ParseDataTableObject.drAerolineaOtd(dr));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }

            return Vuelos;

        }


    }
}