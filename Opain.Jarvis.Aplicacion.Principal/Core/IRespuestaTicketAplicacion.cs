using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface IRespuestaTicketAplicacion
    {
        Task InsertarAsync(RespuestaTicketOtd respuestaTicket);
        Task ActualizarAsync(RespuestaTicketOtd respuestaTicket);
        Task EliminarAsync(int id);
        Task<RespuestaTicketOtd> ObtenerAsync(int id);
        Task<IList<RespuestaTicketOtd>> ObtenerPorTicketAsync(int id);
    }
}
