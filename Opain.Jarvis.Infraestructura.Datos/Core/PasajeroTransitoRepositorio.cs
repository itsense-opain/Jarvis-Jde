using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class PasajeroTransitoRepositorio : IPasajeroTransitoRepositorio
    {
        private readonly ContextoOpain contexto;


        public PasajeroTransitoRepositorio(ContextoOpain c)
        {
            contexto = c;
        }

        public async Task InsertarAsync(PasajeroTransito pasajeroTransito)
        {
            await contexto.AddAsync(pasajeroTransito);
            await contexto.SaveChangesAsync();
        }

        public async Task ActualizarAsync(PasajeroTransito pasajeroTransito)
        {
            contexto.Update(pasajeroTransito);
            await contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var tipoVuelo = await ObtenerAsync(id);
            contexto.PasajerosTransito.Remove(tipoVuelo);
            contexto.SaveChanges();
        }

        public async Task EliminarOperacionAsync(int idOperacion)
        {
            var pasajeros = contexto.PasajerosTransito.Where(x => x.IdOperacionVuelo.Equals(idOperacion));
            contexto.PasajerosTransito.RemoveRange(pasajeros);
            await contexto.SaveChangesAsync();
        }

        public async Task<PasajeroTransito> ObtenerAsync(int id)
        {
            return await contexto.PasajerosTransito
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync(); //<PasajeroTransito>();//(m => m.Id == id);
        }

        public async Task<IList<PasajeroTransito>> ObtenerTodosAsync(int idOperacion)
        {           

            if (idOperacion == 0)
            {
                return await contexto.PasajerosTransito
               .Include(x => x.OperacionesVuelo).ThenInclude(x => x.Aerolinea)
               .Where (x => x.Categoria.Equals("TTC"))
               .ToListAsync();
            }
            else
            {
                return await contexto.PasajerosTransito
               .Include(x => x.OperacionesVuelo).ThenInclude(x => x.Aerolinea)
               .Where(x => x.IdOperacionVuelo.Equals(idOperacion))
               .ToListAsync();
            }
        }

        public async Task<IList<PasajeroTransito>> ObtenerAerolineaAsync(string Aerolinea)
        {

               return await contexto.PasajerosTransito
               .Include(x => x.OperacionesVuelo).ThenInclude(x => x.Aerolinea)
               .Where(x => x.Categoria.Equals("TTC") && x.AerolineaLlegada.Equals(Aerolinea))
               .ToListAsync();
         
         }



        public async Task InsertarMasivoAsync(IList<PasajeroTransito> pasajeros)
        {
            await contexto.AddRangeAsync(pasajeros);
            await contexto.SaveChangesAsync();
        }
    }
}
