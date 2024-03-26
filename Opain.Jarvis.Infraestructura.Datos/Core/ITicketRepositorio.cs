using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface ITicketRepositorio
    {
        Task InsertarAsync(Ticket ticket);
        Task ActualizarAsync(Ticket ticket);
        Task EliminarAsync(int id);
        Task<Ticket> ObtenerAsync(int id);
        Task<IList<Ticket>> ObtenerTodosAsync();
    }
}
