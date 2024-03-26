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
    public class NovedadRepositorio : INovedadRepositorio
    {
        private readonly ContextoOpain _contexto;

        public NovedadRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task ActualizarAsync(NovedadProceso novedad)
        {
            _contexto.Update(novedad);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var novedad = await ObtenerAsync(id);
            _contexto.NovedadesProcesos.Remove(novedad);
            _contexto.SaveChanges();
        }

        public async Task InsertarAsync(NovedadProceso novedad)
        {
            await _contexto.AddAsync(novedad);
            await _contexto.SaveChangesAsync();
        }

        public async Task<NovedadProceso> ObtenerAsync(int id)
        {
            return await _contexto.NovedadesProcesos
                .Include(x => x.Causal)
                .Include(x => x.OperacionesVuelo)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<IList<NovedadProceso>> ObtenerTodosAsync()
        {
            return await _contexto.NovedadesProcesos
                .Include(x => x.Causal)
                .Include(x => x.OperacionesVuelo)
                .ToListAsync();
        }

        public async Task<IList<NovedadProceso>> ObtenerTodosPorOperacionAsync(int id)
        {
            return await _contexto.NovedadesProcesos
                .Include(x => x.Causal)
                .Include(x => x.OperacionesVuelo)
                .Where(x => x.IdOperacionVuelo.Equals(id))
                .ToListAsync();
        }
    }
}
