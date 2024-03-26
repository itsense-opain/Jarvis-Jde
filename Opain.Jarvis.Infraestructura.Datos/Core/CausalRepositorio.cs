using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class CausalRepositorio : ICausalRepositorio
    {
        private readonly ContextoOpain _contexto;

        public CausalRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task ActualizarAsync(Causal causal)
        {
            _contexto.Update(causal);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var causal = await ObtenerAsync(id);
            _contexto.Causales.Remove(causal);
            _contexto.SaveChanges();
        }

        public async Task InsertarAsync(Causal causal)
        {
            await _contexto.AddAsync(causal);
            await _contexto.SaveChangesAsync();
        }

        public async Task<Causal> ObtenerAsync(int id)
        {
            return await _contexto.Causales.FindAsync(id);
        }

        public async Task<Causal> ObtenerPorCodigoAsync(string id)
        {
            return await _contexto.Causales.FirstOrDefaultAsync(x => x.Codigo.Equals(id) && x.Estado.Equals(1));
        }

        public async Task<IList<Causal>> ObtenerTodosAsync()
        {
            return await _contexto.Causales.ToListAsync();
        }

        public async Task<IList<Causal>> ObtenerPorTipoAsync(int tipo)
        {
            return await _contexto.Causales.Where(x => x.Estado == 1).ToListAsync();
        }
    }
}
