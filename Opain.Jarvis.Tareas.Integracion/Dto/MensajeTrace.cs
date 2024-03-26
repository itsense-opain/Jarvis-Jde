using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Tareas.Integracion.Dto
{
    public class MensajeTrace
    {
            public int IDEvento { get; set; }
            public string Aplicacion { get; set; }
            public string Mensaje { get; set; }
            public int Severidad { get; set; }
            public string Usuario { get; set; }

    }
}
