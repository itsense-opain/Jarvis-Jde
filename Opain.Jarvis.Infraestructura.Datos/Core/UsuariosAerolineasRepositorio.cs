using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class UsuariosAerolineasRepositorio : IUsuariosAerolineasRepositorio
    {
        private readonly ContextoOpain _contexto;


        public UsuariosAerolineasRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task InsertarAsync(UsuariosAerolineas usuariosAerolineas)
        {
            await _contexto.AddAsync(usuariosAerolineas);
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarAsync(UsuariosAerolineas usuariosAerolineas)
        {
            _contexto.Update(usuariosAerolineas);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var usuariosAerolineas = await ObtenerAsync(id);
            _contexto.UsuariosAerolineas.Remove(usuariosAerolineas);
            _contexto.SaveChanges();
        }

        public async Task<UsuariosAerolineas> ObtenerAsync(int id)
        {
            return await _contexto.UsuariosAerolineas.FindAsync(id);
        }

        public async Task<IList<UsuariosAerolineas>> ObtenerTodosAsync()
        {
            return await _contexto.UsuariosAerolineas.ToListAsync();
        }

    }
}
