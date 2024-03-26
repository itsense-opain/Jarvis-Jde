using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public class Informe7A
    {
        public static string Consulta(IConfiguration config)
        {
            string squemaControl = string.Empty;
            squemaControl = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Control", config);

            string consulta = "SELECT F0005.DRDL01 as DRDL01, " +
                            "       F0005.DRDL02 as DRDL02, " +
                            "       TRIM(F0005.DRKY) as DRKY, " +
                            "       F0005.DRSY as DRSY, " +
                            "       F0005.DRRT as DRRT " +
                            "FROM	%crpctl%.F0005 F0005 " +
                            "WHERE	F0005.DRSY IN ('00') " +
                            "AND		F0005.DRRT IN ('DT') ";


            consulta = consulta.Replace("%crpctl%", squemaControl);

            return consulta;
        }
    }

    public class Informe7B
    {
        public static string Consulta(Repositorio.FiltroBusqueda filtro,IConfiguration config)
        {

            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string filtroaerolinea = "";
            if (filtro.aerolinea.Trim() != "0")
            {
                filtroaerolinea = string.Format(" and     TRIM(\"FQ70591C\".\"MAAN8\") IN('{0}') ", filtro.aerolinea.Trim());
            }

            var consulta = string.Concat(" SELECT ",
                                                " (SELECT MAQ70OACI FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"Sigla\", ",
                                                " \"F0101\".\"ABTAX\" as \"NIT\", ",
                                                " \"FQ70591C\".\"MAALPH\" as \"NombreAerolinea\", ",
                                                " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDTRDJ\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"F42119\".\"SDTRDJ\" as int)),4,3) -1),1,10) AS FechaVuelo, ",
                                                " \"F42119\".\"SDVR01\" as \"Matricula\", ",
                                                " \"F42119\".\"SDDSC2\" as \"NumeroVuelo\", ",
                                                " \"F42119\".\"SDUPRC\" as \"TarifaCOP\", ", // ?? F42119.SDUPRC
                                                " \"F42119\".\"SDFUP\" as \"TarifaUSD\", ", // ?? F42119.SDFUP
                                                " \"F42119\".\"SDZON\" as \"TipoVuelo\", ",
                                                " CASE ",
                                                    " WHEN SDDCT IN('FV', 'FA') and SDDCTO IN('S1', 'S2')   THEN SDUORG/100 ",
                                                    " ELSE 0 ",
                                                " END as NumNormales, ",
                                                " CASE ",
                                                    " WHEN SDDCT IN('FV', 'FA') and SDDCTO IN('S3', 'SE')   THEN SDUORG/100 ",
                                                    " ELSE 0 ",
                                                " END as NumNormalesUSD, ",
                                                " CASE ",
                                                    " WHEN SDDCT IN('FV', 'FA') and SDDCTO IN('S1', 'S2')   THEN SDUPRC/10000 ",
                                                    " ELSE 0 ",
                                                " END as CobroCOP, ",
                                                " CASE ",
                                                    " WHEN SDDCT IN('FV', 'FA') and SDDCTO IN('S3', 'SE') THEN SDFUP ",
                                                    " ELSE 0 ",
                                                " END as CobroUSD, ",
                                                " \"F42119\".\"SDDOC\" AS \"Factura\", ",
                                                "  CASE WHEN SDDCT IN('FV', 'FA') AND SDDCTO IN 'S1' THEN 'Tasa Aeroportuaria Paga en Pesos' WHEN SDDCT IN('FV', 'FA') AND SDDCTO IN 'S2' THEN 'Tasa Aeroportuaria Paga en Pesos' WHEN SDDCT IN('FV', 'FA') AND SDDCTO IN 'S3' THEN 'Tasa Aeroportuaria Paga en Dolares' WHEN SDDCT IN('FV', 'FA') AND SDDCTO IN 'SE' THEN 'Mostradores' END AS \"TipoFactura\" ",
                                                " FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\", \"%pdta%\".\"F42119\" \"F42119\", \"%pdta%\".\"F0101\" \"F0101\" ",
                                                " WHERE   \"F42119\".\"SDAN8\"=\"FQ70591C\".\"MAAN8\" ",
                                                " and     \"F42119\".\"SDAN8\"=\"F0101\".\"ABAN8\" ",
                                                " and   \"F42119\".\"SDAN8\"=\"F0101\".\"ABAN8\" ",
                                                string.Format(" and TRIM(\"F42119\".\"SDTRDJ\") > = '{0}' ", filtro.fechaDesde.Trim()),                                           
                                                string.Format(" and TRIM(\"F42119\".\"SDTRDJ\") < = '{0}' ", filtro.fechaHasta.Trim()),
                                                filtroaerolinea,
                                                " and     \"F42119\".\"SDDCT\" IN ('FV','FA') ",
                                                string.Format(" and     TRIM(\"F42119\".\"SDZON\") IN('{0}') ", (filtro.tipoVuelo.Trim()=="NAL")?"DOM": filtro.tipoVuelo.Trim()),
                                                " and     \"F42119\".\"SDDCTO\" IN ('S1','S2','S3','SE')  and rownum <= 100 ",
                                                " UNION ",
                                                " SELECT ",
                                                " (SELECT MAQ70OACI FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"Sigla\", ",
                                                " \"F0101\".\"ABTAX\" as \"NIT\", ",
                                                " \"FQ70591C\".\"MAALPH\" as \"NombreAerolinea\", ",
                                                " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDTRDJ\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"F42119\".\"SDTRDJ\" as int)),4,3) -1),1,10) AS FechaVuelo, ",
                                                " \"F42119\".\"SDVR01\" as \"Matrícula\", ",
                                                " \"F42119\".\"SDDSC2\" as \"NumeroVuelo\", ",
                                                " \"F42119\".\"SDUPRC\" as \"TarifaCOP\", ", // ?? F42119.SDUPRC
                                                " \"F42119\".\"SDFUP\" as \"TarifaUSD\", ", // ?? F42119.SDFUP
                                                " \"F42119\".\"SDZON\" as \"TipoVuelo\", ",
                                                " CASE ",
                                                    " WHEN  SDDCT IN('FB','UB') and SDDCTO IN('CA', 'CB') AND  SDFUP = 0 THEN SDUORG/100 ",
                                                    " ELSE 0 ",
                                                " END as NumNormales, ",
                                                " CASE ",
                                                    " WHEN  SDDCT IN('FB','UB') and SDDCTO IN('CA', 'CB') AND  SDFUP != 0 THEN SDUORG/100 ",
                                                    " ELSE 0 ",
                                                " END as NumNormalesUSD, ",
                                                " CASE ",
                                                    " WHEN  SDDCT IN('FB','UB') and SDUPRC > 0 and SDDCTO IN('CA', 'CB') AND SDFUP = 0 THEN SDUPRC/10000 ",
                                                    " ELSE 0 ",
                                                " END as CobroCOP, ",
                                                " CASE ",
                                                    " WHEN  SDDCT IN('FB') and SDFUP > 0 and SDDCTO IN('CA', 'CB') AND SDFUP != 0 THEN SDFUP/100 ",
                                                    " ELSE 0 ",
                                                " END as CobroUSD, ",
                                                " \"F42119\".\"SDDOC\" AS \"Factura\", ",
                                                "  CASE WHEN SDDCT IN('FB', 'UB') AND SDDCTO IN('CA', 'CB') THEN 'Nota Credito' END AS \"TipoFactura\" ",
                                                " FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\", ",
                                                        " \"%pdta%\".\"F42119\" \"F42119\", ",
                                                        " \"%pdta%\".\"F0101\" \"F0101\" ",
                                                " WHERE   \"F42119\".\"SDAN8\"=\"FQ70591C\".\"MAAN8\" ",
                                                " and     \"F42119\".\"SDAN8\"=\"F0101\".\"ABAN8\" ",
                                              string.Format(" and TRIM(\"F42119\".\"SDTRDJ\") > = '{0}' ", filtro.fechaDesde.Trim()),
                                                string.Format(" and TRIM(\"F42119\".\"SDTRDJ\") < = '{0}' ", filtro.fechaHasta.Trim()),
                                                filtroaerolinea,
                                                " and     \"F42119\".\"SDDCT\" IN ('FB','UB') ",
                                                string.Format(" and     TRIM(\"F42119\".\"SDZON\") IN('{0}') ", (filtro.tipoVuelo.Trim() == "NAL") ? "DOM" : filtro.tipoVuelo.Trim()),
                                                " and     \"F42119\".\"SDDCTO\" IN ('CA','CB') and rownum <= 100 ");
            if (filtro.Descarga)
            {
                consulta = consulta.Replace("and rownum <= 100", "");
                consulta = consulta + " Order by 3";
            } else
            {
                consulta = consulta + " Order by 3";
            }


            consulta = consulta.Replace("%pdta%", squemaData);

            return consulta;
        }
    }
}