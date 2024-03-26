using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;
using Opain.Jarvis.Dominio.Entidades;


namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IHorarioAerolineaRepositorio
    {
        Task InsertarAsync(HorarioAerolinea horarioAerolinea);
        Task ActualizarAsync(HorarioAerolinea horarioAerolinea);
        Task EliminarAsync(int id);
        Task<HorarioAerolinea> ObtenerAsync(int id);
        Task<IList<HorarioAerolinea>> ObtenerTodosAsync();

    


    }
}