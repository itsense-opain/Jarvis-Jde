using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface ITicketAplicacion
    {
        Task InsertarAsync(TicketOtd ticket);
        Task ActualizarAsync(TicketOtd ticket);
        Task EliminarAsync(int id);
        Task<TicketOtd> ObtenerAsync(int id);
        Task<IList<TicketOtd>> ObtenerTodosAsync();
    }
}
