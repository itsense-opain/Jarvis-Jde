using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class CiudadRepositorio : ICiudadRepositorio
    {
        private readonly ContextoOpain _contexto;


        public CiudadRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task InsertarAsync(Ciudad ciudad)
        {
            await _contexto.AddAsync(ciudad);
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Ciudad ciudad)
        {
            _contexto.Update(ciudad);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(string id)
        {
            var ciudad = await ObtenerAsync(id);
            _contexto.Ciudades.Remove(ciudad);
            _contexto.SaveChanges();
        }

        public async Task<Ciudad> ObtenerAsync(string id)
        {
            return await _contexto.Ciudades.FindAsync(id);
        }

        public async Task<IList<Ciudad>> ObtenerTodosAsync()
        {
            return await _contexto.Ciudades.ToListAsync();
        }

    }
}
