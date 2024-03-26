using Microsoft.AspNetCore.Identity;
using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IUsuariosRepositorio
    {
        Task InsertarAsync(Usuario usuario, string clave, string rol, string aerolinea);
        Task ActualizarAsync(Usuario usuario, string rol, string aerolinea);
        Task EliminarAsync(string id);
        Task<Usuario> ObtenerAsync(string id);
        Task<Usuario> ObtenerPorEmailAsync(string email);

        Task<Usuario> ObtenerPorAliasAsync(string alias);
        Task<bool> ObtenerPorusuarioyclave(string usuarioNombre, string clave);
        Task<bool> ActualizarclaveUsuario(string usuarioNombre, string clave);
        Task<IList<Usuario>> ObtenerTodosAsync();
    }
}
