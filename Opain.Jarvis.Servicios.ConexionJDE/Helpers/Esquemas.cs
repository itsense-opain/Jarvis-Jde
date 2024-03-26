using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;



namespace Opain.Jarvis.Servicios.ConexionJDE.Helpers
{
    public class Esquemas
    {
        public static string ObtenerEsquema(string Tag, IConfiguration Config)
        {
            string _respuestaEsquema = string.Empty;
            try
            {
                if (Config.GetSection("ProfileDatabase").GetSection("Development").Value.Equals("true"))       // Dev
                {
                    if (Tag == "Data")
                    {
                        _respuestaEsquema = Config.GetSection("ProfileDatabase:SchemeDev:Data").Value.ToString().Trim();
                    }
                    else if (Tag == "Control")
                    {
                        _respuestaEsquema = Config.GetSection("ProfileDatabase:SchemeDev:Control").Value.ToString().Trim();
                    }
                }
                else                                                                                            //Prod
                {
                    if (Tag == "Data")
                    {
                        _respuestaEsquema = Config.GetSection("ProfileDatabase:SchemeProd:Data").Value.ToString().Trim();
                    }
                    else if (Tag == "Control")
                    {
                        _respuestaEsquema = Config.GetSection("ProfileDatabase:SchemeProd:Control").Value.ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _respuestaEsquema;
        }
    }
}
