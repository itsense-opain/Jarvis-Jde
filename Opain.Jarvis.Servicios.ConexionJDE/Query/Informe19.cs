using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public class Informe19
    {
        public static string Consulta(Repositorio.FiltroBusqueda filtro, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);


            // consulta anterior
            //string consulta = "SELECT " +
            //                            "CASE  " +
            //                            "   WHEN  DDURDT > = 120001 THEN DDURDT " +
            //                            "   ELSE 110001 " +
            //                            "END as INDICADOR,	 " +
            //                            "	F5903004.DDTRQT as DDTRQT, " +
            //                            "	F5903004.DDAEXP as DDAEXP, " +
            //                            "     F5903004.DDDOC as DDDOC, " +
            //                            "	 F5903004.DDODOC as DocumentoFactura, " +
            //                            "	 F5903004.DDTAX  as IdQuienRecibe, " +
            //                            "	 F5903004.DDALPH as Cliente, " +
            //                            "	 F5903004.DDURDT as DDURDT, " +
            //                            "	 F4101.IMDSC1 as  TipoCarnet, " +
            //                            "	 F4101.IMDSC2 as  TipoRadicado, " +
            //                            "	 substr(F5903004.DD59REQUE, 1, 8) as reque, " +
            //                            "	 F5903001.HRIR01  as Radicado, " +
            //                            "	 F5903001.HRPA8 as HRPA8, " +
            //                            "	 F0101.ABALPH as ABALPH, " +
            //                            " to_char(to_date(concat(to_char(to_number(substr(\"F5903004\".\"DDUPMJ\", 1, 3) + 1900)), substr(\"F5903004\".\"DDUPMJ\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaAuditoria " +
            //                            //" 	 substr((to_date('01-01-'||to_char(round(1900+(CAST(F5903004.DDUPMJ as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(F5903004.DDUPMJ as int)),4,3) -1),1,10) AS FechaAuditoria	 " +
            //                            "FROM	    %pdta%.F5903004 F5903004, " +
            //                            "	    %pdta%.F4101 F4101, " +
            //                            "	    %pdta%.F0101 F0101, " +
            //                            "	    %pdta%.F5903001 F5903001 " +
            //                            "WHERE  F5903004.DDITM=F4101.IMITM " +
            //                            "and     substr((to_date('01-01-'||to_char(round(1900+(CAST(F5903004.DDUPMJ as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(F5903004.DDUPMJ as int)),4,3) -1),1,10) > = '" + filtro.fechaDesde + "' " +
            //                            "and     substr((to_date('01-01-'||to_char(round(1900+(CAST(F5903004.DDUPMJ as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(F5903004.DDUPMJ as int)),4,3) -1),1,10) < = '" + filtro.fechaHasta + "' " +
            //                            "and     substr(F5903004.DD59REQUE, 1, 8) = F5903001.HRDOCO " +
            //                            "and     F5903001.HRPA8=F0101.ABAN8  " +
            //                            " and rownum < 100";

            // consulta nueva
            string consulta = " SELECT DISTINCT CASE   WHEN  DDURDT > = 120001 THEN DDURDT   ELSE 110001 END as INDICADOR, " +
                    "substr((to_date('01-01-' || to_char(round(1900 + (CAST(F5903004.DDUPMJ as int) / 1000))), 'DD-MM-YYYY') + substr(to_char(CAST(F5903004.DDUPMJ as int)), 4, 3) - 1), 1, 10) AS FechaAuditoria," +
                     "CONCAT(' ', 'CAR') as DocumentoFactura, " +
                    "F5903004.DDDOC as NumeroFactura, " +
                    "CONCAT(' ', 'ANC') as DocumentoPago," +
                    "F5903004.DDODOC as NumeroPago," +
                    "F5903001.HRIR01 as Radicado," +
                    "F0101.ABALPH as Cliente, " +
                    "F4101.IMDSC1 as TipoCarnet, " +
                    "F5903004.DDTAX as IdQuienRecibe, " +
                    "F5903004.DDALPH as NombreQuienRecibe," +
                    "F5903004.DDTRQT / 100 as Entregados," +
                    "F5903004.DDAEXP / 100 as Valor," +
                    "CONCAT(' ', 'Impreso') as EstadoImpresion," +
                    "F4101.IMDSC2 as TipoRadicado " +
                    "FROM   %pdta%.F5903004 F5903004, %pdta%.F4101 F4101, %pdta%.F0101 F0101,  %pdta%.F5903001 F5903001,%pdta%.F03B11 F03B11,%pdta%.F5903002 F5903002 " +
                   "WHERE  F5903004.DDITM = F4101.IMITM"+
                   " and substr(F5903004.DD59REQUE,1, 8) = F5903001.HRDOCO"+
                   " and     F5903001.HRUPMJ > = '120060'"+
                   " and     F5903001.HRPA8 = F0101.ABAN8"+
                   " and substr(F5903004.DD59REQUE,1, 8) = F5903002.DRDOCO" +
                   " AND F5903004.DDUPMJ >= '" + filtro.fechaDesde + "' " +
                   " AND F5903004.DDUPMJ <= '" + filtro.fechaHasta + "' " +
                   " AND   F03B11.RPDOC = F5903004.DDDOC "+
                   " AND   F03B11.RPDCT = 'FC' ";

            if (filtro.Descarga)
            {
                consulta = consulta.Replace("and rownum <= 100", "");
            }


            consulta = consulta.Replace("%pdta%", squemaData);

            return consulta;
        }
    }
}
