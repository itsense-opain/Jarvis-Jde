using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IUsuariosAerolineasRepositorio
    {
        Task InsertarAsync(UsuariosAerolineas usuariosAerolineas);
        Task ActualizarAsync(UsuariosAerolineas usuariosAerolineas);
        Task EliminarAsync(int id);
        Task<UsuariosAerolineas> ObtenerAsync(int id);
        Task<IList<UsuariosAerolineas>> ObtenerTodosAsync();
    }
}