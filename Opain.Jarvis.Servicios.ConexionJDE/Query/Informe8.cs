using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public class Informe8
    {
        public static string Consulta(Repositorio.FiltroBusqueda filtro, IConfiguration config)
        {

            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string filtroAerolinea = "";
            if (filtro.aerolinea.Trim() != "0" && filtro.aerolinea.Trim() != null)
            {
                filtroAerolinea = string.Format(" and TRIM(\"F42119\".\"SDAN8\") IN('{0}') ", filtro.aerolinea.Trim());
            }

            string filtroFechas = "";
            if (filtro.fechaDesde != null && filtro.fechaHasta != null)
            {
                filtroFechas = filtroFechas + string.Concat(string.Format(" and \"F42119\".\"SDTRDJ\" > = '{0}' ", filtro.fechaDesde));
                filtroFechas = filtroFechas + string.Concat(string.Format(" and \"F42119\".\"SDTRDJ\"  < = '{0}' ", filtro.fechaHasta));
            }


            string consulta  =string.Concat(" SELECT distinct ",
                    //"CONCAT(TRIM(\"CRPDTA\".\"F42119\".sdsrp1),'30')AS PrefijoFactura, ",
                    " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                    " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                    " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura, ",
                    " \"F42119\".\"SDDOC\" AS \"Factura\", ",
                    " \"F0101\".\"ABALPH\" as \"NombreAerolinea\", ",
                    " \"F42119\".\"SDVR01\" as \"Matricula\", ",
                    " \"F42119\".\"SDDSC1\" as \"TipodeServicio\", ",
                    " to_char(to_date(concat(to_char(to_number(substr(\"F42119\".\"SDTRDJ\", 1, 3) + 1900)), substr(\"F42119\".\"SDTRDJ\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaServicio, ",
                    //" substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDTRDJ\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"F42119\".\"SDTRDJ\" as int)),4,3) -1),1,10) AS FechaServicio, ",
                    " \"F42119\".\"SDAEXP\" as \"Tarifa\", ",
                    " (\"F42119\".\"SDUPRC\")/10000 as \"ValorCobroCOP\" ",
                    " FROM \"%pdta%\".\"F42119\", \"%pdta%\".\"F0101\", \"%pdta%\".\"FQ70591C\" ",
                    " WHERE ",
                    "\"F42119\".\"SDDCTO\" IN ('SH','SI') ",
                    " AND     \"F42119\".\"SDDCT\" IN ('FP', 'FQ') ",
                    " AND     \"F42119\".\"SDAN8\" = \"F0101\".\"ABAN8\" ",
                    " AND     \"F42119\".\"SDAN8\"=\"FQ70591C\".\"MAAN8\" ",
                    filtroFechas,
                    filtroAerolinea,
                    string.Format(" AND     TRIM(\"F42119\".\"SDDOC\") > =(TRIM('{0}')) ", filtro.facturaDesde.Trim()),
                    string.Format(" AND     TRIM(\"F42119\".\"SDDOC\") < =(TRIM('{0}')) ", filtro.facturaHasta.Trim()),
                    " and rownum <= 100" ) ;
            if (filtro.Descarga)
            {
                consulta = consulta.Replace("and rownum <= 100", " Order by 2");
            }
            else
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
