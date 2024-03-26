using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public class Informe2
    {
        public static string Consulta(Repositorio.FiltroBusqueda filtro, IConfiguration config)
        {

            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consulta = "";

            string filtroFechas = "";
            if (filtro.fechaDesde != null && filtro.fechaHasta != null)
            {
                
                //filtroFechas = filtroFechas + string.Concat(string.Format(" and \"F42119\".\"SDTRDJ\"  > = '{0}' ",filtro.fechaDesde.ToString()));
                //filtroFechas = filtroFechas + string.Concat(string.Format(" and \"F42119\".\"SDTRDJ\"  < = '{0}' ", filtro.fechaHasta.ToString()));
                filtroFechas = filtroFechas + string.Concat(string.Format(" and \"F42119\".\"SDDRQJ\"  > = '{0}' ", filtro.fechaDesde.ToString()));
                filtroFechas = filtroFechas + string.Concat(string.Format(" and \"F42119\".\"SDDRQJ\"  < = '{0}' ", filtro.fechaHasta.ToString()));
            }

            string filtroAerolinea = "";
            if (filtro.aerolinea.Trim() != "0")
            {
                filtroAerolinea = string.Format(" and TRIM(\"F42119\".\"SDAN8\") IN('{0}') ", filtro.aerolinea.Trim());
            }   

            switch (filtro.tipo)
            {
                case "DOM_COP":
                    
                    consulta = string.Concat("SELECT  ",
                                            " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                                            " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                                            " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura, ",
                                            " \"F42119\".\"SDDOC\" AS \"Factura\",",
                                            " \"F42119\".\"SDVR01\" as \"Matricula\", ",
                                            " (SELECT MAQ70OACI FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"OACIAerolinea\", ",
                                            " (SELECT MAALPH FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"NombreAerolinea\", ",
                                            " \"FQ70591H\".\"DOQ70CVS\" as \"VueloSalida\", ",
                                            " \"FQ70591H\".\"DOQ70CVL\" as \"VueloIngreso\", ",
                                            " to_char(to_date(concat(to_char(to_number(substr(\"FQ70591H\".\"DOQ70FLP\", 1, 3) + 1900)), substr(\"FQ70591H\".\"DOQ70FLP\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaIngreso, ",
                                            " to_char(to_date(concat(to_char(to_number(substr(\"F42119\".\"SDTRDJ\", 1, 3) + 1900)), substr(\"F42119\".\"SDTRDJ\", 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FechaSalida, ",
                                            //" substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"FQ70591H\".\"DOQ70FLP\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"FQ70591H\".\"DOQ70FLP\" as int)),4,3) -1),1,10) AS FechaIngreso, ",
                                            //" substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDTRDJ\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"F42119\".\"SDTRDJ\" as int)),4,3) -1),1,10) AS FechaSalida, ",
                                            " \"FQ70591H\".\"DOQ70HLP\" as \"HoraIngreso\", ",
                                            " \"FQ70591H\".\"DOQ70HSP\" as \"HoraSalida\", ",
                                            " (\"F42119\".\"SDUORG\")/100 AS \"TotalHoras\",  ",
                                            " \"F42119\".\"SDUPC1\" as \"CA\", ",// tabla.campo => F42119.SDLCOD
                                            " \"FQ70591H\".\"DOQ70NPS\" as \"POS\", ",
                                            " \"F42119\".\"SDDSC1\" as \"TipoConexion\", ",
                                            " (\"F42119\".\"SDUPRC\")/10000 AS \"CobroCOP\", ",
                                            " ((\"F42119\".\"SDUORG\")/100)*((\"F42119\".\"SDUPRC\")/10000) AS \"TotalCOP\" ",
                                            " FROM \"%pdta%\".\"F42119\" \"F42119\", \"%pdta%\".\"FQ70591H\" \"FQ70591H\", \"%pdta%\".\"FQ70591C\" \"FQ70591C\" ",
                                            " WHERE   \"F42119\".\"SDAN8\"=\"FQ70591C\".\"MAAN8\" ",
                                            " and     \"FQ70591H\".\"DOQ70IDD\"=\"F42119\".\"SDURAT\"/100 ",
                                            filtroFechas +
                                            filtroAerolinea  +
                                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") > =(TRIM('{0}')) ", filtro.facturaDesde.Trim()),
                                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") < =(TRIM('{0}')) ", filtro.facturaHasta.Trim()),
                                            " and     \"F42119\".\"SDDCT\" IN ('FP','FQ') ",
                                            " and     \"F42119\".\"SDDCTO\" IN ('S4','S6','S8','S0','M5','M7','M8') ",
                                            " and     \"F42119\".\"SDZON\" IN ('DOM') ",
                                            " and rownum <= 100 ");
                    break;
                case "DOM_USD":

                    consulta = string.Concat(" SELECT ",
                                            " \"F42119\".\"SDDOC\" AS \"Factura\", ",
                                            " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                                            " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                                            " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura, ",
                                            " \"F42119\".\"SDVR01\" as \"Matricula\", ",
                                            " (SELECT MAQ70OACI FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"OACIAerolinea\", ",
                                            " (SELECT MAALPH FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"NombreAerolinea\", ",
                                            " \"FQ70591C\".\"MAQ70OACI\" AS Sigla, ",
                                            " \"FQ70591H\".\"DOQ70CVS\" as \"VueloIngreso\", ",
                                            " \"FQ70591H\".\"DOQ70CVL\" as \"VueloSalida\", ",
                                            " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"FQ70591H\".\"DOQ70FLP\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"FQ70591H\".\"DOQ70FLP\" as int)),4,3) -1),1,10) AS FechaIngreso, ",
                                            " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDTRDJ\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"F42119\".\"SDTRDJ\" as int)),4,3) -1),1,10) AS FechaSalida, ",
                                            " \"FQ70591H\".\"DOQ70HLP\" as \"HoraIngreso\", ",
                                            " \"FQ70591H\".\"DOQ70HSP\" as \"HoraSalida\", ",
                                            " (\"F42119\".\"SDUORG\")/100 AS \"TotalHoras\",  ",
                                            " \"F42119\".\"SDUPC1\" as \"CA\", ",
                                            " \"FQ70591H\".\"DOQ70NPS\" as \"POS\", ",
                                            " \"F42119\".\"SDDSC1\" as \"TipoConexion\", ",
                                            " (\"F42119\".\"SDFUP\")/10000 AS \"CobroUSD\", ",
                                            //" ((\"F42119\".\"SDUORG\")/100)*((\"F42119\".\"SDFUP\")/100) AS \"TotalUSD\" ",
                                            "(\"F42119\".\"SDFEA\")/100 as TotalUSD ",

                                            " FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\", \"%pdta%\".\"F42119\" \"F42119\", \"%pdta%\".\"FQ70591H\" \"FQ70591H\" ",
                                            " WHERE   \"F42119\".\"SDAN8\"=\"FQ70591C\".\"MAAN8\" ",
                                            " and     \"FQ70591H\".\"DOQ70IDD\"=\"F42119\".\"SDURAT\"/100 ",
                                            //string.Format(" and     substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDIVD\" as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(\"F42119\".\"SDIVD\" as int)),4,3) -1),1,10) > = '{0}' ", filtro.fechaDesde),
                                            //string.Format(" and     substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDIVD\" as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(\"F42119\".\"SDIVD\" as int)),4,3) -1),1,10) < = '{0}' ", filtro.fechaHasta),
                                            filtroFechas, 
                                            filtroAerolinea,
                                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") > =(TRIM('{0}')) ", filtro.facturaDesde.Trim()),
                                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") < =(TRIM('{0}')) ", filtro.facturaHasta.Trim()),
                                            " and     \"F42119\".\"SDDCT\" IN ('FP','FQ') ",
                                            " and     \"F42119\".\"SDDCTO\" IN ('S5','S7','M6', 'M7','M8') ",
                                            " and     \"F42119\".\"SDZON\" IN ('DOM') ",
                                            " and rownum <= 100 ");
                    break;
                case "INT_COP":

                    
                    consulta = string.Concat("SELECT ",
                                            " \"F42119\".\"SDDOC\" AS \"Factura\", ",
                                            " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                                            " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                                            " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura, ",
                                            " \"F42119\".\"SDVR01\" as \"Matricula\", ",
                                            " (SELECT MAQ70OACI FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"OACIAerolinea\", ",
                                            " (SELECT MAALPH FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"NombreAerolinea\", ",
                                            " \"FQ70591C\".\"MAQ70OACI\" AS Sigla, ",
                                            " \"FQ70591H\".\"DOQ70CVS\" as \"VueloSalida\", ",
                                            " \"FQ70591H\".\"DOQ70CVL\" as \"VueloIngreso\", ",
                                            " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"FQ70591H\".\"DOQ70FLP\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"FQ70591H\".\"DOQ70FLP\" as int)),4,3) -1),1,10) AS FechaIngreso, ",
                                            " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDTRDJ\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"F42119\".\"SDTRDJ\" as int)),4,3) -1),1,10) AS FechaSalida, ",
                                            " \"FQ70591H\".\"DOQ70HLP\" as \"HoraIngreso\", ",
                                            " \"FQ70591H\".\"DOQ70HSP\" as \"HoraSalida\", ",
                                            " (\"F42119\".\"SDUORG\")/100 AS \"TotalHoras\",  ",
                                            " \"F42119\".\"SDUPC1\" as \"CA\", ",
                                            " \"FQ70591H\".\"DOQ70NPS\" as \"POS\", ",
                                            " \"F42119\".\"SDDSC1\" as \"TipoConexion\", ",
                                            " (\"F42119\".\"SDUPRC\")/10000 AS \"CobroCOP\", ",
                                            " ((\"F42119\".\"SDUORG\")/100)*((\"F42119\".\"SDUPRC\")/10000) AS \"TotalCOP\" ",
                                            " FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\", \"%pdta%\".\"F42119\" \"F42119\",  \"%pdta%\".\"FQ70591H\" \"FQ70591H\" ",
                                            " WHERE   \"F42119\".\"SDAN8\"=\"FQ70591C\".\"MAAN8\" ",
                                            " and     \"FQ70591H\".\"DOQ70IDD\"=\"F42119\".\"SDURAT\"/100 ",
                                            //string.Format(" and     substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDIVD\" as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(\"F42119\".\"SDIVD\" as int)),4,3) -1),1,10) > = '{0}' ", filtro.fechaDesde),
                                            //string.Format(" and     substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDIVD\" as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(\"F42119\".\"SDIVD\" as int)),4,3) -1),1,10) < = '{0}' ", filtro.fechaHasta),
                                            filtroFechas ,
                                            filtroAerolinea,
                                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") > =(TRIM('{0}')) ", filtro.facturaDesde.Trim()),
                                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") < =(TRIM('{0}')) ", filtro.facturaHasta.Trim()),
                                            " and     \"F42119\".\"SDDCT\" IN ('FP','FQ') ",
                                            " and     \"F42119\".\"SDDCTO\" IN ('S4','S6','S8','S0','M5','M7','M8') ",
                                            " and     \"F42119\".\"SDZON\" IN ('INT') ",
                                            " and rownum <= 100");
                    break;
                case "INT_USD":

                    consulta = string.Concat(" SELECT  ",
                                            " \"F42119\".\"SDDOC\" AS \"Factura\", ",
                                            " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                                            " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                                            " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura, ",
                                            " \"F42119\".\"SDVR01\" as \"Matricula\", ",
                                            " (SELECT MAQ70OACI FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"OACIAerolinea\", ",
                                            " (SELECT MAALPH FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"NombreAerolinea\", ",
                                            " \"FQ70591C\".\"MAQ70OACI\" AS Sigla, ",
                                            " \"FQ70591H\".\"DOQ70CVS\" as \"VueloSalida\", ",
                                            " \"FQ70591H\".\"DOQ70CVL\" as \"VueloIngreso\", ",
                                            " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"FQ70591H\".\"DOQ70FLP\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"FQ70591H\".\"DOQ70FLP\" as int)),4,3) -1),1,10) AS FechaIngreso, ",
                                            " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDTRDJ\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"F42119\".\"SDTRDJ\" as int)),4,3) -1),1,10) AS FechaSalida, ",
                                            " \"FQ70591H\".\"DOQ70HLP\" as \"HoraIngreso\", ",
                                            " \"FQ70591H\".\"DOQ70HSP\" as \"HoraSalida\", ",
                                            " (\"F42119\".\"SDUORG\")/100 AS \"TotalHoras\",  ",
                                            " \"F42119\".\"SDUPC1\" as \"CA\", ",
                                            " \"FQ70591H\".\"DOQ70NPS\" as \"POS\", ",
                                            " \"F42119\".\"SDDSC1\" as \"TipoConexion\", ",
                                            " (\"F42119\".\"SDFUP\")/10000 AS \"CobroUSD\", ",
                                             //" ((\"F42119\".\"SDUORG\")/100)*((\"F42119\".\"SDFUP\")/100) AS \"TotalUSD\" ",
                                             "(\"F42119\".\"SDFEA\")/100 as TotalUSD ",
                                            " FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\", \"%pdta%\".\"F42119\" \"F42119\", \"%pdta%\".\"FQ70591H\" \"FQ70591H\" ",
                                            " WHERE   \"F42119\".\"SDAN8\"=\"FQ70591C\".\"MAAN8\" ",
                                            " and     \"FQ70591H\".\"DOQ70IDD\"=\"F42119\".\"SDURAT\"/100 ",
                                            //string.Format(" and     substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDIVD\" as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(\"F42119\".\"SDIVD\" as int)),4,3) -1),1,10) > = '{0}' ", filtro.fechaDesde),
                                            //string.Format(" and     substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDIVD\" as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(\"F42119\".\"SDIVD\" as int)),4,3) -1),1,10) < = '{0}' ", filtro.fechaHasta),
                                            filtroFechas ,
                                            filtroAerolinea,
                                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") > =(TRIM('{0}')) ", filtro.facturaDesde.Trim()),
                                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") < =(TRIM('{0}')) ", filtro.facturaHasta.Trim()),
                                            " and     \"F42119\".\"SDDCT\" IN ('FP','FQ') ",
                                            " and     \"F42119\".\"SDDCTO\" IN ('S9','SA','M6', 'M7','M8') ",
                                            " and     \"F42119\".\"SDZON\" IN ('INT') ",
                                            " and rownum <= 100");
                    break;
                case "TOT_COP":

                    consulta = string.Concat(" SELECT ",
                                            " \"F42119\".\"SDDOC\" AS \"Factura\", ",
                                            " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                                            " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                                            " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura, ",
                                            " \"F42119\".\"SDVR01\" as \"Matricula\", ",
                                            " (SELECT MAQ70OACI FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"OACIAerolinea\", ",
                                            " (SELECT MAALPH FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"NombreAerolinea\", ",
                                            " \"FQ70591C\".\"MAQ70OACI\" AS Sigla, ",
                                            " \"FQ70591H\".\"DOQ70CVS\" as \"VueloSalida\", ",
                                            " \"FQ70591H\".\"DOQ70CVL\" as \"VueloIngreso\", ",
                                            " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"FQ70591H\".\"DOQ70FLP\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"FQ70591H\".\"DOQ70FLP\" as int)),4,3) -1),1,10) AS FechaIngreso, ",
                                            " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDTRDJ\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"F42119\".\"SDTRDJ\" as int)),4,3) -1),1,10) AS FechaSalida, ",
                                            " \"FQ70591H\".\"DOQ70HLP\" as \"HoraIngreso\", ",
                                            " \"FQ70591H\".\"DOQ70HSP\" as \"HoraSalida\", ",
                                            " (\"F42119\".\"SDUORG\")/100 AS \"TotalHoras\",  ",
                                            " \"F42119\".\"SDUPC1\" as \"CA\", ",
                                            " \"FQ70591H\".\"DOQ70NPS\" as \"POS\", ",
                                            " \"F42119\".\"SDDSC1\" as \"TipoConexion\", ",
                                            " (\"F42119\".\"SDUPRC\")/10000 AS \"CobroCOP\", ",
                                            " ((\"F42119\".\"SDUORG\")/100)*((\"F42119\".\"SDUPRC\")/10000) AS \"TotalCOP\" ",
                                            " FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\", \"%pdta%\".\"F42119\" \"F42119\", \"%pdta%\".\"FQ70591H\" \"FQ70591H\" ",
                                            " WHERE   \"F42119\".\"SDAN8\"=\"FQ70591C\".\"MAAN8\" ",
                                            " and   \"FQ70591H\".\"DOQ70IDD\"=\"F42119\".\"SDURAT\"/100 ",
                                            
                                            filtroFechas ,
                                            filtroAerolinea,
                                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") > =(TRIM('{0}')) ", filtro.facturaDesde.Trim()),
                                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") < =(TRIM('{0}')) ", filtro.facturaHasta.Trim()),
                                            " and     \"F42119\".\"SDDCT\" IN ('FP','FQ') ",
                                            " and   \"F42119\".\"SDDCTO\" IN ('S4','S6','S8','S0', 'M5','M7','M8') ",
                                            " and   \"F42119\".\"SDZON\" IN ('INT', 'DOM') ",
                                            " and rownum <= 100 ");
                    break;
                case "TOT_USD":

                    consulta = string.Concat(" SELECT ",
                                            " \"F42119\".\"SDDOC\" AS \"Factura\", ",
                                            " \"%pdta%\".\"F42119\".SDDCT as TDocFact, ",
                                            " \"%pdta%\".\"F42119\".SDDCTO as TOrdFact, ",
                                            " \"%pdta%\".JARVIS_PREFIJO_FACT(\"%pdta%\".\"F42119\".SDDCT,\"%pdta%\".\"F42119\".SDDCTO) AS PrefijoFactura, ",
                                            " \"F42119\".\"SDVR01\" as \"Matricula\", ",
                                            " (SELECT MAQ70OACI FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"OACIAerolinea\", ",
                                            " (SELECT MAALPH FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN8 = \"F42119\".\"SDAN8\") as \"NombreAerolinea\", ",
                                            " \"FQ70591C\".\"MAQ70OACI\" AS Sigla, ",
                                            " \"FQ70591H\".\"DOQ70CVS\" as \"VueloSalida\", ",
                                            " \"FQ70591H\".\"DOQ70CVL\" as \"VueloIngreso\", ",
                                            " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"FQ70591H\".\"DOQ70FLP\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"FQ70591H\".\"DOQ70FLP\" as int)),4,3) -1),1,10) AS FechaIngreso, ",
                                            " substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDTRDJ\" as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(\"F42119\".\"SDTRDJ\" as int)),4,3) -1),1,10) AS FechaSalida, ",
                                            " \"FQ70591H\".\"DOQ70HLP\" as \"HoraIngreso\", ",
                                            " \"FQ70591H\".\"DOQ70HSP\" as \"HoraSalida\", ",
                                            " (\"F42119\".\"SDUORG\")/100 AS \"TotalHoras\",  ",
                                            " \"F42119\".\"SDUPC1\" as \"CA\", ",
                                            " \"FQ70591H\".\"DOQ70NPS\" as \"POS\", ",
                                            " \"F42119\".\"SDDSC1\" as \"TipoConexion\", ",
                                            " (\"F42119\".\"SDFUP\")/10000 AS \"CobroUSD\", ",
                                            //" ((\"F42119\".\"SDUORG\")/100)*((\"F42119\".\"SDFEA\")/100) AS \"TotalUSD\" ",
                                            "(\"F42119\".\"SDFEA\")/100 as TotalUSD ",
                                            " FROM	\"%pdta%\".\"FQ70591C\" \"FQ70591C\", \"%pdta%\".\"F42119\" \"F42119\", \"%pdta%\".\"FQ70591H\" \"FQ70591H\" ",
                                            " WHERE   \"F42119\".\"SDAN8\"=\"FQ70591C\".\"MAAN8\" ",
                                            " and 	\"FQ70591H\".\"DOQ70IDD\"=\"F42119\".\"SDURAT\"/100 ",
                                            //string.Format(" and     substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDTRDJ\" as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(\"F42119\".\"SDTRDJ\" as int)),4,3) -1),1,10) > = '{0}' ", filtro.fechaDesde),
                                            //string.Format(" and     substr((to_date('01-01-' || to_char(round(1900 + (CAST(\"F42119\".\"SDTRDJ\" as int)/1000))),'DD-MM-YYYY') + substr (to_char(CAST(\"F42119\".\"SDTRDJ\" as int)),4,3) -1),1,10) < = '{0}' ", filtro.fechaHasta),
                                            filtroFechas ,
                                            filtroAerolinea,
                                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") > =(TRIM('{0}')) ", filtro.facturaDesde.Trim()),
                                            string.Format(" and     TRIM(\"F42119\".\"SDDOC\") < =(TRIM('{0}')) ", filtro.facturaHasta.Trim()),
                                            " and     \"F42119\".\"SDDCT\" IN ('FP','FQ') ",
                                            " and 	\"F42119\".\"SDDCTO\" IN ('S5','S7','S9','SA','M6', 'M7','M8') ",
                                            " and	\"F42119\".\"SDZON\" IN ('INT', 'DOM') ",
                                            " and rownum <= 100");  
                    break;
                default:
                    break;
            }
            if (filtro.Descarga)
            {
                consulta = consulta.Replace("and rownum <= 100", "  Order by 4");
            }
            else { consulta = consulta + " Order by 4"; }

            consulta = consulta.Replace("%pdta%", squemaData);
            return consulta;
        }
        public static double ConvertToJulian(DateTime Date)
        {
            double Year = Date.Year;
            double dato = (Year - 1900) * 1000 + Date.DayOfYear;
            return dato;
        }

    }
}
