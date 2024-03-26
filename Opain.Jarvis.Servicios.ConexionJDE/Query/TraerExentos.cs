using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public static class TraerExentos
    {
        public static string Consulta(string numvuelo, DateTime fecha, IConfiguration config)
        {

            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            // convierto la fecha a juliana
            string Fjuliana = ConvertToJulian(fecha).ToString();
            string numeroVuelo = numvuelo.Trim();
            string consulta =
            // $"select E.PEQ70IDX as IDEXENTO, E.PEQ70IDP as idpasajero, substr((to_date('01-01-'||to_char(round(1900+(CAST(E.PEQ70FES as int)/1000))),'DD-MM-YYYY') + substr(to_char(CAST(E.PEQ70FES as int)),4,3) -1),1,10) as Fecha,E.PEQ70CVO as Vuelo, rtrim(E.PEQ70FN) || ' ' || rtrim(E.PEQ70LN) || ' ' || rtrim(E.PEQ70SLN) as Pasajero,'EX' as Categoria, H.DOQ70CM AS MATRICULA,PEQ70TER AS TERMINAL FROM \"%pdta%\".\"FQ70594E\" E  INNER JOIN \"%pdta%\".\"FQ70591H\" H ON H.DOQ70IDD = E.PEQ70IDP where PEQ70EST = 'SIN' and PEQ70CVO ='{numeroVuelo}' AND PEQ70FES ='{Fjuliana}'";
            $"SELECT E.PEQ70IDX as IDEXENTO,E.PEQ70IDP as IDPASAJERO, to_char(to_date(concat(to_char(to_number(substr(E.PEQ70FES, 1, 3) + 1900)), substr(E.PEQ70FES, 4, 3)), 'YYYYDDD'), 'dd/mm/yyyy') AS FECHA,E.PEQ70CVO as VUELO, rtrim(E.PEQ70LN) || ' ' || rtrim(E.PEQ70SLN) || ' ' || rtrim(E.PEQ70FN) as PASAJERO,'EX' as CATEGORIA,H.PEQ70CM AS MATRICULA,PEQ70TER AS TERMINAL,H.PEQ70IDP as IDPAX2 FROM \"%pdta%\".\"FQ70594E\" E INNER JOIN \"%pdta%\".\"FQ70594A\" H ON H.PEQ70FRL = E.PEQ70FES AND H.PEQ70CVO = E.PEQ70CVO where E.PEQ70EST IN ('SIN','REP') and E.PEQ70CVO ='{numeroVuelo}' AND E.PEQ70FES ='{Fjuliana}'";
            //   where rownum < 3

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
