using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IRespuestaTicketRepositorio
    {
        Task InsertarAsync(RespuestaTicket respuestaTicket);
        Task ActualizarAsync(RespuestaTicket respuestaTicket);
        Task EliminarAsync(int id);
        Task<RespuestaTicket> ObtenerAsync(int id);
        Task<IList<RespuestaTicket>> ObtenerPorTicketAsync(int id);
    }
}
