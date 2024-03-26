using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Repositorio
{
    public class RespuestaGenerica
    {
        public bool Exito { get; set; }
        public string MotivoNoExito { get; set; }
        private object _Resultado;

        public RespuestaGenerica(object obj)
        {
            this.Exito = true;
            this.MotivoNoExito = string.Empty;
            this._Resultado = obj;
        }

        public RespuestaGenerica(string motivoNoExito)
        {
            this.Exito = false;
            this.MotivoNoExito = motivoNoExito;
            this._Resultado = null;
        }

        public List<T> Resultado<T>()
        {
            if (this._Resultado != null)
                return (List<T>)_Resultado;

            return null;
        }
    }
}
