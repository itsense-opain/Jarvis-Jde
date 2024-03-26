using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class RespuestaTicketRepositorio : IRespuestaTicketRepositorio
    {
        private readonly ContextoOpain _contexto;

        public RespuestaTicketRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task ActualizarAsync(RespuestaTicket respuestaTicket)
        {
            _contexto.Update(respuestaTicket);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var ticket = await ObtenerAsync(id);
            _contexto.RespuestasTickets.Remove(ticket);
            _contexto.SaveChanges();
        }

        public async Task InsertarAsync(RespuestaTicket respuestaTicket)
        {
            await _contexto.AddAsync(respuestaTicket);
            await _contexto.SaveChangesAsync();
        }

        public async Task<RespuestaTicket> ObtenerAsync(int id)
        {
            return await _contexto.RespuestasTickets.Include(x => x.Usuario).Include(x => x.Ticket).FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<IList<RespuestaTicket>> ObtenerPorTicketAsync(int id)
        {
            return await _contexto.RespuestasTickets.Include(x => x.Usuario).Include(x => x.Ticket).Where(x => x.IdTicket.Equals(id)).ToListAsync();
        }
    }
}
