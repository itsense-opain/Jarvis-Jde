using Microsoft.AspNetCore.Identity;
using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface IUsuarioAplicacion
    {
        Task InsertarAsync(UsuarioOtd usuario);
        Task ActualizarAsync(UsuarioOtd usuario);
        Task EliminarAsync(string id);
        Task<UsuarioOtd> ObtenerAsync(string id);
        Task<UsuarioOtd> ObtenerPorEmailAsync(string email);
        Task<UsuarioOtd> ObtenerPorAliasAsync(string alias);
        Task<bool> ObtenerPorusuarioyclave(string alias, string clave);
        Task<bool> ActualizarclaveUsuario(string usuarioNombre, string clave);
        Task<IList<UsuarioOtd>> ObtenerAsync();
    }
}
