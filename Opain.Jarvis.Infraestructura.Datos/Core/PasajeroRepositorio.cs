using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class PasajeroRepositorio : IPasajeroRepositorio
    {
        private readonly ContextoOpain _contexto;

        public PasajeroRepositorio(ContextoOpain c)
        {
            _contexto = c;
        }

        public async Task InsertarAsync(Pasajero pasajero)
        {
            await _contexto.AddAsync(pasajero);
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Pasajero pasajero)
        {
            _contexto.Update(pasajero);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var tipoVuelo = await ObtenerAsync(id);
            _contexto.Pasajeros.Remove(tipoVuelo);
            _contexto.SaveChanges();
        }

        public async Task EliminarOperacionAsync(int idOperacion)
        {
            var pasajeros = _contexto.Pasajeros.Where(x => x.IdOperacionVuelo == idOperacion);
            _contexto.Pasajeros.RemoveRange(pasajeros);
            await _contexto.SaveChangesAsync();
        }

        public async Task<Pasajero> ObtenerAsync(int id)
        {
            return await _contexto.Pasajeros
                .Include(x => x.OperacionesVuelo)
                    .ThenInclude(x => x.Aerolinea)
                .Where(x => x.Id.Equals(id))
                      .FirstOrDefaultAsync();
        }

        public async Task<IList<Pasajero>> ObtenerTodosAsync(int idOperacion)
        {
            if (idOperacion == 0)
            {
                return await _contexto.Pasajeros
               .Include(x => x.OperacionesVuelo)
                       .ThenInclude(x => x.Aerolinea)
              .ToListAsync();
            }
            else
            {
                return await _contexto.Pasajeros
                    .Include(x => x.OperacionesVuelo)
                       .ThenInclude(x => x.Aerolinea)
               .Where(x => x.IdOperacionVuelo.Equals(idOperacion))
              .ToListAsync();
            }
        }

        public async Task InsertarMasivoAsync(IList<Pasajero> pasajero)
        {
            await _contexto.AddRangeAsync(pasajero);
            await _contexto.SaveChangesAsync();
        }

        public async Task<IList<Pasajero>> ObtenerExentos(int idOperacion)
        {
            
            return null;
        }
    }
}
