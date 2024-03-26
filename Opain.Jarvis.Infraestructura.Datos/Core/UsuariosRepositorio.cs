using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class UsuariosRepositorio : IUsuariosRepositorio
    {
        private readonly ContextoOpain contexto;
        private readonly UserManager<Usuario> administradorUsuario;

        public UsuariosRepositorio(ContextoOpain co, UserManager<Usuario> usuario)
        {
            contexto = co;
            administradorUsuario = usuario;
        }

        public async Task ActualizarAsync(Usuario usuario, string rol, string aerolinea = "")
        {
            await administradorUsuario.UpdateAsync(usuario);

            await administradorUsuario.RemoveFromRoleAsync(usuario, "EXTERNO");
            await administradorUsuario.RemoveFromRoleAsync(usuario, "ADMINISTRADOR");
            await administradorUsuario.RemoveFromRoleAsync(usuario, "OPAIN");
            await administradorUsuario.RemoveFromRoleAsync(usuario, "TECNOLOGIA");
            await administradorUsuario.RemoveFromRoleAsync(usuario, "SUPERVISOR CARGA");
            await administradorUsuario.RemoveFromRoleAsync(usuario, "AEROLINEA");
            await administradorUsuario.RemoveFromRoleAsync(usuario, "SUPERVISOR");
            await administradorUsuario.RemoveFromRoleAsync(usuario, "CARGA");

            await administradorUsuario.AddToRoleAsync(usuario, rol);

            var usuariosAerolineas = await contexto
                .UsuariosAerolineas
                .Where(x => x.IdUsuario.Equals(usuario.Id))
                .ToListAsync();

            if (usuariosAerolineas != null)
            {
                contexto.UsuariosAerolineas.RemoveRange(usuariosAerolineas);
                await contexto.SaveChangesAsync();
            }

            if (!string.IsNullOrEmpty(aerolinea))
            {
                Aerolinea aero = new Aerolinea();
                aero = await ObtenerAerolineaAsync(aerolinea);

                if (aero != null)
                {
                    UsuariosAerolineas ua = new UsuariosAerolineas
                    {
                        IdUsuario = usuario.Id,
                        IdAerolinea = aero.Id
                    };

                    await contexto.AddAsync(ua);
                    await contexto.SaveChangesAsync();
                }

            }
        }

        public async Task EliminarAsync(string id)
        {
            var usuario = await ObtenerAsync(id);
            await administradorUsuario.DeleteAsync(usuario);
        }

        public async Task InsertarAsync(Usuario usuario, string clave, string rol, string aerolinea = "")
        {
            usuario.EmailConfirmed = true;
            var estado = await administradorUsuario.CreateAsync(usuario, clave);

            if (estado.Succeeded)
            {
                await administradorUsuario.AddToRoleAsync(usuario, rol);

                if (!string.IsNullOrEmpty(aerolinea))
                {
                    Aerolinea aero = new Aerolinea();
                    aero = await ObtenerAerolineaAsync(aerolinea);

                    if (aero != null)
                    {
                        UsuariosAerolineas ua = new UsuariosAerolineas
                        {
                            IdUsuario = usuario.Id,
                            IdAerolinea = aero.Id
                        };

                        await contexto.AddAsync(ua);
                        await contexto.SaveChangesAsync();
                    }

                }
            }
        }

        public async Task<Usuario> ObtenerAsync(string id)
        {
            var usuario = await contexto.Usuarios
                .Include(ua => ua.UsuariosAerolineas)
                    .ThenInclude(a => a.Aerolinea)
                    .ThenInclude(ah => ah.HorarioAerolinea)
                .Include(ur => ur.RolesUsuarios)
                    .ThenInclude(r => r.Rol)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

            return usuario;
        }

        public async Task<Usuario> ObtenerPorEmailAsync(string email)
        {
            var usuario = await contexto.Usuarios
                .Include(ua => ua.UsuariosAerolineas)
                    .ThenInclude(a => a.Aerolinea)
                    .ThenInclude(ah => ah.HorarioAerolinea)
                .Include(ur => ur.RolesUsuarios)
                    .ThenInclude(r => r.Rol)
            .FirstOrDefaultAsync(x => x.Email.Equals(email));

            return usuario;
        }

        public async Task<Usuario> ObtenerPorAliasAsync(string alias)
        {
            var usuario = await contexto.Usuarios
                .Include(ua => ua.UsuariosAerolineas)
                    .ThenInclude(a => a.Aerolinea)
                    .ThenInclude(ah => ah.HorarioAerolinea)
                .Include(ur => ur.RolesUsuarios)
                    .ThenInclude(r => r.Rol)
            .FirstOrDefaultAsync(x => x.UserName.Equals(alias));

            return usuario;
        }

        public async Task<bool> ObtenerPorusuarioyclave(string usuarioNombre, string clave)
        {
            PasswordHasher<string> pw = new PasswordHasher<string>();
            var usuario = await contexto.Usuarios
               .Include(ua => ua.UsuariosAerolineas)
                   .ThenInclude(a => a.Aerolinea)
                   .ThenInclude(ah => ah.HorarioAerolinea)
               .Include(ur => ur.RolesUsuarios)
                   .ThenInclude(r => r.Rol)
           .FirstOrDefaultAsync(x => x.UserName.Equals(usuarioNombre));
            string s1 = usuario.PasswordHash;
            Microsoft.AspNetCore.Identity.PasswordVerificationResult v1;
            v1 = pw.VerifyHashedPassword(usuarioNombre, s1, clave);
            if (v1 == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
            {
                return true;
            }
            else { return false; }
        }

        public async Task<bool> ActualizarclaveUsuario(string usuarioNombre, string clave)
        {
            Usuario userUpdated = new Usuario();
            userUpdated = await contexto.Usuarios
                .Include(ua => ua.UsuariosAerolineas)
                    .ThenInclude(a => a.Aerolinea)
                    .ThenInclude(ah => ah.HorarioAerolinea)
                .Include(ur => ur.RolesUsuarios)
                    .ThenInclude(r => r.Rol)
            .FirstOrDefaultAsync(x => x.UserName.Equals(usuarioNombre));
            var code = await administradorUsuario.GeneratePasswordResetTokenAsync(userUpdated);
            var estado = await administradorUsuario.ResetPasswordAsync(userUpdated, code, clave);
            return true;
        }
        public async Task<IList<Usuario>> ObtenerTodosAsync()
        {
            IList<Usuario> usuario = await contexto.Usuarios
                .Include(ua => ua.UsuariosAerolineas)
                    .ThenInclude(a => a.Aerolinea)
                    .ThenInclude(ah => ah.HorarioAerolinea)
                .Include(ur => ur.RolesUsuarios)
                    .ThenInclude(r => r.Rol)
            .ToListAsync();

            return usuario;
        }

        public async Task<Aerolinea> ObtenerAerolineaAsync(string nombre)
        {
            return await contexto.Aerolineas.FirstOrDefaultAsync(x => x.Nombre.ToUpper().Equals(nombre.ToUpper()));
        }
    }
}
