using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface ICausalAplicacion
    {
        Task InsertarAsync(CausalOtd causalOtd);
        Task ActualizarAsync(CausalOtd causalOtd);
        Task EliminarAsync(int id);
        Task<CausalOtd> ObtenerAsync(int id);
        Task<CausalOtd> ObtenerPorCodigoAsync(string id);
        Task<IList<CausalOtd>> ObtenerTodosAsync();
        Task<IList<CausalOtd>> ObtenerPorTipoAsync(int tipo);
    }
}
