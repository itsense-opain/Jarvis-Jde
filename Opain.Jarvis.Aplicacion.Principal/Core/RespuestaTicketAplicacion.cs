using AutoMapper;
using Opain.Jarvis.Aplicacion;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Opain.Jarvis.Infraestructura.Datos.Core;

namespace Opain.Jarvis.Aplicacion
{
    public class RespuestaTicketAplicacion : IRespuestaTicketAplicacion
    {
        private readonly IRespuestaTicketRepositorio rticketRepositorio;
        private readonly ITicketRepositorio ticketRepositorio;
        private readonly IMapper mapper;

        public RespuestaTicketAplicacion(ITicketRepositorio ticket, IRespuestaTicketRepositorio rticket, IMapper m)
        {
            ticketRepositorio = ticket;
            mapper = m;
            rticketRepositorio = rticket;
        }
        public async Task ActualizarAsync(RespuestaTicketOtd rTicketOtd)
        {
            var rticket = mapper.Map<RespuestaTicketOtd, RespuestaTicket>(rTicketOtd);
            await rticketRepositorio.ActualizarAsync(rticket);
        }

        public async Task EliminarAsync(int id)
        {
            await rticketRepositorio.EliminarAsync(id);
        }

        public async Task InsertarAsync(RespuestaTicketOtd rTicketOtd)
        {
            var rticket = mapper.Map<RespuestaTicketOtd, RespuestaTicket>(rTicketOtd);
            await rticketRepositorio.InsertarAsync(rticket);

            var ticket = await ticketRepositorio.ObtenerAsync(rticket.IdTicket);

            if(ticket.Seguimiento == 1)
            {
                ticket.Seguimiento = 0;
                await ticketRepositorio.ActualizarAsync(ticket);
            }            
        }

        public async Task<RespuestaTicketOtd> ObtenerAsync(int id)
        {
            var ticket = await rticketRepositorio.ObtenerAsync(id);
            var rticketOtd = mapper.Map<RespuestaTicket, RespuestaTicketOtd>(ticket);

            return rticketOtd;
        }

        public async Task<IList<RespuestaTicketOtd>> ObtenerPorTicketAsync(int id)
        {
            List<RespuestaTicketOtd> rticketsOtd = new List<RespuestaTicketOtd>();

            var rtickets = await rticketRepositorio.ObtenerPorTicketAsync(id).ConfigureAwait(false);

            foreach (var item in rtickets)
            {
                var r = mapper.Map<RespuestaTicket, RespuestaTicketOtd>(item);
                rticketsOtd.Add(r);
            }

            return rticketsOtd;
        }
    }
}
