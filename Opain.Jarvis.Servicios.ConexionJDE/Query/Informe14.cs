using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public class Informe14
    {
        public static string Consulta(Repositorio.FiltroBusqueda filtro, IConfiguration config)
        {

            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consulta = string.Empty;
            string filtroAerolinea = "";
            if (filtro.aerolinea.Trim() != "0")
            {
                filtroAerolinea = string.Format(" and TRIM(\"F42119\".\"SDAN8\") IN('{0}') ", filtro.aerolinea.Trim());
            }
            string filtroFechas = "";
            if (filtro.fechaDesde != null && filtro.fechaHasta != null)
            {
                filtroFechas = filtroFechas + string.Concat(string.Format(" and \"FQ70591H\".\"DOQ70FSP\" > = '{0}' ", filtro.fechaDesde));
                filtroFechas = filtroFechas + string.Concat(string.Format(" and \"FQ70591H\".\"DOQ70FSP\"  < = '{0}' ", filtro.fechaHasta));
            }
            consulta = string.Concat(" SELECT ",
                        //"CONCAT(TRIM(\"CRPDTA\".\"F42119\".sdsrp1),'30')AS PrefijoFactura,  ",
                        " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                        " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                        " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura, ",
                        "0 as Cantidad, \"F42119\".\"SDUPC1\" AS  Categoria,  \"FQ70591C\".\"MAQ70OACI\" AS Sigla, ",
                        "\"F42119\".\"SDCRCD\" AS TipoMoneda,",
                        "\"F42119\".\"SDDCT\" AS Factura,",
                        "(\"F42119\".\"SDUORG\")/100 AS CantidadHoras,",
                        "\"F42119\".\"SDDOC\" AS NumeroFactura,",
                        "(\"F42119\".\"SDUPRC\")/10000 AS TarifaCOP,",
                        "\"F42119\".\"SDURRF\" AS Posicion,",
                        "\"F42119\".\"SDAEXP\" AS TotalCOP,",
                        "(\"F42119\".\"SDFUP\")/10000 AS TarifaUSD,",
                        "(\"F42119\".\"SDFEA\")/100 AS TotalUSD,",
                        "\"F42119\".\"SDAN8\" AS AN8,",
                        " \"F42119\".\"SDDSC1\" as \"TipoConexion\", ",
                        "\"FQ70591C\".\"MAALPH\" AS Aerolinea,",
                        "\"FQ70591H\".\"DOQ70CM\" AS Matricula,",
                        "\"FQ70591H\".\"DOQ70CVL\" AS CodigoVueloLlegada,",
                        "\"FQ70591H\".\"DOQ70CVS\" AS CodigoVueloSalida,",
                        " \"FQ70591H\".\"DOQ70HLP\" as \"HoraIngreso\", ",
                        " \"FQ70591H\".\"DOQ70HSP\" as \"HoraSalida\", ",
                        " to_char(to_date(concat(to_char(to_number(substr(\"FQ70591H\".\"DOQ70FLP\", 1, 3) + 1900)), substr(\"FQ70591H\".\"DOQ70FLP\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaLlegadaPosicion, ",
                        " to_char(to_date(concat(to_char(to_number(substr(\"FQ70591H\".\"DOQ70FSP\", 1, 3) + 1900)), substr(\"FQ70591H\".\"DOQ70FSP\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaSalidaPosicion, ",
                        "\"FQ70591H\".\"DOQ70ORG\" AS OrigenVuelo,",
                        "\"FQ70591H\".\"DOQ70DES\" AS DestinoVuelo ",
                        " FROM ",
                        "\"%pdta%\".\"F42119\" F42119,",
                        "\"%pdta%\".\"FQ70591H\" FQ70591H,",
                        "\"%pdta%\".\"FQ70591C\" FQ70591C ",
                        " WHERE ",
                        "\"F42119\".\"SDDCT\" IN('FP') ",
                        " AND \"F42119\".\"SDURAT\" / 100 = \"FQ70591H\".\"DOQ70IDD\" ",
                        " AND     TRIM(\"F42119\".\"SDDOC\") > = " + filtro.facturaDesde + " ",
                        " AND     TRIM(\"F42119\".\"SDDOC\") < =" + filtro.facturaHasta + " ",
                        " AND \"F42119\".\"SDIVD\" > = 117335 ",
                        " AND \"F42119\".\"SDAN8\" = \"FQ70591C\".\"MAAN8\" ",
                        filtroAerolinea,
                        filtroFechas,
                        " AND \"F42119\".\"SDDCTO\" IN('SM', 'SL')",
                        " UNION ",
                        " SELECT ",
                        //"CONCAT(TRIM(\"CRPDTA\".\"F42119\".sdsrp1),'30')AS PrefijoFactura, ",
                        " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                        " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                        " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura, ",
                        "0 as Cantidad, \"F42119\".\"SDUPC1\" as Categoria, \"FQ70591C\".\"MAQ70OACI\" AS Sigla, ",
                        "\"F42119\".\"SDCRCD\" AS TipoMoneda,",
                        "\"F42119\".\"SDDCT\" AS Factura,",
                        "(\"F42119\".\"SDUORG\")/100 AS CantidadHoras,",
                        "\"F42119\".\"SDDOC\" AS NumeroFactura,",
                        "(\"F42119\".\"SDUPRC\")/10000 AS TarifaCOP,",
                        "\"F42119\".\"SDURRF\" AS Posicion,",
                        "\"F42119\".\"SDAEXP\" AS TotalCOP,",
                        "(\"F42119\".\"SDFUP\")/10000 AS TarifaUSD,",
                        "(\"F42119\".\"SDFEA\")/100 AS TotalUSD,",
                        "\"F42119\".\"SDAN8\" AS AN8,",
                        " \"F42119\".\"SDDSC1\" as \"TipoConexion\", ",
                        "\"FQ70591C\".\"MAALPH\" AS Aerolinea,",
                        "\"FQ70591H\".\"DOQ70CM\" AS Matrícula,",
                        "\"FQ70591H\".\"DOQ70CVL\" AS CodigoVueloLlegada,",
                        "\"FQ70591H\".\"DOQ70CVS\" AS CodigoVueloSalida,",
                        " \"FQ70591H\".\"DOQ70HLP\" as \"HoraIngreso\", ",
                        " \"FQ70591H\".\"DOQ70HSP\" as \"HoraSalida\", ",
                        " to_char(to_date(concat(to_char(to_number(substr(\"FQ70591H\".\"DOQ70FLP\", 1, 3) + 1900)), substr(\"FQ70591H\".\"DOQ70FLP\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaLlegadaPosicion, ",
                        " to_char(to_date(concat(to_char(to_number(substr(\"FQ70591H\".\"DOQ70FSP\", 1, 3) + 1900)), substr(\"FQ70591H\".\"DOQ70FSP\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaSalidaPosicion, ",
                        "\"FQ70591H\".\"DOQ70ORG\" AS OrigenVuelo,",
                        "\"FQ70591H\".\"DOQ70DES\" AS DestinoVuelo",
                        " FROM ",
                        "\"%pdta%\".\"F42119\" F42119,",
                        "\"%pdta%\".\"FQ70591H\" FQ70591H,",
                        "\"%pdta%\".\"FQ70591C\" FQ70591C ",
                        " WHERE ",
                        "\"F42119\".\"SDDCT\" IN('FL')",
                        " AND \"F42119\".\"SDURAT\" / 100 = \"FQ70591H\".\"DOQ70IDD\"",
                        " AND     TRIM(\"F42119\".\"SDDOC\") > = " + filtro.facturaDesde + "",
                        " AND     TRIM(\"F42119\".\"SDDOC\") < =" + filtro.facturaHasta + "",
                        " AND \"F42119\".\"SDIVD\" > = 1",
                        filtroAerolinea,
                        filtroFechas,
                        " AND \"F42119\".\"SDAN8\" = \"FQ70591C\".\"MAAN8\" ",
                        " AND \"F42119\".\"SDDCTO\" IN('A8','A9','A2','A4') and rownum <= 100 ");
            if (filtro.Descarga)
            {
                consulta = consulta.Replace("and rownum <= 100", " Order by 10");
            }
            else
            {
                consulta = consulta + " Order by 10";
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
