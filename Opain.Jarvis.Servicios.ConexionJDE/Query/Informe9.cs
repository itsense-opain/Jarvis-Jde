using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public class Informe9
    {
        public static string Consulta(Repositorio.FiltroBusqueda filtro, IConfiguration config)
        {

            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consulta = "SELECT	 FQ70591H.DOQ70IDD as DOQ70IDD, " +
                        "	 FQ70591H.DOQ70CM as DOQ70CM, " +
                        "	 FQ70591H.DOAN8 as DOAN8, " +
                        "	 FQ70594A.PEUKID as PEUKID, " +
                        "	 FQ70594A.PEQ70IATA as PEQ70IATA, " +
                        "	 FQ70594A.PEQ70OACI as PEQ70OACI, " +
                        "	 FQ70594A.PEQ70IDP as PEQ70IDP, " +
                        "	 FQ70594A.PEQ70FRL as PEQ70FRL, " +
                        "  substr((to_date('01-01-'||to_char(round(1900+(CAST(FQ70594A.PEQ70FRL as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(FQ70594A.PEQ70FRL as int)),4,3) -1),1,10) AS FechaSalida, " +
                        "	 FQ70594A.PEQ70HRL as PEQ70HRL, " +
                        "	 FQ70594A.PEQ70TO as PEQ70TO, " +
                        "	 FQ70594A.PEQ70TRP as PEQ70TRP, " +
                        "	 FQ70594A.PEQ70TRF as PEQ70TRF, " +
                        "	 FQ70594A.PEQ70EC as PEQ70EC, " +
                        "	 FQ70594A.PEQ70TR2I as PEQ70TR2I, " +
                        "	 FQ70594A.PEQ70TEX as PEQ70TEX, " +
                        "	 FQ70594A.PEQ70TTA as PEQ70TTA, " +
                        "	 FQ70594A.PEQ70TTI as PEQ70TTI, " +
                        "	 FQ70594A.PEQ70TTP as PEQ70TTP, " +
                        "	 FQ70594A.PEQ70TTC as PEQ70TTC, " +
                        "	 FQ70594A.PEQ70TTU as PEQ70TTU, " +
                        "	 FQ70594A.PEAEXP as PEAEXP, " +
                        "	 FQ70594A.PEFEA as PEFEA, " +
                        "	 FQ70594A.PECRR as PECRR, " +
                        "    FQ70594A.PEQ70EST as PEQ70EST, " +
                        "	 FQ70594A.PEQ70AFCT as PEQ70AFCT, " +
                        "	 FQ70591C.MAALPH as MAALPH, " +
                        "	 FQ70591C.MAQ70IATA as MAQ70IATA, " +
                        "	 FQ70591C.MAQ70OACI as MAQ70OACI, " +
                        "	 FQ70594A.PEQ70CVO as PEQ70CVO, " +
                        "	 FQ70594A.PEQ70NV as PEQ70NV, " +
                        "	 FQ70594A.PEQ70TV as PEQ70TV, " +
                        "    FQ70591H.DOQ70CVS as DOQ70CVS  " +
                        "from	%pdta%.FQ70591C FQ70591C, " +
                        "	%pdta%.FQ70594A FQ70594A, " +
                        "	%pdta%.FQ70591H FQ70591H  " +
                        "where   FQ70591H.DOQ70IDD=FQ70594A.PEQ70IDD " +
                        " and	 FQ70594A.PEQ70AFCT=FQ70591C.MAAN8 " +
                        " and     substr((to_date('01-01-'||to_char(round(1900+(CAST(FQ70594A.PEQ70FRL as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(FQ70594A.PEQ70FRL as int)),4,3) -1),1,10) > = '" + filtro.fechaDesde + "' " +
                        " and     substr((to_date('01-01-'||to_char(round(1900+(CAST(FQ70594A.PEQ70FRL as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(FQ70594A.PEQ70FRL as int)),4,3) -1),1,10) < = '" + filtro.fechaHasta + "' " +
                        " and     TRIM(FQ70594A.PEQ70TV) IN('" + filtro.tipoVuelo + "') " +
                        " and     TRIM(FQ70594A.PEQ70EST) IN(" + filtro.estado + ") " +
                        " and     TRIM(FQ70591C.MAQ70OACI) IN('" + filtro.aerolinea + "') " +
                        "ORDER BY FQ70591C.MAALPH DESC ";

                   consulta = string.Concat(" SELECT ",
                                                    " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"FQ70594A\".\"PEQ70FRL\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"FQ70594A\".\"PEQ70FRL\" as int)),4,3) -1),1,10) AS FechaSalida, ",
                                                    " \"FQ70594A\".\"PEQ70HRL\" AS \"Hoja\", ",
                                                    " \"FQ70591H\".\"DOQ70CM\" as \"MatriculaAeronave\", ",
                                                    " \"FQ70594A\".\"PEQ70TV\" as \"TipoVuelo\", ",
                                                    " \"FQ70591C\".\"MAQ70OACI\" as \"Sigla\", ",
                                                    " \"FQ70594A\".\"PEUKID\" as \"NumeroVuelo\", ",
                                                    " \"FQ70594A\".\"PEQ70TTA\" as \"Embarc\", ",
                                                    " \"FQ70594A\".\"PEQ70TR2I\" as \"Transito\", ",
                                                    " \"FQ70594A\".\"PEQ70TRP\" as \"Linea\", ", // ?? FQ70594A.PEQ70TRP
                                                    " \"FQ70594A\".\"PEQ70TRF\" as \"Conexion\", ", // ?? FQ70594A.PEQ70TRF
                                                    " (\"FQ70594A\".\"PEQ70TTP\" - \"FQ70594A\".\"PEQ70EC\" - \"FQ70594A\".\"PEQ70TRP\") as \"Loc\", ", // ?? FQ70594A.PEQ70TTP - FQ70594A.PEQ70EC - FQ70594A.PEQ70TRP
                                                    " \"FQ70594A\".\"PEQ70TEX\" as \"Exe\", ",
                                                    " \"FQ70594A\".\"PEQ70TTI\" as \"Inf\", ",
                                                    " (\"FQ70594A\".\"PEQ70TTC\" + \"FQ70594A\".\"PEQ70TTU\" ) as \"PaganTasa\", ", // SUMAR CON PEQ70TTC EL CAMPO PEQ70TTU
                                                    " \"FQ70594A\".\"PEQ70TTC\" as \"PaganCOP\", ",
                                                    " \"FQ70594A\".\"PEQ70TTU\" as \"PaganUSD\" ", // ?? FQ70594A.PEQ70TTU
                                            " FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\", \"%pdta%\".\"FQ70594A\" \"FQ70594A\", \"%pdta%\".\"FQ70591H\" \"FQ70591H\"  ",
                                            " WHERE   \"FQ70591H\".\"DOQ70IDD\"=\"FQ70594A\".\"PEQ70IDD\" ",
                                            " and	 \"FQ70594A\".\"PEQ70AFCT\"=\"FQ70591C\".\"MAAN8\" ",
                                            //string.Format(" and     substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"FQ70594A\".\"PEQ70FRL\" as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(\"FQ70594A\".\"PEQ70FRL\" as int)),4,3) -1),1,10) > = '{0}' ", filtro.fechaDesde),
                                            string.Format(" and TRIM(\"FQ70594A\".\"PEQ70FRL\") > = '{0}' ", filtro.fechaDesde.Trim()),
                                            //string.Format(" and     substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"FQ70594A\".\"PEQ70FRL\" as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(\"FQ70594A\".\"PEQ70FRL\" as int)),4,3) -1),1,10) < = '{0}' ", filtro.fechaHasta),
                                            string.Format(" and TRIM(\"FQ70594A\".\"PEQ70FRL\") < = '{0}' ", filtro.fechaHasta.Trim()),
                                            string.Format(" and     TRIM(\"FQ70594A\".\"PEQ70TV\") IN('{0}') ", filtro.tipoVuelo.Trim()),
                                            string.Format(" and     TRIM(\"FQ70594A\".\"PEQ70EST\") IN('{0}') ", filtro.estado.Trim()));
                                            //string.Format(" and     TRIM(\"FQ70591C\".\"MAQ70OACI\") IN('{0}') ", filtro.aerolinea.Trim()));


            consulta = consulta.Replace("%pdta%", squemaData);

            return consulta;
        }
    }
}
