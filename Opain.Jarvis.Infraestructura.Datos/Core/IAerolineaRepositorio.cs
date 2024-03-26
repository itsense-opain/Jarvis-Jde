using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IAerolineaRepositorio
    {
        Task InsertarAsync(Aerolinea aerolinea);
        Task ActualizarAsync(Aerolinea aerolinea);
        Task EliminarAsync(int id);
        Task<Aerolinea> ObtenerAsync(int id);
        Task<IList<Aerolinea>> ObtenerTodosAsync();
        Task ActualizarTodosAsync(List<Aerolinea> aerolineas);
        Task<IList<Aerolinea>>  ObtenerHorarioIdAerolineaAsync(int IdAerolinea);
        
    }
}