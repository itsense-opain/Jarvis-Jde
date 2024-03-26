using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Opain.Jarvis.Servicios.Store
{
    public class Trace
    {
        private readonly Helper.Ejecutor Ejecutor;
        private readonly IConfiguration _config;
        public Trace(Helper.Ejecutor ejecutor, IConfiguration config)
        {
            this.Ejecutor = ejecutor;
            this._config = config;
        }
        public bool Save(TraceOtd Registro)
        {
            this.Ejecutor.AgregarCampoIn("Severidad", Registro.Severidad);
            this.Ejecutor.AgregarCampoIn("Usuario", Registro.Usuario);
            this.Ejecutor.AgregarCampoIn("Mensaje", Registro.Mensaje);
            this.Ejecutor.AgregarCampoIn("IdEvento", Registro.IDEvento);
            this.Ejecutor.Conexion("FuncionTrace");
            return true;
        }
    }
}
