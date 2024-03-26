using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public class Informe16
    {
        public static string Consulta(Repositorio.FiltroBusqueda filtro, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);


            string consulta = "";
            string filtroAerolinea = "";
            if (filtro.aerolinea.Trim() != "0")
            {
                filtroAerolinea = string.Format(" and TRIM(\"F42119\".\"SDAN8\") IN('{0}') ", filtro.aerolinea.Trim());
            }
            string filtroFechas = "";
            if (filtro.fechaDesde != null && filtro.fechaHasta != null)
            {
                //filtroFechas = filtroFechas + string.Concat(string.Format(" and \"FQ70595C\".\"PUQ70FCX\" > = '{0}' ", filtro.fechaDesde));
                //filtroFechas = filtroFechas + string.Concat(string.Format(" and \"FQ70595C\".\"PUQ70FCX\"  < = '{0}' ", filtro.fechaHasta));
                filtroFechas = filtroFechas + string.Concat(string.Format(" and \"F42119\".\"SDIVD\" > = '{0}' ", filtro.fechaDesde));
                filtroFechas = filtroFechas + string.Concat(string.Format(" and \"F42119\".\"SDIVD\"  < = '{0}' ", filtro.fechaHasta));
            }

            consulta = string.Concat("SELECT  ",
                             //"CONCAT(TRIM(\"CRPDTA\".\"F42119\".sdsrp1),'30')AS PrefijoFactura, ",
                            " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                            " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                            " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura,",
                            "\"F42119\".\"SDAN8\" as \"Aero\",",
                            " \"FQ70591C\".\"MAALPH\" as \"NombreAerolinea\",",
                            //" \"FQ70591H\".\"DOQ70NPS\" as \"IdGPU\",",
                            " \"FQ70591H\". \"DOQ70IDD\" as \"IdGPU\",",
                            " \"F42119\".\"SDVR01\" as \"Matricula\",",
                            " \"FQ70591H\".\"DOQ70CVL\" as \"NumeroVueloIngreso\",",
                            " \"FQ70591H\".\"DOQ70CVS\" as \"NumeroVueloSalida\",",
                            " \"FQ70595C\".\"PUQ70HCX\" as \"HoraConexion\",",
                            " \"FQ70591H\".\"DOQ70HSP\" as \"HoraDesconexion\",",
                            " \"F42119\".\"SDDOC\" as \"Factura\",",
                            " (\"F47012\".\"SZFUP\")*1000 as \"TarifaUSD\",",
                            " \"F47012\".\"SZCRR\"*10000 as \"TRM\",",
                            " (\"F47012\".\"SZUPRC\")/10000 as \"TarifaCOP\",",
                            " \"F47012\".\"SZAEXP\" as \"TotalCOP\",",
                            " (\"FQ70595C\".\"PUMATH01\")/100 as \"Minutos\",",
                            " to_char(to_date(concat(to_char(to_number(substr(\"FQ70595C\".\"PUQ70FCX\", 1, 3) + 1900)), substr(\"FQ70595C\".\"PUQ70FCX\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaConexion, ",
                            " to_char(to_date(concat(to_char(to_number(substr(\"FQ70595C\".\"PUQ70FDX\", 1, 3) + 1900)), substr(\"FQ70595C\".\"PUQ70FDX\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaDesconexion, ",
                            " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"FQ70591H\".\"DOQ70FLR\" as int) / 1000))), 'DD-MM-YYYY') + substr(to_char(CAST(\"FQ70591H\".\"DOQ70FLR\" as int)), 4, 3) - 1), 1, 10) AS FechaIngreso,",
                            " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"FQ70591H\".\"DOQ70FSP\" as int) / 1000))), 'DD-MM-YYYY') + substr(to_char(CAST(\"FQ70591H\".\"DOQ70FSP\" as int)), 4, 3) - 1), 1, 10) AS FechaSalidaFSP,",
                            " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDTRDJ\" as int) / 1000))), 'DD-MM-YYYY') + substr(to_char(CAST(\"F42119\".\"SDTRDJ\" as int)), 4, 3) - 1), 1, 10) AS FechaSalida",
                            " FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\",",
                            " \"%pdta%\".\"F42119\" \"F42119\",",
                            " \"%pdta%\".\"FQ70591H\" \"FQ70591H\",",
                            " \"%pdta%\".\"FQ70595C\" \"FQ70595C\",",
                            " \"%pdta%\".\"FQ70593C\" \"FQ70593C\",",
                            " \"%pdta%\".\"F47012\" \"F47012\"",
                            "  WHERE  (",
                            " \"F42119\".\"SDAN8\" = \"FQ70591C\".\"MAAN8\"",
                            " and \"FQ70591H\".\"DOQ70IDD\"=\"F47012\".\"SZURAT\"/100 ",
                            " and \"FQ70595C\".\"PUQ70IDD\"=\"F42119\".\"SDURAT\"/100",
                            " and FQ70595C.PUQ70IDD = FQ70591H.DOQ70IDD",
                            " and FQ70595C.PUQ70TAR = FQ70593C.MEQ70TAR",
                            " and  F42119.SDDOCO = F47012.SZEDOC ",
                            "and     \"F42119\".\"SDDCTO\"=\"F47012\".\"SZEDCT\"",

                            " and     FQ70595C.PUQ70IDD = (F42119.SDURAT)/100",
                            filtroAerolinea,
                            filtroFechas,
                            " and     TRIM(F42119.SDDOC) > = (TRIM('" + filtro.facturaDesde + "'))",
                            " and     TRIM(F42119.SDDOC) < = (TRIM('" + filtro.facturaDesde + "'))",
                            " and     F42119.SDDCT IN('UG')",
                            " and     F42119.SDDCTO IN('SK')",
                            " and     FQ70593C.MEEV05 = '0'",
                            " and     FQ70593C.MEEV06 <> '1'",
                            " )and rownum <= 100 ");
            if (filtro.Descarga)
            {
                consulta = consulta.Replace("and rownum <= 100", "");
                consulta = consulta + " Order by 2";
            }
            else {
                consulta = consulta + " Order by 2";
            }


            consulta = consulta.Replace("%pdta%", squemaData);

            return consulta;
        }
    }
}
