using Microsoft.Extensions.Configuration;
using Opain.Jarvis.Servicios.ConexionJDE.Repositorio;
using System;
using System.Collections.Generic;

namespace Opain.Jarvis.Servicios.ConexionJDE.Query
{
    /// <summary>
    /// ActualizaExento
    /// </summary>
    public static class ActualizaExento
    {
        public static string QueryUPDExento(DatosExento exento, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consultaUpdEx = "";

            //cuando el pasajero viaja
            if (exento.viajo == 1)
            {
                consultaUpdEx = "UPDATE %pdta%.FQ70594E SET PEQ70IDP = '" + exento.PEQ70IDPPAX2 + "', " +
                              "PEQ70EST = '" + exento.PEQ70EST + "'  WHERE PEQ70IDX = '" + exento.PEQ70IDX + "' AND PEQ70TER = '" + exento.PEQ70TER + "'";

            }
            //cuando el pasajero no viaja
            else
            {
                
                consultaUpdEx = "UPDATE %pdta%.FQ70594E SET PEQ70IDP = '0', " +
                           "PEQ70EST = 'SIN' WHERE PEQ70IDX = '" + exento.PEQ70IDX + "' AND PEQ70TER = '" + exento.PEQ70TER + "'";

            }

            consultaUpdEx = consultaUpdEx.Replace("%pdta%", squemaData);

            return consultaUpdEx;
        }

        public static string QueryINSAuditEX(DatosExento exento, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consultaInsAuEx = "Insert into %pdta%.FQ70594F " +
                "(AEQ70IDX,AELNID,AEQ70EST,AEURCD,AEURDT,AEURAT,AEURAB,AEURRF," +
                "AEUSER,AEPID,AEJOBN,AEUPMJ,AEUPMT) " +
                "values ('"+ exento.AEQ70IDX + "','"+exento.AELNID + "','"+exento.AEQ70EST+ "'," +
                "'"+exento.AEURCD + "','"+exento.AEURDT + "','"+exento.AEURAT + "','"+exento.AEURAB + "'," +
                "'"+exento.AEURRF + "'," +"'"+exento.AEUSER + "','"+exento.AEPID + "'," +
                "'"+exento.AEJOBN + "','"+exento.AEUPMJ+ "','"+exento.AEUPMT+ "')";

            consultaInsAuEx = consultaInsAuEx.Replace("%pdta%", squemaData);

            return consultaInsAuEx;
        }
        public static string QueryUPDAuditRegVuelo(DatosExento exento, IConfiguration config) {

            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consultaUpdAuRegVue = "INSERT INTO %pdta%.FQ70594C " +
                "(APQ70IDP,APLNID,APQ70CAU,APDL011,APEV01,APDSC1," +
                "APDSC2,APDSC3,APQ70EST,APURCD,APURAT,APURAB,APURRF," +
                "APTORG,APUSER,APPID,APJOBN,APUPMJ,APUPMT)" +
                " values ('"+exento.APQ70IDP+"','"+exento.APLNID+"','"+exento.APQ70CAU+"'," +
                "'"+exento.APDL011+ "','"+exento.APEV01+ "','"+exento.APDSC1+ "'," +
                "'"+exento.APDSC2+ "','"+exento.APDSC3+ "','"+exento.APQ70EST+ "','"+exento.APURCD+ "'," +
                "'"+exento.APURAT+ "','"+exento.APURAB+ "','"+exento.APURRF+ "'," +
                "'"+exento.APTORG+ "','"+exento.APUSER+ "','"+exento.APPID+ "','"+exento.APJOBN+ "'," +
                "'"+exento.APUPMJ + "','"+exento.APUPMT+ "')";

            consultaUpdAuRegVue = consultaUpdAuRegVue.Replace("%pdta%", squemaData);

            return consultaUpdAuRegVue;
        }
        /// <summary>
        /// ConvertToJulian
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static double ConvertToJulian(DateTime Date)
        {
            double Year = Date.Year;
            double dato = (Year - 1900) * 1000 + Date.DayOfYear;
            return dato;
        }

        public static string ConsultaExJDE(DatosExento exento, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consultaExJDE = "SELECT PEQ70IDP, PEQ70IDX, PEQ70IATA,PEAN8,PEQ70TER  " +
                "FROM \"%pdta%\".\"FQ70594E\" " +
                "WHERE PEQ70CVO = '" + exento.PEQ70CVO+ "' " +
                "AND PEQ70FES = '" + ConvertToJulian(exento.PEQ70FRL).ToString()+ "'";

            consultaExJDE = consultaExJDE.Replace("%pdta%", squemaData);

            return consultaExJDE;
        }
        /// <summary>
        /// ConsultaUtlRow
        /// </summary>
        /// <param name="tblName"></param>
        /// <returns></returns>
        public static string ConsultaUtlRow(string tblName, IConfiguration config)
        {
            string squemaData = string.Empty;
            squemaData = Opain.Jarvis.Servicios.ConexionJDE.Helpers.Esquemas.ObtenerEsquema("Data", config);

            string consultaUltRow = "";

            if (tblName == "FQ70594F") 
            {
                consultaUltRow = "SELECT (AELNID + 1) as AELNID FROM \"%pdta%\".\"FQ70594F\" ORDER BY AELNID DESC FETCH FIRST 1 ROWS ONLY";
            }

            if (tblName == "FQ70594C")
            {
                consultaUltRow = "SELECT (APLNID + 1) as APLNID FROM \"%pdta%\".\"FQ70594C\" ORDER BY APLNID DESC FETCH FIRST 1 ROWS ONLY";
            }
            
            consultaUltRow = consultaUltRow.Replace("%pdta%", squemaData);

            return consultaUltRow;
        }
    }
}