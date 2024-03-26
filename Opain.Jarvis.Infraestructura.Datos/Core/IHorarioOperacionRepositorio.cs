using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IHorarioOperacionRepositorio
    {
        Task InsertarAsync(HorarioOperacion horarioOperacion);
        Task ActualizarAsync(HorarioOperacion horarioOperacion);
        Task EliminarAsync(int id);
        Task<HorarioOperacion> ObtenerAsync(int id);
        Task<IList<HorarioOperacion>> ObtenerTodosAsync();
        Task ActualizarTodosAsync(List<HorarioOperacion> horarioOperaciones);
    }
}
