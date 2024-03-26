using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public static class TraerAerolineas
    {
        public static string Consulta(IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);
            string consulta = " SELECT MAQ70OACI AS Valor, MAALPH as Texto FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN80 >= 1 order by rtrim(ltrim( MAALPH)) ";
            
            consulta = consulta.Replace("%pdta%", squemaData);

            return consulta;
        }

        public static string Consulta2(IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            //SELECT MAAN8 AS Valor, MAALPH as Texto, MAQ70OACI as Sigla FROM "%pdta%"."FQ70591C" "FQ70591C" 
            string consulta = " SELECT MAAN8 AS Valor,rtrim(ltrim( MAALPH)) as Texto, MAQ70OACI as Sigla FROM \"%pdta%\".\"FQ70591C\" \"FQ70591C\" WHERE MAAN80 >= 1 order by rtrim(ltrim( MAALPH)) ";
            
            consulta = consulta.Replace("%pdta%", squemaData);

            return consulta;
        }
    }
}
