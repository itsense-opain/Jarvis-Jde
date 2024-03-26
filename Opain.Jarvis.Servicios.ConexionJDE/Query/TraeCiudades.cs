using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public static class TraeCiudades
    {
        public static string Consulta(IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consulta = " SELECT MAQ70IATA AS COD_IATA,MAQ70OACI AS COD_OACI,MACTY1 AS CIUDAD,MACTR AS PAIS , MAQ70ALPH AS AEROPUERTO FROM \"%pdta%\".\"FQ70590A\" ";

            consulta = consulta.Replace("%pdta%", squemaData);

            return consulta;
        }
    }
}
