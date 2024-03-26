using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IPasajeroRepositorio
    {
        Task InsertarAsync(Pasajero pasajero);
        Task InsertarMasivoAsync(IList<Pasajero> pasajero);
        Task ActualizarAsync(Pasajero pasajero);
        Task EliminarAsync(int id);
        Task EliminarOperacionAsync(int idOperacion);
        Task<Pasajero> ObtenerAsync(int id);
        Task<IList<Pasajero>> ObtenerTodosAsync(int idOperacion);
    }
}