using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class TicketRepositorio : ITicketRepositorio
    {
        private readonly ContextoOpain _contexto;

        public TicketRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task ActualizarAsync(Ticket ticket)
        {
            _contexto.Update(ticket);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var ticket = await ObtenerAsync(id);
            _contexto.Tickets.Remove(ticket);
            _contexto.SaveChanges();
        }

        public async Task InsertarAsync(Ticket ticket)
        {
            await _contexto.AddAsync(ticket);
            await _contexto.SaveChangesAsync();
        }

        public async Task<Ticket> ObtenerAsync(int id)
        {
            return await _contexto.Tickets
                .Include(x => x.Aerolinea)
                .Include(x => x.Usuario)
                .Include(x => x.RespuestasTickets)
                .FirstOrDefaultAsync(X => X.Id.Equals(id));
        }

        public async Task<IList<Ticket>> ObtenerTodosAsync()
        {
            return await _contexto.Tickets
                .Include(x => x.Aerolinea)
                .Include(x => x.Usuario)
                .Include(x => x.RespuestasTickets)
                .ToListAsync();
        }
    }
}
