using AutoMapper;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Aplicacion;

using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Opain.Jarvis.Infraestructura.Datos.Core;

namespace Opain.Jarvis.Aplicacion
{
    public class UsuarioAplicacion : IUsuarioAplicacion
    {
        private readonly IUsuariosRepositorio usuarioRepositorio;
        private readonly IMapper mapper;

        public UsuarioAplicacion(IUsuariosRepositorio ur, IMapper m)
        {
            usuarioRepositorio = ur;
            mapper = m;
        }

        public async Task ActualizarAsync(UsuarioOtd usuarioOtd)
        {
            var usuario = await usuarioRepositorio.ObtenerAsync(usuarioOtd.Id);

            usuario.Nombre = usuarioOtd.Nombre;
            usuario.Apellido = usuarioOtd.Apellido;
            usuario.TipoDocumento = usuarioOtd.TipoDocumento;
            usuario.NumeroDocumento = usuarioOtd.NumeroDocumento;
            usuario.Email = usuarioOtd.Email;
            usuario.Telefono = usuarioOtd.Telefono;
            usuario.Cargo = usuarioOtd.Cargo;
            usuario.UserName = usuarioOtd.NumeroDocumento;
            usuario.Activo = usuarioOtd.Activo;

            await usuarioRepositorio.ActualizarAsync(usuario, usuarioOtd.Perfil, usuarioOtd.Aerolinea);
        }

        public async Task EliminarAsync(string id)
        {
            await usuarioRepositorio.EliminarAsync(id);
        }

        public async Task InsertarAsync(UsuarioOtd usuarioOtd)
        {
            Usuario usuario = new Usuario
            {
                Nombre = usuarioOtd.Nombre,
                Apellido = usuarioOtd.Apellido,
                TipoDocumento = usuarioOtd.TipoDocumento,
                NumeroDocumento = usuarioOtd.NumeroDocumento,
                Email = usuarioOtd.Email,
                Telefono = usuarioOtd.Telefono,
                Cargo = usuarioOtd.Cargo,
                UserName = usuarioOtd.NumeroDocumento,
                Activo = true
            };

            await usuarioRepositorio.InsertarAsync(usuario, usuarioOtd.Clave, usuarioOtd.Perfil, usuarioOtd.Aerolinea);
        }

        public async Task<UsuarioOtd> ObtenerAsync(string id)
        {
            var usuario =  await usuarioRepositorio.ObtenerAsync(id);
            var usuarioOtd = mapper.Map<Usuario, UsuarioOtd>(usuario);

            return usuarioOtd;
        }

        public async Task<IList<UsuarioOtd>> ObtenerAsync()
        {
            IList<Usuario> usuarios = await usuarioRepositorio.ObtenerTodosAsync();
            IList<UsuarioOtd> retorno = new List<UsuarioOtd>();

            for (int i = 0; i < usuarios.Count; i++)
            {
                UsuarioOtd u = mapper.Map<Usuario, UsuarioOtd>(usuarios[i]);

                retorno.Add(u);
            }

            return retorno;
        }

        public async Task<UsuarioOtd> ObtenerPorEmailAsync(string email)
        {
            Usuario usuario = await usuarioRepositorio.ObtenerPorEmailAsync(email);
            UsuarioOtd usuarioR = mapper.Map<Usuario, UsuarioOtd>(usuario);

            return usuarioR;
        }

        public async Task<UsuarioOtd> ObtenerPorAliasAsync(string alias)
        {
            Usuario usuario = await usuarioRepositorio.ObtenerPorAliasAsync(alias);
            UsuarioOtd usuarioR = mapper.Map<Usuario, UsuarioOtd>(usuario);

            return usuarioR;
        }

        public async Task<bool> ObtenerPorusuarioyclave(string alias, string clave)
        {
            bool respuesta = await usuarioRepositorio.ObtenerPorusuarioyclave(alias,clave );
           return respuesta;
        }
        public async Task<bool> ActualizarclaveUsuario(string usuarioNombre, string clave)
        {
            bool respuesta = await usuarioRepositorio.ActualizarclaveUsuario(usuarioNombre, clave);
            return respuesta;
        }

    }
}

