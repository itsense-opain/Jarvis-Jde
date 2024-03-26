using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class PaisRepositorio : IPaisRepositorio
    {
        private readonly ContextoOpain _contexto;


        public PaisRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task InsertarAsync(Pais pais)
        {
            await _contexto.AddAsync(pais);
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Pais pais)
        {
            _contexto.Update(pais);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(string id)
        {
            var tipoVuelo = await ObtenerAsync(id);
            _contexto.Paises.Remove(tipoVuelo);
            _contexto.SaveChanges();

        }

        public async Task<Pais> ObtenerAsync(string id)
        {
            return await _contexto.Paises.FindAsync(id);
        }

        public async Task<IList<Pais>> ObtenerTodosAsync()
        {
            return await _contexto.Paises.ToListAsync();
        }

    }
}
