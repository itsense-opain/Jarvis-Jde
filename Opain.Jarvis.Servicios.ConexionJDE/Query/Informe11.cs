using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public class Informe11
    {
        public static string Consulta(Repositorio.FiltroBusqueda filtro, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consulta = string.Empty;
            string tipo = (filtro.tipo == "COP")?"DOM":"INT";

            

            consulta = string.Concat(" SELECT COUNT(*) as NumeroVuelos,",
        " SUM(PEQ70TTP) as Pasajeros,",
        " SUM(PEQ70TRF + PEQ70TRP) as Transito,",
        " SUM(PEQ70EC) as Tripulacion,",
        " SUM(PEQ70TEX) as Excentos,",
        " SUM(PEQ70TTI) as Infantes,",
        " SUM(PEQ70TTC) as PaganCOP,",
        " SUM(PEQ70TTU) as PaganUSD,",
        " A.ABALPH as FacturadoA ",
        " from ",
             "\"%pdta%\".\"FQ70594A\"  FQ70594A",
            "  LEFT JOIN \"%pdta%\".\"F0101\" A ON A.ABAN8 = \"FQ70594A\".\"PEQ70AFCT\"",
            " WHERE TRIM(\"FQ70594A\".\"PEQ70TV\") = ('" + tipo + "') ",
            " and  TRIM(\"FQ70594A\".\"PEQ70EST\") = ('08') ",
             " and ", string.Format("\"FQ70594A\".\"PEQ70FRL\" > = '{0}'", filtro.fechaDesde),
               " and ", string.Format("\"FQ70594A\".\"PEQ70FRL\" > = '{0}'", filtro.fechaHasta),
               " and rownum <= 100",
         " GROUP BY  PEQ70AFCT, A.ABALPH" );

            if (filtro.Descarga)
            {
                consulta = consulta.Replace("and rownum <= 100", " ");
                consulta = consulta + " Order by 9";
            }
            else
            {
                consulta = consulta + " Order by 9";
            }


            consulta = consulta.Replace("%pdta%", squemaData);

            return consulta;
        }
    }
}
