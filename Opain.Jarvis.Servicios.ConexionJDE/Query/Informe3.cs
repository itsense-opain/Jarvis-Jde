using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public class Informe3
    {
        public static string Consulta(Repositorio.FiltroBusqueda filtro, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            var Tipo = string.Empty;
            string vlrPorcentaje = string.Empty;
                      
            Tipo = "'S1','S2'";
            vlrPorcentaje = "((\"F42119\".\"SDAEXP\" * 3)/100)";
         

            string filtroFechas = "";
            if (filtro.fechaDesde != null && filtro.fechaHasta != null)
            {
                filtroFechas = filtroFechas + string.Concat(string.Format(" and \"F42119\".\"SDTRDJ\" > = '{0}' ", filtro.fechaDesde));
                filtroFechas = filtroFechas + string.Concat(string.Format(" and \"F42119\".\"SDTRDJ\"  < = '{0}' ", filtro.fechaHasta));

            }

            string filtroAerolinea = "";
            if (filtro.aerolinea.Trim() != "0")
            {
                filtroAerolinea = string.Format(" and TRIM(\"F42119\".\"SDAN8\") IN('{0}') ", filtro.aerolinea.Trim());
            }

            string consulta = "";
                  consulta = string.Concat(" SELECT ",
                        " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                        " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                        " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura,",
                        "(\"F42119\".\"SDPQOR\" / 100) AS Cantidad, ",
                        " \"F42119\".\"SDDOC\" AS \"Factura\", ",
                        " (SELECT MAALPH FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"Aerolinea\", ",
                        " to_char(to_date(concat(to_char(to_number(substr(\"F42119\".\"SDTRDJ\", 1, 3) + 1900)), substr(\"F42119\".\"SDTRDJ\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaVuelo, ",
                        //" substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDTRDJ\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"F42119\".\"SDTRDJ\" as int)),4,3) -1),1,10) AS \"FechaVuelo\", ",
                        " \"F42119\".\"SDVR01\" as \"Matricula\", ",
                        " (SELECT MAQ70OACI FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"Sigla\", ",
                        " \"F42119\".\"SDDSC2\" as \"NumeroVuelo\", ",
                        " \"FQ70591H\".\"DOQ70CVS\" as \"VueloSalida\", ",
                        " (\"F42119\".\"SDUPRC\"/10000) as \"TarifaCOP\", ",
                        "( \"F42119\".\"SDUORG\")/100 as \"PaganCOP\", ",// F42119.SDUORG
                        " \"F42119\".\"SDAEXP\" as \"CobroCOP\", ",// F42119.SDAEXP
                        //" "+ vlrPorcentaje + " as \"VrComisionCOP\" ",
                       " CASE    WHEN \"F42119\".\"SDDCTO\" = 'S1' THEN \"F42119\".\"SDAEXP\" * 0.03 WHEN \"F42119\".\"SDDCTO\" IN('S2', 'S3') THEN \"F42119\".\"SDAEXP\" * 0.02  ELSE 0 END AS \"VrComisionCOP\""
                        , // IF(F42119.SDDCTO IN ('S1') ? (F42119.SDAEXP) * 3% || IF(F42119.SDDCTO IN ('S2','S3') ? (F42119.SDAEXP) * 2% 
                        " FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\", \"%pdta%\".\"F42119\" \"F42119\", ",
                        " \"%pdta%\".\"FQ70591H\" \"FQ70591H\" ",
                        " WHERE   \"F42119\".\"SDAN8\"=\"FQ70591C\".\"MAAN8\" ",
                        filtroFechas,
                        filtroAerolinea,
                         " and \"FQ70591H\".\"DOQ70IDD\"=\"F42119\".\"SDURAT\"/100 ",
                        string.Format(" and     TRIM(\"F42119\".\"SDDOC\") > =(TRIM('{0}')) ", filtro.facturaDesde.Trim()),
                        string.Format(" and     TRIM(\"F42119\".\"SDDOC\") < =(TRIM('{0}')) ", filtro.facturaHasta.Trim()),
                        " and \"F42119\".\"SDDCT\" IN ('FV', 'FA') ",
                        string.Format(" and \"F42119\".\"SDDCTO\" IN ({0}) ", Tipo),
                        " and rownum <= 100");

            if (filtro.Descarga)
            {
                consulta = consulta.Replace("and rownum <= 100", " Order by 2");
            } else
            {
                consulta = consulta + " Order by 2";
            }


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
