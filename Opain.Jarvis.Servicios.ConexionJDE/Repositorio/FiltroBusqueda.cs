using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Repositorio
{
    public class FiltroBusqueda
    {
        public string aerolinea { get; set; }
        public string fechaDesde { get; set; }
        public string fechaHasta { get; set; }
        public string facturaDesde { get; set; }
        public string facturaHasta { get; set; }
        public string tipo { get; set; }
        public string tipoVuelo { get; set; }
        public string estado { get; set; }
        public bool Descarga { get; set; }
    }
}
