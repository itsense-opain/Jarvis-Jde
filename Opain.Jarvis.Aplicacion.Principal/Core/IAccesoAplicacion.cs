using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface IAccesoAplicacion
    {
        Task InsertarAsync(AccesoOtd acceso);

        Task<IList<AccesoOtd>> ObtenerTodosAsync(DateTime inicio, DateTime fin,string aerolinea);
    }
}
