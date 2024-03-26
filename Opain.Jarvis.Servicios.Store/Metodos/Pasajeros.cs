
using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Opain.Jarvis.Servicios.Store;

namespace Opain.Jarvis.Servicios.Store.Metodos
{
    public class Pasajeros
    {
        private readonly Helper.Ejecutor Ejecutor;
        private readonly IConfiguration _config;

        public Pasajeros(Helper.Ejecutor ejecutor, IConfiguration config)
        {
            this.Ejecutor = ejecutor;
            this._config = config;
        }

        public async Task<IList<PasajeroTransitoOtd>> Find(TransitoRequest Otd)
        {
            List<PasajeroTransitoOtd> oList;
            DataTable oDT = new DataTable();
            this.Ejecutor.AgregarCampoIn("Opcion", Otd.OpcionOperacion);
            this.Ejecutor.AgregarCampoIn("Id", Otd.Id.ToString());
            this.Ejecutor.AgregarCampoIn("FechaLlegada", Otd.FechaHoraCargue);
            this.Ejecutor.AgregarCampoIn("FechaSalida", Otd.FechaHoraFirma);
            this.Ejecutor.AgregarCampoIn("MatriculaVuelo", "");
            this.Ejecutor.AgregarCampoIn("TipoVuelo", Otd.Tipo);
            this.Ejecutor.AgregarCampoIn("NumeroVuelo", Otd.NumeroVueloSalida);
            this.Ejecutor.AgregarCampoIn("IdAerolinea", Otd.AerolineaSalida);
            this.Ejecutor.AgregarCampoIn("EstadoProceso", Otd.EstadoProceso);
            this.Ejecutor.AgregarCampoIn("IdCargue", Otd.IdCargue);
            this.Ejecutor.AgregarCampoIn("Categoria", Otd.Categoria);
            this.Ejecutor.AgregarCampoIn("IdVuelo", "");
            this.Ejecutor.AgregarCampoIn("NumeroVueloLlegada", Otd.NumeroVueloLlegada);
            try
            {
                oDT = this.Ejecutor.GetData("FiltroPasajeros");
                oList = ConvertDataTableToList(oDT);
            }
            catch (Exception oE)
            {
                throw;
            }
            return oList;
        }

        private List<PasajeroTransitoOtd> ConvertDataTableToList(DataTable oDt)
        {
            PasajeroTransitoOtd Otd;
            List<PasajeroTransitoOtd> oList = new List<PasajeroTransitoOtd>();

            foreach (DataRow oR in oDt.Rows)
            {
                try
                {
                    Otd = new PasajeroTransitoOtd();
                    Otd.AerolineaLlegada = oR["AerolineaLlegada"].ToString() == "" ? "" : oR["AerolineaLlegada"].ToString();
                    Otd.AerolineaSalida = oR["AerolineaSalida"].ToString() == "" ? "" : oR["AerolineaSalida"].ToString();
                    Otd.Destino = oR["Destino"].ToString() == "" ? "XXX" : oR["Destino"].ToString();
                    Otd.FechaHoraCargue = Comun.Funciones.TryParse(oR["FechaHoraCargue"].ToString());
                    Otd.FechaHoraFirma = Comun.Funciones.TryParse(oR["FechaHoraFirma"].ToString());
                    Otd.FechaLlegada = Comun.Funciones.TryParse(oR["FechaLlegada"].ToString());
                    Otd.FechaSalida = Comun.Funciones.TryParse(oR["FechaSalida"].ToString());
                    Otd.Firmado = oR["Firmado"].ToString() == "" ? 0 : int.Parse(oR["Firmado"].ToString());
                    Otd.HoraLlegada = oR["HoraLlegada"].ToString() == "" ? "00:00" : oR["HoraLlegada"].ToString();
                    Otd.HoraSalida = oR["HoraSalida"].ToString() == "" ? "00:00" : oR["HoraSalida"].ToString();
                    Otd.Id = oR["Id"].ToString() == "" ? 0 : int.Parse(oR["Id"].ToString());
                    Otd.IdCargue = oR["IdCargue"].ToString() == "" ? 0 : int.Parse(oR["IdCargue"].ToString());
                    Otd.NombreAerolinea = oR["ASalida"].ToString() == "" ? "NA" : oR["ASalida"].ToString();
                    Otd.NombrePasajero = oR["NombrePasajero"].ToString() == "" ? "" : oR["NombrePasajero"].ToString();
                    Otd.NumeroVueloLlegada = oR["NumeroVueloLlegada"].ToString() == "" ? "" : oR["NumeroVueloLlegada"].ToString();
                    Otd.NumeroVueloSalida = oR["NumeroVueloSalida"].ToString() == "" ? "" : oR["NumeroVueloSalida"].ToString();
                    Otd.Observaciones = oR["Observaciones"].ToString() == "" ? "" : oR["Observaciones"].ToString();
                    Otd.Operacion = oR["Error"].ToString() == "" ? 0 : int.Parse(oR["Error"].ToString());
                    Otd.Origen = oR["Origen"].ToString() == "" ? "" : oR["Origen"].ToString();
                    Otd.Tipo = oR["TipoVuelo"].ToString() == "" ? "" : oR["TipoVuelo"].ToString();
                    Otd.TipoVuelo = oR["TipoVuelo"].ToString() == "" ? "" : oR["TipoVuelo"].ToString();
                    Otd.TTL = oR["Categoria"].ToString() == "TTL" ? 1 : 0;
                    Otd.TTC = oR["Categoria"].ToString() == "TTC" ? 1 : 0;
                    Otd.Operacion = int.Parse(oR["IdOperacionVuelo"].ToString());
                    oList.Add(Otd);
                }
                catch (Exception o)
                {

                }
            }
            return oList;
        }
    }
}
