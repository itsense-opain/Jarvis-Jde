using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IPaisRepositorio
    {
        Task InsertarAsync(Pais pais);
        Task ActualizarAsync(Pais pais);
        Task EliminarAsync(string id);
        Task<Pais> ObtenerAsync(string id);
        Task<IList<Pais>> ObtenerTodosAsync();
    }
}