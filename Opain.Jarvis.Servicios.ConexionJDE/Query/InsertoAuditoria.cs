using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Opain.Jarvis.Infraestructura.Datos.EntidadesIntegracion;
namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    public static class InsertoAuditoria
    {
        public static string Consulta(DatosAuditoria datosAuditoria, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consulta = "INSERT INTO \"%pdta%\".\"FQ70594C\" (APQ70IDP,APLNID,APEV01,APDSC1, APDSC2,APDSC3,APUSER,APUPMJ,APUPMT)" +
                "VALUES ('" + datosAuditoria.Id_Pasajero + "','" + datosAuditoria.Linea + "','" + datosAuditoria.Genera_Cobro + "','" + datosAuditoria.Campo_Modificado + "','" + datosAuditoria.Valor_Anterior + "','" + datosAuditoria.Valor_Nuevo + "',(case when length(USER)>9 then substr(user,1,9) else user end),'" + ConvertToJulian(DateTime.Now) + "','" + ConvertToJulian(DateTime.Now) + "')";

            
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
