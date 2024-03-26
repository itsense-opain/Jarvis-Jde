using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Opain.Jarvis.Infraestructura.Datos.EntidadesIntegracion;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public static class ActualizaTotales
    {
        public static string ActualizaTotalesJDE(VuelosValidados vuelosValidados, IConfiguration config)
        {

            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string Update = " UPDATE  \"%pdta%\".\"FQ70594A\"  SET PEQ70TTA ='" + vuelosValidados.TotalEmbarcados + "', PEQ70TTP ='" + vuelosValidados.Adultos + "', PEQ70TTI ='" + vuelosValidados.Infantes + "',PEQ70EST = '06',PEQ70FRL='" + ConvertToJulian(vuelosValidados.FechaVuelo) + "', PEQ70EC='" + vuelosValidados.Tripulacion + "', " +
            "PEEV02 = '1',PEEV01 = '1',PEQ70TTC ='" + vuelosValidados.PagoCOP + "',PEQ70TTU = '" + vuelosValidados.PagoUSD + "',PEQ70TRP = '" + vuelosValidados.TotalLinea + "',PEQ70TRF = '" + vuelosValidados.TotalConexion + "',PEUSER = (case when length(USER)>9 then substr(user,1,9) else user end),PEUPMJ =  '" + ConvertToJulian(DateTime.Now) + "'" +
            ",PEUPMT = '" + ConvertToJulian(DateTime.Now) + "' " + 
            " WHERE TRIM(PEQ70IDD) = '"+vuelosValidados.Id_daily.Trim()+"' and rownum = 1";
            //" WHERE PEQ70IDD = (SELECT DOQ70IDD from \"%pdta%\".\"FQ70591H\" where DOQ70CM = '" + vuelosValidados.Matricula + "' AND DOAN8 = '" + vuelosValidados.Aerolinea + "' AND DOQ70CVS =  '" + vuelosValidados.NumeroVuelo + "' AND ((SUBSTR((LPAD(DOQ70HSP, 4, '0')),1,2) || ':' || SUBSTR(LPAD(DOQ70HSP, 4, '0'),3,2))) = '" + vuelosValidados.HoraVuelo + "'" +
            //"AND DOQ70FSP = '" + ConvertToJulian(vuelosValidados.FechaVuelo) + " ' and rownum = 1);";
            
            Update = Update.Replace("%pdta%", squemaData);

            return Update;
        }

        public static string ConsultaSelect(VuelosValidados vuelosValidados, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consulta = " SELECT PEQ70IDD as Id_daily,PEQ70IDP as Id_pasajero, PEQ70TTA as Adultos,PEQ70TTI as Infantes, PEQ70EC as Tripulacion, PEQ70TTC as PagoCOP,PEQ70TTU as PagoUSD, PEQ70TRP as TotalLinea, PEQ70TRF as TotalConexion, PEQ70IDA as IDAEROPUERTO,PEQ70IATA AS IATACODE,PEQ70OACI AS OACICODE, PEQ70IDV as IdVuelo from  \"%pdta%\".\"FQ70594A\"" +
            " WHERE TRIM(PEQ70IDD) = '" + vuelosValidados.Id_daily.Trim() + "' and rownum = 1";

            //" WHERE PEQ70IDD = (SELECT DOQ70IDD from \"%pdta%\".\"FQ70591H\" where DOQ70CM = '" + vuelosValidados.Matricula + "' AND DOAN8 = '" + vuelosValidados.Aerolinea + "' AND DOQ70CVS =  '" + vuelosValidados.NumeroVuelo + "' AND ((SUBSTR((LPAD(DOQ70HSP, 4, '0')),1,2) || ':' || SUBSTR(LPAD(DOQ70HSP, 4, '0'),3,2))) = '" + vuelosValidados.HoraVuelo + "'" +
            //"AND DOQ70FSP = '" + ConvertToJulian(vuelosValidados.FechaVuelo) + " ' and rownum = 1);";
            consulta = consulta.Replace("%pdta%", squemaData);

            return consulta;
        }


        public static string ConsultaSelecTransito(string numeroVuelo , DateTime Date, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            DateTime IntervaloInicial = Date.AddMinutes(-119);
            DateTime IntervaloFinal = Date.AddMinutes(119);

            string HoraIni = int.Parse(IntervaloInicial.ToString("HH:mm").Replace(":", "")).ToString();
            string HoraFin = int.Parse(IntervaloFinal.ToString("HH:mm").Replace(":", "")).ToString();

            string consulta = " SELECT DISTINCT VL.MFQ70IDV AS IDV_VUELO,D.DOQ70CVL AS NUM_VUELO_LLEGANDO, "+
                /*" D.DOQ70TVS AS TIPO_VUELO," +*/
                " D.DOQ70TVL AS TIPO_VUELO," +
                " D.DOQ70FLR AS FECHA_LLEGADA,D.DOQ70HLP AS HORA_LLEGADA, " +
                /*SUBSTR((LPAD(D.DOQ70HLP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(D.DOQ70HLP, 4, '0'),3,2)) AS HORA_LLEGADA,*/
                " ORG.MAQ70IDA AS IDA_LLEGADA,ORG.MAQ70IATA AS CODIGO_IATA_LLE," +
                " ORG.MAQ70OACI AS CODIGO_OACI_LLE,D.DOAN8 AS ID_AEROLINEA_LLEGADA," +
                " D.DOQ70IDD AS ID_DAILY,D.DOQ70CM AS MATRICULA,D.DOQ70ORG AS ORIGEN," +
                " AA.MAALPH AS AEROLINEA,D.DOQ70IDVS as ID_VUELO,D.DOQ70NVS AS NATURALEZA" +
                " From \"%pdta%\".\"FQ70591H\" D" +
                " LEFT JOIN \"%pdta%\".\"FQ70591C\" AA ON AA.MAAN8 = D.DOAN8" +
                " LEFT JOIN \"%pdta%\".\"FQ70590A\" ORG ON ORG.MAQ70IATA = D.DOQ70ORG" +
                " LEFT JOIN \"%pdta%\".\"FQ70591E\" VL ON D.DOQ70CVL = VL.MFQ70CVO" +
                " LEFT JOIN \"%crpctl%\".\"F0005\" CAT ON CAT.DRKY = D.DOQ70NVS" +
                " WHERE MFQ70TO = 'LLE'" +
                " AND DOQ70CVL = '" + numeroVuelo.Trim() + "' AND " +
                /*" DOQ70FLR BETWEEN '" + ConvertToJulian(IntervaloInicial) + "' and  '" + ConvertToJulian(IntervaloFinal) + "' " +*/
                "  concat(D.DOQ70FLR, SUBSTR((10000 + DOQ70HLP),2,4)) BETWEEN " +
                " concat('"+ ConvertToJulian(IntervaloInicial) + "', SUBSTR((10000 + '"+ HoraIni + "'), 2, 4)) AND " +
                " concat('"+ ConvertToJulian(IntervaloFinal)  +  "', SUBSTR((10000 + '"+ HoraFin + "'), 2, 4)) " +
                " Order by DOQ70FLR, DOQ70HLP asc ";               
            
            consulta = consulta.Replace("%pdta%", squemaData);

            string squemaControl = string.Empty;
            squemaControl = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Control", config);
            consulta = consulta.Replace("%crpctl%", squemaControl);

            return consulta;
        }
        public static string ConsultaSelecTransitoDatosLlegada(int daily,string numeroVuelo, DateTime Date, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consulta = " SELECT DISTINCT D.DOQ70IDD AS ID_DAILY,PA2.PEQ70IDP AS ID_PAX,PA2.PEQ70FIT AS FECHA_LLEGADA,D.DOQ70CM AS MATRICULA,/*SUBSTR((LPAD(D.DOQ70HLP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(D.DOQ70HLP, 4, '0'),3,2)) AS HORA_LLEGADA,*/D.DOQ70ORG AS ORIGEN,ORG.MAQ70IATA AS CODIGO_IATA_LLE,ORG.MAQ70OACI AS CODIGO_OACI_LLE,ORG.MACTY1 AS ORIGEN_DES,D.DOAN8 AS ID_AEROLINEA_LLEGADA,AA.MAALPH AS AEROLINEA,D.DOQ70FLP AS FECHA_SALIDA,/*SUBSTR((LPAD(D.DOQ70HSP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(D.DOQ70HSP, 4, '0'), 3, 2))  AS HORA_SALIDA,*/PA2.PEQ70CVO AS NUM_VUELO_LLEGANDO,D.DOQ70CVS AS NUM_VUELO_SALIENDO,D.DOQ70DES AS DESTINO,DES.MACTY1 AS DESTINODESC,DES.MAQ70IATA AS CODIGO_IATA_DES,DES.MAQ70OACI AS CODIGO_OACI_DES,D.DOQ70IDVS as ID_VUELO,D.DOQ70TVS AS TIPO_VUELO,D.DOQ70NVS AS NATURALEZA" +
                              " From \"%pdta%\".\"FQ70591H\" D" +
                              " INNER JOIN \"%pdta%\".\"FQ70594A\" PA1 ON D.DOQ70IDD = PA1.PEQ70IDD" +
                              " JOIN \"%pdta%\".\"FQ70591C\" AA ON AA.MAAN8 = D.DOAN8" +
                              " JOIN \"%pdta%\".\"FQ70590A\" DES ON DES.MAQ70IATA = D.DOQ70DES" +
                              " JOIN \"%pdta%\".\"FQ70590A\" ORG ON ORG.MAQ70IATA = D.DOQ70ORG" +
                              " JOIN \"%crpctl%\".\"F0005\" CAT ON CAT.DRKY = D.DOQ70NVS" +
                              " JOIN \"%pdta%\".\"FQ70594A\" PA2 ON PA1.PEQ70IDP = PA2.PEQ70IDP" +
                              " WHERE D.DOQ70CVS = '" + numeroVuelo.Trim() + "' and  D.DOQ70FSP = '" + ConvertToJulian(Date) + "'";

            //" SELECT DISTINCT D.DOQ70IDD AS DAILY, D.DOQ70IDVS AS ID_NVUELO,D.DOAN8 AS AEROLINEA,D.DOQ70CVL AS N_VUELO,P.PEQ70IDP as ID_PASAJERO,D.DOQ70TVS AS TIPO_VUELO,D.DOQ70ORG AS ORIGEN,D.DOQ70FLP AS FECHA_LLEGADA_POSICION,D.DOQ70HLP AS HORA_LLEGADA_POSICION,D.DOQ70IDVS as ID_VUELO,E.MFQ70TO AS TIPO_DE_OPERACION,ORG.MAQ70IDA AS ID_ORIGEN,ORG.MACTY1 AS ORIGEN_DES,ORG.MAQ70IATA AS ORIGEN_IATA,ORG.MAQ70OACI AS ORGIEN_OACI,D.DOQ70CM AS MATRICULA,D.DOQ70FSP AS FECHA_SALIDA,D.DOQ70HSP AS HORA_SALIDA,D.DOQ70CVS AS NUM_VUELO_SALIENDO,D.DOQ70DES AS DESTINO,DES.MACTY1 AS DESTINODESC,D.DOQ70NVS AS NATURALEZA  " +
            //                 " From \"%pdta%\".\"FQ70591H\" D" +
            //                 " LEFT JOIN \"%pdta%\".\"FQ70591C\" AA ON AA.MAAN8 = D.DOAN8" +
            //                 " LEFT JOIN \"%crpctl%\".\"F0005\" CAT ON CAT.DRKY = D.DOQ70NVS " +
            //                 " LEFT JOIN  \"%pdta%\".\"FQ70591E\" E ON E.MFQ70IDV = D.DOQ70IDVL" +
            //                 " LEFT JOIN  \"%pdta%\".\"FQ70590A\" DES ON DES.MAQ70IATA = D.DOQ70DES" +
            //                 " LEFT JOIN  \"%pdta%\".\"FQ70590A\" ORG ON ORG.MAQ70IATA = D.DOQ70ORG" +
            //                 " LEFT JOIN  \"%pdta%\".\"FQ70594A\" P ON  P.PEQ70IDD = D.DOQ70IDD" +
            //                 //" where D.DOQ70IDD = " + daily + "" +
            //                 " where D.DOQ70CVL = '" + numeroVuelo.Trim() + "' and  D.DOQ70FSP = '" + ConvertToJulian(Date) + "'" +
            //                 " and rownum = 1";

            consulta = consulta.Replace("%pdta%", squemaData);

            string squemaControl = string.Empty;
            squemaControl = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Control", config);
            consulta = consulta.Replace("%crpctl%", squemaControl);

            return consulta;
        }

        public static string ConsultaSelecDestino(string nombreIATA,string nombreOACI, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consulta = " select MAQ70IDA AS ID, MAQ70IATA AS COD_IATA,MAQ70OACI AS COD_OACI  from \"%pdta%\".\"FQ70590A\" " +
                             " where MAQ70IATA = '" + nombreIATA + "'" +
                             " OR MAQ70OACI = '" + nombreOACI + "'" +
                             " and rownum = 1";
            
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
