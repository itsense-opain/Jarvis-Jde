using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Motor.Negocio
{
    public static class Oracle
    {
        public static async Task RealizarTareaAsync()
        {
            await Ejecutar();
        }
        public static async Task Ejecutar()

        {
            ServicioApi servicio = new ServicioApi();
            //Logica
            IList<Aerolinea> respuesta = await servicio.GetAsync<IList<Aerolinea>>("api/Integracion/TraerAerolineas");
            IList<DatosCiudades> respuesta2 = await servicio.GetAsync<IList<DatosCiudades>>("api/Integracion/TraerdatosCiudades");
            IList<DatosVuelo> respuesta3 = await servicio.GetAsync<IList<DatosVuelo>>("api/Integracion/TraerVuelos?fechaBusqueda='" + DateTime.Now.AddDays(-1) + "'");
            //tener los datos realizar un insert a mysql
        }
        public static async Task Ejecutar2()
        {
            ServicioApi servicio = new ServicioApi();
            //Logica
            int respuesta = await servicio.GetAsync<int>("api/Integracion/ActualizarVuelosValidados");
            
        }
    }
}