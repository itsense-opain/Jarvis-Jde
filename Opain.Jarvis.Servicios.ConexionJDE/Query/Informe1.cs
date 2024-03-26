using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public static class Informe1
    {        

        public static string Consulta(Repositorio.FiltroBusqueda filtro, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string Tipo = string.Empty;

            if (filtro.tipo.Equals("USD"))
            {
                Tipo = "SD";
            }

            if (filtro.tipo.Equals("COP"))
            {
                Tipo = "SB";
            }

            //Antigua
            string consulta = "";

            //Nueva
            consulta = string.Concat(" SELECT (SELECT MAQ70OACI FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"SiglaAerolinea\", ",
                                    //" CONCAT(TRIM(\"CRPDTA\".\"F42119\".sdsrp1),'30')AS PrefijoFactura, ",
                                    " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                                    " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                                    " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura, ",
                                    " \"F42119\".\"SDVR01\" as \"Matricula\", ",
                                    " (SELECT MAALPH FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"NombreAerolinea\", ",
                                    " \"FQ70591H\".\"DOQ70CVL\" as \"VueloIngreso\", ",
                                    " \"FQ70591H\".\"DOQ70CVS\" as \"VueloSalida\", ",
                                    //" to_char(to_date(concat(to_char(to_number(substr(\"FQ70591H\".\"DOQ70FLR\", 1, 3) + 1900)), substr(\"FQ70591H\".\"DOQ70FLR\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaIngreso, ",
                                    //" to_char(to_date(concat(to_char(to_number(substr(\"FQ70591H\".\"DOQ70FSP\", 1, 3) + 1900)), substr(\"FQ70591H\".\"DOQ70FSP\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaSalida, ",
                                    " to_char(to_date(concat(to_char(to_number(substr(\"FQ70591H\".\"DOQ70FLR\", 1, 3) + 1900)), substr(\"FQ70591H\".\"DOQ70FLP\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaIngreso, ",
                                    " to_char(to_date(concat(to_char(to_number(substr(\"FQ70591H\".\"DOQ70FSP\", 1, 3) + 1900)), substr(\"FQ70591H\".\"DOQ70FSP\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaSalida, ",
                                    " \"FQ70591H\".\"DOQ70HLP\" as \"HoraIngreso\", ",
                                    " \"FQ70591H\".\"DOQ70HSP\" as \"HoraSalida\", ",
                                    //" \"FQ70596A\".\"VMQ70CXP\" as \"Cantidad\", ",
                                    //" (\"F42119\".\"SDUORG\" )/100 as \"Cantidad\", ", 
                                    "(\"F42119\".\"SDPQOR\" / 100) AS Cantidad, ",
                                    " \"F42119\".\"SDUPC1\" as \"CA\", ",
                                    " \"F42119\".\"SDDOC\" AS \"Factura\", ",
                                    " \"FQ70591H\".\"DOQ70NPS\" as \"POS\", ",
                                    " (\"F42119\".\"SDUPRC\")/10000 as \"POSCobroCOP\", ",
                                    " (\"F42119\".\"SDFUP\") /10000 as \"CobroUSD\", ",
                                    //" ((\"F42119\".\"SDUPRC\")/10000)* (\"FQ70596A\".\"VMQ70CXP\")  as \"TotalCOP\", ",
                                    " ((\"F42119\".\"SDUPRC\")/10000)* ((\"F42119\".\"SDPQOR\"/100))  as \"TotalCOP\", ",
                                    //" ((\"F42119\".\"SDFUP\")/10000)* (\"FQ70596A\".\"VMQ70CXP\")  as \"TotalUSD\" ",
                                    " ((\"F42119\".\"SDFUP\")/10000)* ((\"F42119\".\"SDPQOR\"/100))  as \"TotalUSD\" ",
                                    " FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\", ",
                                    " \"%pdta%\".\"F42119\" \"F42119\", ",
                                    " \"%pdta%\".\"FQ70591H\" \"FQ70591H\", ",
                                    " \"%pdta%\".\"FQ70596A\" \"FQ70596A\" ",
                                    " WHERE \"F42119\".\"SDAN8\"=\"FQ70591C\".\"MAAN8\" ",
                                    " and \"FQ70591H\".\"DOQ70IDD\"=\"F42119\".\"SDURAT\"/100 ",
                                    " and \"FQ70596A\".\"VMQ70IDD\"=\"F42119\".\"SDURAT\"/100 " );
                                    // DOQ70FLP : Llegada de posición
                                    // DOQ70FSP : Salida Posición
                                    // DOQ70FLR : LLegada Real
                                    // DOQ70FSR : salida Real
                                    //if (filtro.fechaDesde != null && filtro.fechaHasta != null)
                                    //{
                                    //    consulta = consulta + string.Concat(string.Format(" and \"FQ70591H\".\"DOQ70FLR\"  > = '{0}' ", filtro.fechaDesde));
                                    //    consulta = consulta + string.Concat(string.Format(" and \"FQ70591H\".\"DOQ70FLR\"  < = '{0}' ", filtro.fechaHasta));
                                    //}
                                    if (filtro.fechaDesde != null && filtro.fechaHasta != null)
                                    {
                                        consulta = consulta + string.Concat(string.Format(" and \"FQ70591H\".\"DOQ70FSP\"  > = '{0}' ", filtro.fechaDesde));
                                        consulta = consulta + string.Concat(string.Format(" and \"FQ70591H\".\"DOQ70FSP\"  < = '{0}' ", filtro.fechaHasta));
                                    }

            if (filtro.aerolinea.Trim() == "0" && filtro.aerolinea.Trim() != null)
            {
                consulta = consulta + string.Concat(string.Format(" and TRIM(\"F42119\".\"SDDOC\") > =(TRIM('{0}')) ", filtro.facturaDesde.Trim()),
                                    string.Format(" and TRIM(\"F42119\".\"SDDOC\") < =(TRIM('{0}')) ", filtro.facturaHasta.Trim()),
                                    " and \"F42119\".\"SDDCT\" IN ('FP','FQ') ",
                                     " and \"F42119\".\"SDDCTO\" IN ('SB','SD') and rownum <= 100");

            }
            else
            {

                consulta = consulta + string.Concat(string.Format(" and TRIM(\"F42119\".\"SDAN8\") IN('{0}') ", filtro.aerolinea.Trim()),
                                    string.Format(" and TRIM(\"F42119\".\"SDDOC\") > =(TRIM('{0}')) ", filtro.facturaDesde.Trim()),
                                    string.Format(" and TRIM(\"F42119\".\"SDDOC\") < =(TRIM('{0}')) ", filtro.facturaHasta.Trim()),
                                    " and \"F42119\".\"SDDCT\" IN ('FP','FQ') ",
                                    " and \"F42119\".\"SDDCTO\" IN ('SB','SD')  and rownum <= 100");
            }
            if (filtro.Descarga)
            {
                consulta = consulta.Replace("and rownum <= 100", "  Order by 3");
            }
            else {
                consulta = consulta + "  Order by 3";
            }


            consulta = consulta.Replace("%pdta%", squemaData);

            return consulta;
        }

        public static string Ejemplo()
        {
            return "Select TOP 1 F42119.SDDOC as SDDOC FROM RPDTA.F42119 F42119";
        }
        public static double ConvertToJulian(DateTime Date)
        {
            double Year = Date.Year;
            double dato = (Year - 1900) * 1000 + Date.DayOfYear;
            return dato;
        }
    }
}
