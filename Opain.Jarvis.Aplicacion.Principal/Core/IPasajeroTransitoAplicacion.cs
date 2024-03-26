using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface IPasajeroTransitoAplicacion
    {
        Task InsertarAsync(PasajeroTransitoOtd pasajeroTransitoOtd);
        Task ProcesarArchivoAsync(StreamReader archivo, string nombreArchivo);
        Task ActualizarAsync(PasajeroTransitoOtd pasajeroTransitoOtd);
        Task EliminarAsync(int id);
        Task<PasajeroTransitoOtd> ObtenerAsync(int id);
        Task<IList<PasajeroTransitoOtd>> ObtenerTodosAsync(int idOperacion);
        Task InsertarMasivoAsync(IList<PasajeroTransitoOtd> pasajerosTransitoOtd);
        Task ActualizarVuelosFirmadosAsync(int IdOperacionVuelo);
        Task<IList<PasajeroTransitoOtd>> ObtenerAerolineaAsync(string Aerolinea);
    }
}
