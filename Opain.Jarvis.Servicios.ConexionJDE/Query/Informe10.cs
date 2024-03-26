using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    /// <summary>
    /// 
    /// </summary>
    public static class Informe10
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public static string Consulta(Repositorio.FiltroBusqueda filtro, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string filtroAerolinea = "";
            if (filtro.aerolinea.Trim() != "0")
            {
                filtroAerolinea = string.Format(" and TRIM(\"F42119\".\"SDAN8\") IN('{0}') ", filtro.aerolinea.Trim());
            }
            string consulta = string.Empty;

          
            // Consulta Nueva
            consulta = string.Concat("SELECT sum(ft) as  Valor,sum(nt) as NotaCredito,sum((ft)+(nt)) as Total,SDAN8, ABALPH as NombredeTercero, ABTAX as NIT_CEDULA",
                                     " from(SELECT SUM(SDAEXP),",
                                      "CASE  WHEN SDDCT IN('FA', 'FV') THEN SUM(SDAEXP)  ELSE 0 END as FT,",
                                      "CASE  WHEN SDDCT = 'FB' THEN SUM(SDAEXP) ELSE 0  END as NT, SDAN8, ABALPH, ABTAX" ,
                                      "  FROM \"%pdta%\".\"F42119\", \"%pdta%\".\"F0101\" ",
                                       " where " + string.Format("\"F42119\".\"SDIVD\" > = '{0}'", filtro.fechaDesde),
                                        " and ", string.Format("\"F42119\".\"SDIVD\" > = '{0}'", filtro.fechaHasta),
                                        filtroAerolinea,
                                        "AND \"F42119\".\"SDDCT\" IN('FA', 'FB', 'FV')",
                                        "and \"F42119\".\"SDDCTO\" IN('S2', 'S3', 'S1', 'CA', 'CB')",
                                        "AND \"F42119\".\"SDAN8\" = \"F0101\".\"ABAN8\"  and rownum <= 100 ",
                                        " GROUP BY SDAN8, SDDCT, ABALPH, ABTAX ",
                                        " )dual  group by SDAN8, ABALPH, ABTAX order by 5");

            consulta = consulta.Replace("%pdta%", squemaData);

            return consulta;
        }
    }
}
