using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface IHorarioOperacionAplicacion
    {
        Task InsertarAsync(HorarioOperacionOtd horarioOperacionOtd);
        Task ActualizarAsync(HorarioOperacionOtd horarioOperacionOtd);
        Task EliminarAsync(int id);
        Task<HorarioOperacionOtd> ObtenerAsync(int id);
        Task<IList<HorarioOperacionOtd>> ObtenerTodosAsync();
        Task ActualizarTodosAsync(List<HorarioOperacionOtd> horariosOperacionesOtd);
    }
}
