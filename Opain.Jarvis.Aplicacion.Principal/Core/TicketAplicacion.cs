using AutoMapper;
using Opain.Jarvis.Aplicacion;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public class TicketAplicacion : ITicketAplicacion
    {
        private readonly ITicketRepositorio ticketRepositorio;
        private readonly IRespuestaTicketAplicacion rticketAplicacion;
        private readonly IMapper mapper;

        public TicketAplicacion(ITicketRepositorio ticket, IMapper m, IRespuestaTicketAplicacion rticket)
        {
            rticketAplicacion = rticket;
            mapper = m;
            ticketRepositorio = ticket;
        }

        public async Task ActualizarAsync(TicketOtd ticketOtd)
        {
            var ticket = mapper.Map<TicketOtd, Ticket>(ticketOtd);
            await ticketRepositorio.ActualizarAsync(ticket);
        }

        public async Task EliminarAsync(int id)
        {
            await ticketRepositorio.EliminarAsync(id);
        }

        public async Task InsertarAsync(TicketOtd ticketOtd)
        {
            var ticket = mapper.Map<TicketOtd, Ticket>(ticketOtd);
            await ticketRepositorio.InsertarAsync(ticket);
        }

        public async Task<TicketOtd> ObtenerAsync(int id)
        {
            var ticket = await ticketRepositorio.ObtenerAsync(id);            
            var ticketOtd = mapper.Map<Ticket, TicketOtd>(ticket);

            var respuestas = await rticketAplicacion.ObtenerPorTicketAsync(ticket.Id);
            ticketOtd.Respuestas = respuestas;

            return ticketOtd;
        }

        public async Task<IList<TicketOtd>> ObtenerTodosAsync()
        {
            List<TicketOtd> ticketsOtd = new List<TicketOtd>();

            var tickets = await ticketRepositorio.ObtenerTodosAsync().ConfigureAwait(false);

            foreach (var item in tickets)
            {
                var t = mapper.Map<Ticket, TicketOtd>(item);
                ticketsOtd.Add(t);
            }

            return ticketsOtd;
        }
    }
}
