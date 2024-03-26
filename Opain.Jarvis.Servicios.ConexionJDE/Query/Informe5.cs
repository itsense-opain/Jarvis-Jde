using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    /// <summary>
    /// Informe de Mostradores
    /// </summary>
    public class Informe5
    {
        public static string Consulta(Repositorio.FiltroBusqueda filtro, IConfiguration config)
        {

            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string filtroFechas = "";
            if (filtro.fechaDesde != null && filtro.fechaHasta != null)
            {
                filtroFechas = filtroFechas + string.Concat(string.Format(" and \"F42119\".\"SDTRDJ\" > = '{0}' ", filtro.fechaDesde));
                filtroFechas = filtroFechas + string.Concat(string.Format(" and \"F42119\".\"SDTRDJ\"  < = '{0}' ", filtro.fechaHasta));               
           }

            string filtroAerolinea = "";
            if (filtro.aerolinea.Trim() != "0" && filtro.aerolinea.Trim() != null)
            {
                filtroAerolinea = string.Format(" and TRIM(\"F42119\".\"SDAN8\") IN('{0}') ", filtro.aerolinea.Trim());
            }

            string consulta = string.Concat(" SELECT ",
                            //" CONCAT(TRIM(\"CRPDTA\".\"F42119\".sdsrp1),'30')AS PrefijoFactura, ",
                            " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                            " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                            " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura, ",
                            " \"F42119\".\"SDCRCD\" AS TipoMoneda, ",
                            " \"F42119\".\"SDDOC\" AS \"Factura\", ",
                            " (SELECT MAALPH FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"Aerolineas\", ",
                            " to_char(to_date(concat(to_char(to_number(substr(\"F42119\".\"SDTRDJ\", 1, 3) + 1900)), substr(\"F42119\".\"SDTRDJ\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaVuelo, ",
                            " \"F42119\".\"SDVR01\" as \"Matricula\", ",
                            " (SELECT MAQ70OACI FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"Sigla\", ",
                            " \"F42119\".\"SDDSC2\" as \"NumeroVuelo\", ",
                            "( \"F42119\".\"SDUORG\" )/100 as \"Cantidad\", ", // F42119.SDUORG
                            "( \"F42119\".\"SDFUP\" )/10000 as \"TarifaUSD\", ", // F42119.SDFUP 
                            "( \"F42119\".\"SDFEA\" )/100 as \"CobroUSD\", ", // F42119.SDFEA
                                                                              //"( \"F42119\".\"SDUORG\" )/100 as \"PaganUSD\", ", // F42119.SDUORG
                            "(\"F42119\".\"SDUPRC\")/10000 AS TarifaCOP,",
                            "\"F42119\".\"SDAEXP\" AS CobroCOP, ",
                            "\"FQ70591H\".\"DOQ70CVS\" AS CodigoVueloSalida ",
                            " FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\", ",
                            " \"%pdta%\".\"F42119\" \"F42119\", ",
                            " \"%pdta%\".\"FQ70591H\" FQ70591H ",
                            " WHERE   \"F42119\".\"SDAN8\"=\"FQ70591C\".\"MAAN8\" ",
                            filtroFechas,
                            filtroAerolinea,
                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") > =(TRIM('{0}')) ", filtro.facturaDesde.Trim()),
                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") < =(TRIM('{0}')) ", filtro.facturaHasta.Trim()),
                            " and \"F42119\".\"SDDCT\" IN ('FV', 'FA') ",
                            " and \"F42119\".\"SDUORG\" > = 0 ",
                            " and \"F42119\".\"SDDCTO\" IN ('SE') ",
                            "and  (\"F42119\".\"SDURAT\")/100 = \"FQ70591H\".\"DOQ70IDD\" ");
                            //" and rownum <= 10000 ");
            if (filtro.Descarga)
            {
                //consulta = consulta.Replace("and rownum <= 100", " Order by 2");
                consulta = consulta + " Order by 2";
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
