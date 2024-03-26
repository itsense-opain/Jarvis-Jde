using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class AerolineaRepositorio : IAerolineaRepositorio
    {
        private readonly ContextoOpain _contexto;


        public AerolineaRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task InsertarAsync(Aerolinea aerolinea)
        {

            await _contexto.AddAsync(aerolinea);
            await _contexto.SaveChangesAsync();

        }

        public async Task ActualizarAsync(Aerolinea aerolinea)
        {

            _contexto.Update(aerolinea);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {

            var tipoVuelo = await ObtenerAsync(id);
            _contexto.Aerolineas.Remove(tipoVuelo);
            _contexto.SaveChanges();

        }

        public async Task<Aerolinea> ObtenerAsync(int id)
        {

            return await _contexto.Aerolineas.FindAsync(id);

        } 

        public async Task<IList<Aerolinea>> ObtenerTodosAsync()
        {

           return await _contexto.Aerolineas.ToListAsync();

        }

        public async Task ActualizarTodosAsync(List<Aerolinea> aerolineas)
        {
            _contexto.Update(aerolineas);
            await _contexto.SaveChangesAsync();
        }

        public async Task<IList<Aerolinea>> ObtenerHorarioIdAerolineaAsync(int idAerolinea)
        {
            IList<Aerolinea> _aerolinea = null;

            _aerolinea = await _contexto.Aerolineas
                   .Include(a => a.HorarioAerolinea)
                   .Where(x => x.Id.Equals(idAerolinea))
                   .ToListAsync();

            return _aerolinea;
        }


    }
}
