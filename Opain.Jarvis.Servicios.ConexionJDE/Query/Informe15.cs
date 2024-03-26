using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    /// <summary>
    /// Puentes de abordaje ampliación LA33 
    /// </summary>
    public class Informe15
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
                consulta = string.Concat("SELECT \"F42119\".\"SDDOC\" as Factura,",
                    " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                    " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                    " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura, ",
                    "\"F42119\".\"SDVR01\" as Matricula,",
                    "\"FQ70591H\".\"DOQ70CVL\" as VueloIngreso,",
                    "\"FQ70591H\".\"DOQ70CVS\" as VueloSalida,",
                    "\"FQ70591H\".\"DOQ70NPS\" as POS,",
                     "(\"F42119\".\"SDPQOR\" / 100) AS Cantidad, ",
                    //"\"FQ70596A\".\"VMQ70CXP\" as Cantidad,",
                    "(\"F42119\".\"SDUPRC\")/10000 as CobroCOP,",
                    "\"F42119\".\"SDAEXP\" as TotalCOP,",
                    "(\"F42119\".\"SDFEA\")/100 as TotalUSD,",
                    "(\"F42119\".\"SDFUP\")/10000 as CobroUSD,",
                    "(SELECT MAQ70OACI FROM \"%pdta%\".\"FQ70591C\"  FQ70591C  WHERE MAAN8 = \"F42119\".\"SDAN8\") as SiglaAerolinea,",
                    "(SELECT MAALPH FROM \"%pdta%\".\"FQ70591C\"  FQ70591C  WHERE MAAN8 = \"F42119\".\"SDAN8\") as NombreAerolinea,",
                    " to_char(to_date(concat(to_char(to_number(substr(\"FQ70591H\".\"DOQ70FLR\", 1, 3) + 1900)), substr(\"FQ70591H\".\"DOQ70FLR\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaIngreso, ",
                    " to_char(to_date(concat(to_char(to_number(substr(\"FQ70591H\".\"DOQ70FSP\", 1, 3) + 1900)), substr(\"FQ70591H\".\"DOQ70FSP\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaSalida, ",
                    "\"FQ70591H\".\"DOQ70HLP\" as HoraIngreso,",
                    "\"FQ70591H\".\"DOQ70HSP\" as HoraSalida, ",
                    "\"F42119\".\"SDCRCD\" AS TipoMoneda ",
                    " FROM ",
                    "\"%pdta%\".\"FQ70591C\" FQ70591C,",
                    "\"%pdta%\".\"F42119\" F42119,",
                    "\"%pdta%\".\"FQ70591H\" FQ70591H,",
                    "\"%pdta%\".\"FQ70593B\" FQ70593B,",
                    "\"%pdta%\".\"FQ70593C\" FQ70593C,",
                    "\"%pdta%\".\"FQ70596A\" FQ70596A,",
                    "\"%pdta%\".\"F4106\" F4106 ",
                    " WHERE(\"F42119\".\"SDAN8\" = \"FQ70591C\".\"MAAN8\"",
                    " and     \"FQ70591H\".\"DOQ70IDD\" = \"FQ70593B\".\"GPQ70IDD\"",
                    " and     \"FQ70593B\".\"GPQ70TAR\" = \"FQ70593C\".\"MEQ70TAR\"",
                    " and     \"FQ70591H\".\"DOQ70IDD\" = \"F42119\".\"SDURAT\" / 100 ",
                    " and     \"FQ70596A\".\"VMQ70IDD\" = \"F42119\".\"SDURAT\" / 100 ",
                    filtroAerolinea,
                    filtroFechas,
                    " and     TRIM(\"F42119\".\"SDDOC\") > = " + filtro.facturaDesde + "",
                    " and     TRIM(\"F42119\".\"SDDOC\") < =  " + filtro.facturaHasta + "",
                    //" and     \"F42119\".\"SDDCT\" IN('FH', 'FL')",
                    " and     \"F42119\".\"SDDCT\" IN('FL')",
                    " and     \"F42119\".\"SDDCTO\" IN('A6','A7')",
                    " and     \"FQ70593C\".\"MEEV05\" = '0'",
                    " and     \"FQ70593C\".\"MEEV06\" = '1'",
                    " and     \"FQ70591H\".\"DOQ70NPS\" in ('C88','C89') ",
                    " and     \"F4106\".\"BPITM\" = \"F42119\".\"SDITM\"",
                    " and     \"F4106\".\"BPEFTJ\" <= \"F42119\".\"SDIVD\"",
                    " and     \"F4106\".\"BPEXDJ\" >= \"F42119\".\"SDIVD\") ");

            consulta = consulta + " Order by 12 ";

            consulta = consulta.Replace("%pdta%", squemaData);

            return consulta;
        }
    }
}
