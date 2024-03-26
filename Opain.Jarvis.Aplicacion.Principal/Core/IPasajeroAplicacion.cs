using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface IPasajeroAplicacion
    {
        Task InsertarAsync(PasajeroOtd pasajeroOtd);
        Task ProcesarArchivoAsync(StreamReader archivo, string nombreArchivo);
        Task ActualizarAsync(PasajeroOtd pasajeroOtd);
        Task EliminarAsync(int id);
        Task<PasajeroOtd> ObtenerAsync(int id);
        Task<IList<PasajeroOtd>> ObtenerTodosAsync(int idVuelo);
        Task InsertarMasivoAsync(IList<PasajeroOtd> pasajeroOtd);
        Task<IList<ExentoODT>> ObtenerExentos(string numerovuelo, string fecha);
    }
}
