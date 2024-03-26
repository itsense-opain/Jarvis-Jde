using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Opain.Jarvis.Motor.Integracion
{
    public partial class Pruebas : Form
    {
        public Pruebas()
        {
            InitializeComponent();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ServicioApi servicio = new ServicioApi();
           // IList<DatosVuelo> respuesta = await servicio.GetAsync<IList<DatosVuelo>>("api/Integracion/TraerVuelos");
        }
    }
}
