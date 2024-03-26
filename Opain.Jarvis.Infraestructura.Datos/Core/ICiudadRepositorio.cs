using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface ICiudadRepositorio
    {
        Task InsertarAsync(Ciudad ciudad);
        Task ActualizarAsync(Ciudad ciudad);
        Task EliminarAsync(string id);
        Task<Ciudad> ObtenerAsync(string id);
        Task<IList<Ciudad>> ObtenerTodosAsync();
    }
}