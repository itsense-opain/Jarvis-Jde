using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Opain.Jarvis.Infraestructura.Datos.EntidadesIntegracion;
namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public static class ActualizaTransito
    {
        public static string Consulta(DatosTransito datosTransito, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            /*string consulta = "INSERT INTO \"%pdta%\".\"FQ70594B\" (PTQ70IDP, PTLNID, PTQ70IDV, PTQ70CVO, PTQ70TVL, PTQ70FLR, PTQ70HLR, PTAN8, PTQ70TTR, PTQ70TTP, PTUSER, PTUPMJ, PTUPMT,PTQ70IDA,PTQ70IATA,PTQ70OACI,PTDSC1,PTDSC2,PTQ70DES,PTA,PTURCD,PTURDT,PTURAT,PTURAB,PTURRF) " +
            "VALUES ('" + datosTransito.Id_Pasajero + "','" + datosTransito.Id_Linea.ToString() + "','" + datosTransito.Id_Vuelo.ToString() + "', " +
            "'" + datosTransito.Numero_Vuelo.ToString() + "','" + datosTransito.Tipo_Vuelo.ToString() + "','" + ConvertToJulian(datosTransito.Fecha_Llegada).ToString() + "', " +
            "'" + datosTransito.Hora_Llegada.Replace(":","") + "','" + datosTransito.Aerolinea.ToString() + "','" + datosTransito.Tipo_Transito.ToString() + "','" + datosTransito.Total_Pasajeros.ToString() + "',(case when length(USER)>9 then substr(user,1,9) else user end),'" + ConvertToJulian(DateTime.Now).ToString() + "','" + ConvertToJulian(DateTime.Now).ToString() + "','" + datosTransito.id_aeropuerto + "','" + datosTransito.IATACODE + "','" + datosTransito.OACICODE + "','" + datosTransito.descripcion + "','" + datosTransito.descripcion2 + "',' ', 'N','','0','0','0',' ' )";
           */

            string consulta = "INSERT INTO \"%pdta%\".\"FQ70594B\" (PTQ70IDP,PTLNID,PTQ70IDV,PTQ70CVO,PTQ70TVL,PTQ70FLR,PTQ70HLR,PTQ70IDA,PTQ70IATA,PTQ70OACI,PTAN8,PTQ70TTR,PTQ70TTP,PTDSC1,PTDSC2,PTQ70DES,PTA,PTURCD,PTURDT,PTURAT,PTURAB,PTURRF,PTUSER,PTPID,PTJOBN,PTUPMJ,PTUPMT) " +
                " VALUES ('" + datosTransito.Id_Pasajero + "','" + datosTransito.Id_Linea.ToString() + "','" + datosTransito.Id_Vuelo.ToString() + "','" + datosTransito.Numero_Vuelo.ToString() + "','" + datosTransito.Tipo_Vuelo.ToString() + "','" + ConvertToJulian(datosTransito.Fecha_Llegada).ToString() + "','" + datosTransito.Hora_Llegada.Replace(":", "") + "','" + datosTransito.id_aeropuerto + "','" + datosTransito.IATACODE + "','" + datosTransito.OACICODE + "','" + datosTransito.Aerolinea.ToString() + "','" + datosTransito.Tipo_Transito.ToString() + "','" + datosTransito.Total_Pasajeros.ToString() + "','" + datosTransito.descripcion + "','" + datosTransito.descripcion2 + "','','Y',null,'0','0','0','','JARVIS','PQ70594A','SOPAIN38','" + ConvertToJulian(DateTime.Now).ToString() + "','" + DateTime.Now.ToString("HHmmss") + "')";

            consulta = consulta.Replace("%pdta%", squemaData);

            return consulta;



        }
        public static double ConvertToJulian(DateTime Date)
        {
            double Year = Date.Year;
            double dato = (Year - 1900) * 1000 + Date.DayOfYear;
            return dato;
        }
    }
}
