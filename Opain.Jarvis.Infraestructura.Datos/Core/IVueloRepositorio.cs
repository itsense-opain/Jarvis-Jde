using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IVueloRepositorio
    {
        Task InsertarAsync(Vuelo vuelo);
        Task ActualizarAsync(Vuelo vuelo);
        Task EliminarAsync(int id);
        Task<Vuelo> ObtenerAsync(int id);
        Task<Vuelo> ObtenerPorNombreAsync(string nombre);
        Task<IList<Vuelo>> ObtenerTodosAsync();
    }
}
