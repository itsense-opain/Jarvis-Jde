using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Opain.Jarvis.Servicios.Store.Helper
{
    public static class ParseDataTableObject
    {
        public static OperacionVueloOtd drOperacionVueloOtd(DataRow dr)
        {
            OperacionVueloOtd ope = new OperacionVueloOtd();

            ope.Tipo = dr["TipoVuelo"].ToString();
            ope.Id = int.Parse(dr["Id"].ToString());
            ope.Destino = dr["Destino"].ToString();
            ope.NombreAerolinea = dr["NombreAerolinea"].ToString();
            ope.Matricula = dr["MatriculaVuelo"].ToString();
            ope.Vuelo = dr["NumeroVuelo"].ToString();
            ope.Fecha = DateTime.Parse(dr["FechaVuelo"].ToString());
            ope.Hora = dr["HoraVuelo"].ToString();
            ope.PDFPasajeros = int.Parse(dr["PDFPasajeros"].ToString());
            ope.ArchivoPasajeros = dr["ArchivoPasajeros"].ToString();
            ope.ArchivoTransito = dr["ArchivoTransito"].ToString();
            ope.ArchivoGendec = dr["ArchivoGendec"].ToString();
            ope.ArchivoManifiesto = dr["ArchivoManifiesto"].ToString();
            ope.IdConsecutivoCargue = int.Parse(dr["IdCargue"].ToString());
            //Adicionar informacion
            ope.INF=int.Parse(dr["INF"].ToString());
            ope.TTL = int.Parse(dr["TTL"].ToString());
            ope.TTC = int.Parse(dr["TTC"].ToString());
            ope.EX = int.Parse(dr["EX"].ToString());
            ope.TRIP = int.Parse(dr["TRIP"].ToString());
            ope.PAX = int.Parse(dr["PAX"].ToString());
            ope.ConfirmacionPasajeros = int.Parse(dr["ConfirmacionPasajeros"].ToString());
            ope.ConfirmacionTransitos = int.Parse(dr["ConfirmacionTransitos"].ToString());
            ope.ConfirmacionManifiesto = int.Parse(dr["CanfirmacionManifiesto"].ToString());
            ope.ConfirmacionGenDec = int.Parse(dr["ConfirmacionGenDec"].ToString());
            ope.PagoCOP = int.Parse(dr["PagoCOP"].ToString());
            ope.PagoUSD = int.Parse(dr["PagoUSD"].ToString());

            ///Informacion de liquidados
            if (dr["INF_LIQ"].ToString() != null && dr["INF_LIQ"].ToString() != string.Empty)
            {
                ope.INF_LIQ = int.Parse(dr["INF_LIQ"].ToString());
                ope.TTL_LIQ = int.Parse(dr["TTL_LIQ"].ToString());
                ope.TTC_LIQ = int.Parse(dr["TTC_LIQ"].ToString());
                ope.EX_LIQ = int.Parse(dr["EX"].ToString());
                ope.TRIP_LIQ = int.Parse(dr["TRIP_LIQ"].ToString());
                ope.PAX_LIQ = int.Parse(dr["PAX_LIQ"].ToString());
                ope.PAGOCOP_LIQ = int.Parse(dr["PAGOCOP_LIQ"].ToString());
                ope.PAGOUSD_LIQ = int.Parse(dr["PAGOUSD_LIQ"].ToString());
            }

            return ope;
        }

        public static AerolineaOtd drAerolineaOtd(DataRow dr)
        {
            AerolineaOtd _aerolineaOtd = new AerolineaOtd();

            _aerolineaOtd.CantidadUsuarios = int.Parse(dr["CantidadUsuarios"].ToString());
            _aerolineaOtd.Codigo = dr["Codigo"].ToString();
            //_aerolineaOtd.HorarioAerolinea =  IList<HorarioAerolineaOtd>() dr["HorarioAerolinea"].ToString();
            _aerolineaOtd.Id = int.Parse(dr["NombreAerolinea"].ToString());
            _aerolineaOtd.IdEstado = dr["IdEstado"].ToString();
            _aerolineaOtd.Nombre = dr["Nombre"].ToString();
            _aerolineaOtd.PDFPasajeros = dr["PDFPasajeros"].ToString();
            _aerolineaOtd.Sigla = dr["Sigla"].ToString();
            //_aerolineaOtd.Vuelos = dr["NumeroVuelo"].ToString();

            return _aerolineaOtd;
        }

        public static NovedadOtd drNovedadOtd(DataRow dr)
        {
            NovedadOtd NovErr = new NovedadOtd();

            NovErr.Id = int.Parse(dr["Id"].ToString());
            NovErr.TipoError = dr["TipoError"].ToString();
            NovErr.Error = dr["Error"].ToString();
            NovErr.FechaHora = DateTime.Parse(dr["Fecha"].ToString());
            NovErr.FechaVuelo = DateTime.Parse(dr["FechaVuelo"].ToString());
            NovErr.Valores = dr["Valores"].ToString();
            NovErr.ValoresNuevos = dr["ValoresNuevos"].ToString();
            NovErr.NumeroVuelo = dr["IdVuelo"].ToString();
            NovErr.TipoValidacion =Convert.ToInt32(dr["TipoValidacion"].ToString());
            NovErr.CodCausal = dr["CodCausal"].ToString();
            NovErr.DescCausal = dr["DescCausal"].ToString();
            return NovErr;
        }

        public static NovedadesAgrupadasOtd drNovedadesAgrupadasOtd(DataRow dr)
        {
            NovedadesAgrupadasOtd _NovedadesAgrupadasOtd = new NovedadesAgrupadasOtd();

            _NovedadesAgrupadasOtd.FechaVuelo = DateTime.Parse(dr["FechaVuelo"].ToString());
            _NovedadesAgrupadasOtd.CantidadVuelos = int.Parse(dr["CantidadVuelos"].ToString());
            _NovedadesAgrupadasOtd.NovedadesCargue = int.Parse(dr["NovedadesCargue"].ToString());
            _NovedadesAgrupadasOtd.NovedadesProcesos = int.Parse(dr["NovedadesProcesos"].ToString());
            _NovedadesAgrupadasOtd.Exitoso = Convert.ToBoolean(dr["Exitoso"]); ;
            _NovedadesAgrupadasOtd.Procesados = int.Parse(dr["Procesados"].ToString());

            return _NovedadesAgrupadasOtd;
        }


        public static NotificacionODT drNotificacionOdt(DataRow dr)
        {
            NotificacionODT Notify = new NotificacionODT();

            Notify.Id = int.Parse(dr["Id"].ToString());
            Notify.titulo = dr["titulo"].ToString();
            Notify.descripcion = dr["descripcion"].ToString();
            Notify.fecha = dr["fecha"].ToString();
            Notify.id_item = dr["id_item"].ToString();
            Notify.id_responsable = dr["id_responsable"].ToString();
            Notify.rol_responsable = dr["rol_responsable"].ToString();
            Notify.id_aerolinea = dr["id_aerolinea"].ToString();

            return Notify;
        }        
    }
}