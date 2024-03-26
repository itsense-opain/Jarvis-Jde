using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public static class TraeVuelos
    {
        public static string ConsultaJDE(DateTime FechaBusqueda, DateTime FechaAnterior, List<string> Iddaily, IConfiguration config)
        {

            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            //  FechaAnterior = new DateTime(2020, 11, 20);

            string FJulianaAnt = ConvertToJulian(FechaAnterior).ToString();
            string FJulianaAct = ConvertToJulian(FechaAnterior.AddDays(1)).ToString();
            // string FJulianaAnt = "119297";

            string consulta = " SELECT DISTINCT " +
            //"P.PEQ70IDD AS ID_DAILY, " +
            "D.DOQ70IDD AS ID_DAILY, " +
            "P.PEQ70IDP as ID_PASAJERO, " +
            "D.DOQ70FLP AS FECHA_LLEGADA, " +
            "D.DOQ70CM AS MATRICULA, " +
            "(SUBSTR((LPAD(D.DOQ70HLP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(D.DOQ70HLP, 4, '0'), 3, 2))  AS HORA_LLEGADA, " +
            "D.DOQ70ORG AS ORIGEN, ORG.MACTY1 AS ORIGEN_DES, D.DOAN8 AS ID_AEROLINEA_LLEGADA, " +
            "A.MAALPH AS AEROLINEA, D.DOQ70FSP AS FECHA_SALIDA, " +
            "(SUBSTR((LPAD(D.DOQ70HSP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(D.DOQ70HSP, 4, '0'), 3, 2))  AS HORA_SALIDA, " +
            "D.DOQ70CVL AS NUM_VUELO_LLEGANDO, " +
            "D.DOQ70CVS AS NUM_VUELO_SALIENDO,D.DOQ70DES AS DESTINO, DES.MACTY1 AS DESTINODESC, P.PEQ70IDV as ID_VUELO, " +
            "P.PEQ70TV AS TIPO_VUELO, D.DOQ70NVS AS NATURALEZA " +
            "From %pdta%.FQ70594A P " +            
            "LEFT JOIN %pdta%.FQ70591H D ON D.DOQ70IDD = P.PEQ70IDD " +            
            "LEFT JOIN %pdta%.FQ70591C A ON A.MAAN8 = D.DOAN8 " +
            "LEFT JOIN %pdta%.FQ70590A DES ON DES.MAQ70IATA = D.DOQ70DES " +
            "LEFT JOIN %pdta%.FQ70590A ORG ON ORG.MAQ70IATA = D.DOQ70ORG " +
            "LEFT JOIN %crpctl%.F0005  CAT ON CAT.DRKY = D.DOQ70NVS " +
            "WHERE D.DOQ70FSP = '" + FJulianaAnt + "' and P.PEQ70EST IN ('01','02','04','08') " +            
            //"WHERE P.PEQ70IDD IN ('437538','438528') and P.PEQ70EST IN ('01','02','04') " +

            //"AND TRIM(D.DOQ70NVS) in (SELECT TRIM(CAT2.DRKY) FROM  %crpctl%.F0005 CAT2 WHERE CAT2.DRSY = 'Q70' AND CAT2.DRRT = 'NV' AND CAT2.DRDL02 LIKE '%PAX%')";
            "AND TRIM(D.DOQ70NVS) in ('A','C','F','M','N','O','P','S','T','TR','V') ";
            /*"AND P.PEQ70IDD = 980118";*/

            consulta = consulta + " union SELECT DISTINCT " +

            //consulta = "SELECT DISTINCT " +
            //"P.PEQ70IDD AS ID_DAILY, " +
            "D.DOQ70IDD AS ID_DAILY, " +
            "P.PEQ70IDP as ID_PASAJERO, " +
            "D.DOQ70FLP AS FECHA_LLEGADA, " +
            "D.DOQ70CM AS MATRICULA, " +
            "(SUBSTR((LPAD(D.DOQ70HLP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(D.DOQ70HLP, 4, '0'), 3, 2))  AS HORA_LLEGADA, " +
            "D.DOQ70ORG AS ORIGEN, ORG.MACTY1 AS ORIGEN_DES, D.DOAN8 AS ID_AEROLINEA_LLEGADA, " +
            "A.MAALPH AS AEROLINEA, D.DOQ70FSP AS FECHA_SALIDA, " +
            "(SUBSTR((LPAD(D.DOQ70HSP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(D.DOQ70HSP, 4, '0'), 3, 2))  AS HORA_SALIDA, " +
            "D.DOQ70CVL AS NUM_VUELO_LLEGANDO, " +
            "D.DOQ70CVS AS NUM_VUELO_SALIENDO,D.DOQ70DES AS DESTINO, DES.MACTY1 AS DESTINODESC, P.PEQ70IDV as ID_VUELO, " +
            "P.PEQ70TV AS TIPO_VUELO, D.DOQ70NVS AS NATURALEZA " +
             //"From %pdta%.FQ70594A P " +
             "From %pdta%.FQ70591H D " +
            //"LEFT JOIN %pdta%.FQ70591H D ON D.DOQ70IDD = P.PEQ70IDD " +
            "LEFT JOIN %pdta%.FQ70594A P ON P.PEQ70IDD = D.DOQ70IDD " +
            "LEFT JOIN %pdta%.FQ70591C A ON A.MAAN8 = D.DOAN8 " +
            "LEFT JOIN %pdta%.FQ70590A DES ON DES.MAQ70IATA = D.DOQ70DES " +
            "LEFT JOIN %pdta%.FQ70590A ORG ON ORG.MAQ70IATA = D.DOQ70ORG " +
            "LEFT JOIN %crpctl%.F0005  CAT ON CAT.DRKY = D.DOQ70NVS " +
            //"WHERE D.DOQ70FSP = '" + FJulianaAnt + "' and P.PEQ70EST IN ('01','02','04','08') " +
            "WHERE D.DOUPMJ = '" + FJulianaAct + "'";
           //+
           //" and P.PEQ70EST IN  ('01','02','03','04','05','06','07','08')  " +
           //" WHERE P.PEQ70IDD IN ('437538','438528') and P.PEQ70EST IN ('01','02','04') " +

           //"AND TRIM(D.DOQ70NVS) in (SELECT TRIM(CAT2.DRKY) FROM  %crpctl%.F0005 CAT2 WHERE CAT2.DRSY = 'Q70' AND CAT2.DRRT = 'NV' AND CAT2.DRDL02 LIKE '%PAX%')";
           //" AND TRIM(D.DOQ70NVS) in ('A','C','F','M','N','O','P','S','T','TR','V') ";
            /*"AND P.PEQ70IDD = 980118";*/

            /*
            consulta = consulta + "  UNION SELECT DISTINCT D.DOQ70IDD AS ID_DAILY, 0 AS ID_PASAJERO, D.DOQ70FLP AS FECHA_LLEGADA, " +
            "D.DOQ70CM AS MATRICULA,(SUBSTR((LPAD(D.DOQ70HLP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(D.DOQ70HLP, 4, '0'), 3, 2)) AS HORA_LLEGADA,   D.DOQ70ORG AS ORIGEN,ORG.MACTY1 AS ORIGEN_DES, " +
            "D.DOAN8 AS ID_AEROLINEA_LLEGADA, AA.MAALPH AS AEROLINEA,D.DOQ70FSP AS FECHA_SALIDA,(SUBSTR((LPAD(D.DOQ70HSP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(D.DOQ70HSP, 4, '0'), 3, 2))  AS HORA_SALIDA," +
            "D.DOQ70CVL AS NUM_VUELO_LLEGANDO, D.DOQ70CVS AS NUM_VUELO_SALIENDO,D.DOQ70DES AS DESTINO,DES.MACTY1 AS DESTINODESC,D.DOQ70IDVS as ID_VUELO,D.DOQ70TVS AS TIPO_VUELO,D.DOQ70NVS AS NATURALEZA " +
            "From %pdta%.FQ70591H D LEFT JOIN %pdta%.FQ70591C AA ON AA.MAAN8 = D.DOAN8 LEFT JOIN %pdta%.FQ70590A DES ON DES.MAQ70IATA = D.DOQ70DES LEFT JOIN %pdta%.FQ70590A ORG ON ORG.MAQ70IATA = D.DOQ70ORG " +
            "LEFT JOIN %crpctl%.F0005  CAT ON CAT.DRKY = D.DOQ70NVS  WHERE DOQ70DES LIKE '%HAN%' AND DOQ70FSP = '" + FJulianaAnt + "' AND DOQ70ORG NOT LIKE '%HAN%' AND DOQ70NVL<> 'C' ";
            */
    
            //consulta = " SELECT DISTINCT P.PEQ70IDD AS ID_DAILY,P.PEQ70IDP as ID_PASAJERO ,D.DOQ70FLP AS FECHA_LLEGADA, D.DOQ70CM AS MATRICULA," +
            //" (SUBSTR((LPAD(D.DOQ70HLP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(D.DOQ70HLP, 4, '0'), 3, 2))  AS HORA_LLEGADA, D.DOQ70ORG AS ORIGEN, " +
            //" ORG.MACTY1 AS ORIGEN_DES,D.DOAN8 AS ID_AEROLINEA_LLEGADA, A.MAALPH AS AEROLINEA, D.DOQ70FSP AS FECHA_SALIDA, " +
            //" (SUBSTR((LPAD(D.DOQ70HSP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(D.DOQ70HSP, 4, '0'), 3, 2))  AS HORA_SALIDA," +
            //" D.DOQ70CVL AS NUM_VUELO_LLEGANDO, D.DOQ70CVS AS NUM_VUELO_SALIENDO,D.DOQ70DES AS DESTINO, DES.MACTY1 AS DESTINODESC," +
            //" P.PEQ70IDV as ID_VUELO, P.PEQ70TV AS TIPO_VUELO, D.DOQ70NVS AS NATURALEZA" +
            //" From \"%pdta%\".\"FQ70594A\" P " +
            //" LEFT JOIN \"%pdta%\".\"FQ70591H\" D ON D.DOQ70IDD = P.PEQ70IDD " +
            //" LEFT JOIN \"%pdta%\".\"FQ70591C\" A ON A.MAAN8 = D.DOAN8 " +
            //" LEFT JOIN \"%pdta%\".\"FQ70590A\" DES ON DES.MAQ70IATA = D.DOQ70DES " +
            //" LEFT JOIN \"%pdta%\".\"FQ70590A\" ORG ON ORG.MAQ70IATA = D.DOQ70ORG" +
            //"  WHERE D.DOQ70FSP = '119295' and P.PEQ70IDD = '597161' and" +
            //" P.PEQ70EST IN('01','02','04')  ";

            /*
            if (Iddaily.Count >0)
            {
                string cadenaid = "";
                for (int i = 0; i < Iddaily.Count; i++)
                {
                    if (i < 999)
                    {
                        cadenaid = cadenaid + "'" + Iddaily[i] + "',";

                    }
                    else { break; }
                    
                } 
                cadenaid = cadenaid.Substring(0, cadenaid.Length - 1);

                consulta = consulta + "    union all " +
            " SELECT DISTINCT P.PEQ70IDD AS ID_DAILY,P.PEQ70IDP as ID_PASAJERO ,D.DOQ70FLP AS FECHA_LLEGADA, D.DOQ70CM AS MATRICULA, "+
            " (SUBSTR((LPAD(D.DOQ70HLP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(D.DOQ70HLP, 4, '0'), 3, 2))  AS HORA_LLEGADA, D.DOQ70ORG AS ORIGEN,  "+
            " ORG.MACTY1 AS ORIGEN_DES,D.DOAN8 AS ID_AEROLINEA_LLEGADA, A.MAALPH AS AEROLINEA, D.DOQ70FSP AS FECHA_SALIDA,  "+
            " (SUBSTR((LPAD(D.DOQ70HSP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(D.DOQ70HSP, 4, '0'), 3, 2))  AS HORA_SALIDA,"+
            " D.DOQ70CVL AS NUM_VUELO_LLEGANDO, D.DOQ70CVS AS NUM_VUELO_SALIENDO,D.DOQ70DES AS DESTINO, DES.MACTY1 AS DESTINODESC, "+
            " P.PEQ70IDV as ID_VUELO, P.PEQ70TV AS TIPO_VUELO, D.DOQ70NVS AS NATURALEZA "+
            " From %pdta%.FQ70594A P " +
            " LEFT JOIN %pdta%.FQ70591H  D ON D.DOQ70IDD = P.PEQ70IDD" +
            " LEFT JOIN %pdta%.FQ70591C  A ON A.MAAN8 = D.DOAN8" +
            " LEFT JOIN %pdta%.FQ70590A  DES ON DES.MAQ70IATA = D.DOQ70DES " +
            " LEFT JOIN %pdta%.FQ70590A  ORG ON ORG.MAQ70IATA = D.DOQ70ORG " +
             "  WHERE P.PEQ70IDD in (" + cadenaid +")";
            //" ";
            }
            */


            consulta = consulta.Replace("%pdta%", squemaData);

            string squemaControl = string.Empty;
            squemaControl = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Control", config);
            
            consulta = consulta.Replace("%crpctl%", squemaControl);


            return consulta;
        }


        public static string ConsultaJDEPendientes(DateTime FechaBusqueda, DateTime FechaAnterior, IConfiguration config)
        {

            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string Fjuliana = ConvertToJulian(FechaBusqueda).ToString();
            string FJulianaAnt = ConvertToJulian(FechaAnterior).ToString();
            string consulta = " SELECT DISTINCT DOQ70IDD AS ID_DAILY,DOQ70FLP AS FECHA_LLEGADA," +
            " DOQ70CM AS MATRICULA,(SUBSTR((LPAD(DOQ70HLP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(DOQ70HLP, 4, '0'), 3, 2))  AS HORA_LLEGADA," +
            "DOQ70ORG AS ORIGEN, DOAN8 AS ID_AEROLINEA_LLEGADA " +
            " FROM %pdta%.FQ70591H WHERE DOQ70FLP = '" + FJulianaAnt + "' AND DOQ70FSR = '0' AND DOQ70NVL<> 'C' AND DOQ70ORG NOT LIKE '%HAN%' ";

            // para efectos de pruebas, tenemos fecha 119263  
            //consulta = "SELECT DISTINCT DOQ70IDD AS ID_DAILY,DOQ70FLP AS FECHA_LLEGADA," +
            //" DOQ70CM AS MATRICULA,(SUBSTR((LPAD(DOQ70HLP, 4, '0')), 1, 2) || ':' || SUBSTR(LPAD(DOQ70HLP, 4, '0'), 3, 2))  AS HORA_LLEGADA," +
            //"DOQ70ORG AS ORIGEN, DOAN8 AS ID_AEROLINEA_LLEGADA " +
            //" FROM %pdta%.FQ70591H WHERE DOQ70FLP = '119295' AND DOQ70FSR = '0' AND DOQ70NVL<> 'C' AND DOQ70ORG NOT LIKE '%HAN%' ";
            
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
