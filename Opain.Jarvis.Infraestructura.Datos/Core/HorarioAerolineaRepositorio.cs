using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class HorarioAerolineaRepositorio : IHorarioAerolineaRepositorio
    {
        private readonly ContextoOpain _contexto;


        public HorarioAerolineaRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task InsertarAsync(HorarioAerolinea horarioAerolinea)
        {
            horarioAerolinea.Aerolinea = null;

            await _contexto.AddAsync(horarioAerolinea);
            await _contexto.SaveChangesAsync();


        }

        public async Task ActualizarAsync(HorarioAerolinea horarioAerolinea)
        {
            horarioAerolinea.Aerolinea = await _contexto.Aerolineas.FirstOrDefaultAsync(x => x.Id.Equals(horarioAerolinea.IdAerolinea));
            _contexto.Update(horarioAerolinea);
            await _contexto.SaveChangesAsync();

        }

        public async Task EliminarAsync(int id)
        {
            var tipoVuelo = await ObtenerAsync(id);
            _contexto.HorariosAerolineas.Remove(tipoVuelo);
            _contexto.SaveChanges();
        }

        public async Task<HorarioAerolinea> ObtenerAsync(int id)
        {
            return await _contexto.HorariosAerolineas
                .Include(x => x.Aerolinea)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<IList<HorarioAerolinea>> ObtenerTodosAsync()
        {
            return await _contexto.HorariosAerolineas
                .Include(x => x.Aerolinea)
                .ToListAsync();
        }

      


    }
}
