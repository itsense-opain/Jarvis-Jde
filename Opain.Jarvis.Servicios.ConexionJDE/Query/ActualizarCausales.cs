using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Opain.Jarvis.Infraestructura.Datos.EntidadesIntegracion;
namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public static class ActualizarCausales
    {
        public static string Consulta(Causales datosCausales, IConfiguration config)
        {
            string squemaData = string.Empty;
            string consulta = string.Empty;

            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            consulta = "INSERT INTO \"%pdta%\".\"FQ70594G\" (CPQ70IDP, CPLNID, CPQ70CAU, CPQ70TTP, CPA, CPQ70TV, CPUSER, CPUPMJ, CPUPMT)" +
                                "VALUES('" + datosCausales.Id_Pasajero + "','" + datosCausales.Linea + "','" + datosCausales.Causal + "','" + datosCausales.TotalPax + "','" + datosCausales.GeneraCobro + "','" + datosCausales.TipoVuelo + "',(case when length(USER)>9 then substr(user,1,9) else user end),'" + ConvertToJulian(DateTime.Now).ToString() + "','" + ConvertToJulian(DateTime.Now).ToString() + "')";
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
