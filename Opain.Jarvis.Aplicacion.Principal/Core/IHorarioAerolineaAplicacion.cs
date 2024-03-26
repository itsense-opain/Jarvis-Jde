using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface IHorarioAerolineaAplicacion
    {
        Task InsertarAsync(HorarioAerolineaOtd horarioAerolineaOtd);
        Task ActualizarAsync(HorarioAerolineaOtd horarioAerolineaOtd);
        Task EliminarAsync(int id);
        Task<HorarioAerolineaOtd> ObtenerAsync(int id);
        Task<IList<HorarioAerolineaOtd>> ObtenerTodosAsync();

       // Task InsertarTripulanteMasivoAsync(IList<Tripulantes> tripulante);

    }
}
