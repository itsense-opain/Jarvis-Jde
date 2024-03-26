using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IPasajeroTransitoRepositorio
    {
        Task InsertarAsync(PasajeroTransito pasajeroTransito);
        Task ActualizarAsync(PasajeroTransito pasajeroTransito);
        Task EliminarAsync(int id);
        Task EliminarOperacionAsync(int idOperacion);
        Task<PasajeroTransito> ObtenerAsync(int id);
        Task<IList<PasajeroTransito>> ObtenerTodosAsync(int idOperacion);
        Task InsertarMasivoAsync(IList<PasajeroTransito> pasajeros);
        Task<IList<PasajeroTransito>> ObtenerAerolineaAsync(string Aerolinea);
    }
}