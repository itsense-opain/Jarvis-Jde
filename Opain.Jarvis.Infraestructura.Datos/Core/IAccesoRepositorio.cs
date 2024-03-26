using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IAccesoRepositorio
    {
        Task InsertarAsync(Acceso acceso);

        Task<IList<Acceso>> ObtenerTodosAsync(DateTime inicio, DateTime fin,string aerolinea);
    }
}
