using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface IAerolineaAplicacion
    {
        Task InsertarAsync(AerolineaOtd aerolineaOtd);
        Task ActualizarAsync(AerolineaOtd aerolineaOtd);
        Task EliminarAsync(int id);
        Task<AerolineaOtd> ObtenerAsync(int id);
        Task<IList<AerolineaOtd>> ObtenerTodosAsync();
        Task ActualizarTodosAsync(List<AerolineaOtd> aerolineasOtd);
        Task<IList<AerolineaOtd>> ObtenerHorarioIdAerolineaAsync(int IdAerolinea);
        
    }
}
