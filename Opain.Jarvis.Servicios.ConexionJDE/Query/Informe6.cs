using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Opain.Jarvis.Servicios.ConexionJDE.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public  class Informe6
    {
        public static string Consulta(Repositorio.FiltroBusqueda filtro, IConfiguration config )
        {

            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string tipovuelo = filtro.tipoVuelo;
            if (tipovuelo=="NAL")
            {
                tipovuelo = "DOM";
            }
            string filtroAerolinea = "";
            if (filtro.aerolinea != null)
            {
                if (filtro.aerolinea.Trim() != "0")
                {
                    filtroAerolinea = string.Format(" and TRIM(\"FQ70591C\".\"MAAN8\") IN('{0}') ", filtro.aerolinea.Trim());
                }
            }
           

            string filtroFechas = "";
            if (filtro.fechaDesde != null && filtro.fechaHasta != null)
            {

                filtroFechas = filtroFechas + string.Concat(string.Format(" and TRIM(\"FQ70594A\".\"PEQ70FRL\")  > = '{0}'  ", filtro.fechaDesde.Trim()));
                filtroFechas = filtroFechas + string.Concat(string.Format(" and TRIM(\"FQ70594A\".\"PEQ70FRL\")   < = '{0}'  ", filtro.fechaHasta.Trim()));
            }

            string consulta = string.Concat(" SELECT ",
                                                    " \"FQ70591C\".\"MAALPH\" as \"NombreAerolinea\", ",
                                                    " to_char(to_date(concat(to_char(to_number(substr(\"FQ70594A\".\"PEQ70FRL\", 1, 3) + 1900)), substr(\"FQ70594A\".\"PEQ70FRL\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaVuelo, ",
                                                    //" substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"FQ70594A\".\"PEQ70FRL\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"FQ70594A\".\"PEQ70FRL\" as int)),4,3) -1),1,10) AS FechaVuelo, ",
                                                    " \"FQ70594A\".\"PEQ70HRL\" as \"Hoja\", ",
                                                    //" \"FQ70591H\".\"DOQ70CM\" AS \"Matricula\", ",
                                                    " \"FQ70594A\".\"PEQ70CM\" AS \"Matricula\", ",
                                                    " \"FQ70591C\".\"MAQ70OACI\" as \"Sigla\", ",
                                                    " \"FQ70594A\".\"PEQ70CVO\" as \"CodigoVuelo\", ",
                                                    " \"FQ70594A\".\"PEQ70TV\" as \"TipoVuelo\", ",
                                                    " \"FQ70594A\".\"PEQ70TTA\" as \"Embarcados\", ",
                                                    " \"FQ70594A\".\"PEQ70TRP\" + \"FQ70594A\".\"PEQ70TRF\" as \"Transito\", ",
                                                    " \"FQ70594A\".\"PEQ70TRP\" as \"TransitoLinea\", ",
                                                    " \"FQ70594A\".\"PEQ70TRF\" as \"TransitoConexion\", ",
                                                    " (\"FQ70594A\".\"PEQ70TTA\") - (\"FQ70594A\".\"PEQ70TRP\" + \"FQ70594A\".\"PEQ70TRF\") as \"Local\", ",// ?? (FQ70594A.PEQ70TTP - FQ70594A.PEQ70EC - FQ70594A.PEQ70TRP)
                                                    " \"FQ70594A\".\"PEQ70TEX\" as \"Exento\", ",
                                                    " \"FQ70594A\".\"PEQ70EC\" AS \"Tripulantes\", ",
                                                    " \"FQ70594A\".\"PEQ70TEX\" + \"FQ70594A\".\"PEQ70EC\" AS \"Exento_2\", ",
                                                    " \"FQ70594A\".\"PEQ70TTI\" as \"Infantes\", ",
                                                    " \"FQ70594A\".\"PEQ70EC\" as \"Tripulacion\", ",
                                                    " \"FQ70594A\".\"PEQ70TTC\" + \"FQ70594A\".\"PEQ70TTU\" as \"PaganTasa\", ", // SUMAR CON PEQ70TTC EL CAMPO PEQ70TTU
                                                    " \"FQ70594A\".\"PEQ70TTC\" as \"PaganCOP\", ",
                                                    " \"FQ70594A\".\"PEQ70TTU\" as \"PaganUSD\", ",// ?? FQ70594A.PEQ70TTU
                                                    " \"FQ70594A\".\"PEQ70IDP\" as \"ID_PAX_AUDITORIA\", ",
                                                    " \"FQ70594A\".\"PEQ70IDD\" as \"ID_DAILY_AUDITORIA\" ",
                                            " FROM	\"%pdta%\".\"FQ70591C\" \"FQ70591C\", \"%pdta%\".\"FQ70594A\" \"FQ70594A\", \"%pdta%\".\"FQ70591H\" \"FQ70591H\" ",
                                            " WHERE   \"FQ70591H\".\"DOQ70IDD\"=\"FQ70594A\".\"PEQ70IDD\" ",
                                            " and	 \"FQ70594A\".\"PEQ70AFCT\"=\"FQ70591C\".\"MAAN8\" ",
                                            filtroFechas,
                                            filtroAerolinea,                                          
                                            string.Format(" and     TRIM(\"FQ70594A\".\"PEQ70TV\") IN('{0}') ", tipovuelo.Trim()),
                                            string.Format(" and     TRIM(\"FQ70594A\".\"PEQ70EST\") IN('{0}') ", filtro.estado.Trim()),
                                            " and rownum <= 100 ",
                                            " ORDER BY \"FQ70591C\".\"MAALPH\", \"FQ70594A\".\"PEQ70TV\", \"FQ70594A\".\"PEQ70FRL\" ");
            
            consulta = consulta.Replace("%pdta%", squemaData);
            if (filtro.Descarga)
            {
                consulta = consulta.Replace("and rownum <= 100", "");
            }  

            return consulta;
        }



    }
}
