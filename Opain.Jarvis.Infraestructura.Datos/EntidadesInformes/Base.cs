using System;
using System.Collections.Generic;
using System.Text;

namespace Opain.Jarvis.Infraestructura.Datos.EntidadesInformes
{
    public abstract class Base
    {
        public string PrefijoFactura { get; set; }
        public string TDocFact { get; set; }
        public string TOrdFact { get; set; }
        public string TipoMoneda { get; set; }
        public string Sigla { get; set; }
    }
}
