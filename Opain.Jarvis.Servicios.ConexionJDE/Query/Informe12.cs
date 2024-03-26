using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public class Informe12
    {
        public static string Consulta(Repositorio.FiltroBusqueda filtro, IConfiguration config)
        {

            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string filtroAerolinea = "";
            if (filtro.aerolinea.Trim() != "0")
            {
                filtroAerolinea = string.Format(" and TRIM(\"FQ70591C\".\"MAAN8\") IN('{0}') ", filtro.aerolinea.Trim());
            }
            string consulta = string.Empty;
            consulta = string.Concat(" SELECT TERM.DRDL01 AS TI ,IDTERMINAL,Extencion,FechaSalida as FechaVuelo,Cedula,NOMBRE as Pasajero,APELLIDO as Apellido1, Apellido2,Clase,Destino, CODSOLICITANTE, SOL.DRDL01 AS Solicitante,Aerolinea,",
"ESTADO as Reportada, FechaVuelo as FechaItineradaVuelo, NUMERO_VUELO as Vuelo, CODCAUSAL, CAU.DRDL01 AS CAUSAL from(",
"select   to_char(to_date(concat(to_char(to_number(substr(\"FQ70594E\".\"PETRDJ\", 1, 3) + 1900)), substr(\"FQ70594E\".\"PETRDJ\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaSalida,",
 "    TRIM(FQ70594E.PEQ70TER) as IDTERMINAL,",
  "   TRIM(FQ70594E.PEQ70CSL) as CODSOLICITANTE,",
   "  TRIM(FQ70594E.PEQ70EST) as ESTADO,",
    " TRIM(FQ70594E.PEQ70CAU) as CODCAUSAL,",
     "FQ70594E.PEQ70IDX as Extencion,",
     "FQ70594E.PETAX as CEDULA,",
     "FQ70594E.PEQ70FN as NOMBRE,",
     "FQ70594E.PEQ70SLN as Apellido2,",
   " FQ70594E.PEQ70LN as APELLIDO,",
    " FQ70590A.MACTY1 as DESTINO,",
     "    FQ70591C.MAALPH as AEROLINEA,",
      "   FQ70591E.MFQ70TV as CLASE,",
       "  FQ70594E.PEQ70CVO as NUMERO_VUELO,",
        " to_char(to_date(concat(to_char(to_number(substr(\"FQ70594E\".\"PEQ70FES\", 1, 3) + 1900)), substr(\"FQ70594E\".\"PEQ70FES\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaVuelo ",
 " from %pdta%.FQ70594E FQ70594E,",
  "      %pdta%.FQ70591C FQ70591C,",
  "      %pdta%.FQ70591E FQ70591E,",
   " %pdta%.FQ70590A FQ70590A ",
" where ",
 "FQ70594E.PETRDJ > = " + filtro.fechaDesde + "",
" and  FQ70594E.PETRDJ < =" + filtro.fechaHasta + "",
" AND FQ70594E.PEQ70OACI = FQ70590A.MAQ70OACI",
" and FQ70594E.PEAN8 = FQ70591C.MAAN8 ",
filtroAerolinea,
" and FQ70594E.PEQ70IDV = FQ70591E.MFQ70IDV) p ",
"LEFT JOIN  %crpctl%.F0005 TERM ON TRIM(TERM.DRKY) = P.IDTERMINAL AND  TERM.DRRT IN('TX') AND TERM.DRSY IN('Q70') ",
"LEFT JOIN  %crpctl%.F0005  CAU ON TRIM(CAU.DRKY) = TRIM(P.CODCAUSAL) AND CAU.DRRT IN('CX') AND CAU.DRSY IN('Q70') ",
"LEFT JOIN  %crpctl%.F0005  SOL ON TRIM(SOL.DRKY) = TRIM(P.CODSOLICITANTE) AND SOL.DRRT IN('SL') AND SOL.DRSY IN('Q70') ",
"WHERE ROWNUM <= 100");
            if (filtro.Descarga)
            {
                consulta = consulta.Replace("WHERE ROWNUM <= 100", " Order by 13");
            } else
            {
                consulta = consulta + " Order by 13";
            }

            consulta = consulta.Replace("%pdta%", squemaData);

            string squemaControl = string.Empty;
            squemaControl = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Control", config);

            consulta = consulta.Replace("%crpctl%", squemaControl);


            return consulta;
        }
    }
}
