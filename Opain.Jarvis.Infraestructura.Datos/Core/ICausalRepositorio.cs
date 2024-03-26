using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface ICausalRepositorio
    {
        Task InsertarAsync(Causal causal);
        Task ActualizarAsync(Causal causal);
        Task EliminarAsync(int id);
        Task<Causal> ObtenerAsync(int id);
        Task<Causal> ObtenerPorCodigoAsync(string id);
        Task<IList<Causal>> ObtenerTodosAsync();
        Task<IList<Causal>> ObtenerPorTipoAsync(int tipo);
    }
}
