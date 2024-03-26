using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class HorarioOperacionRepositorio : IHorarioOperacionRepositorio
    {
        private readonly ContextoOpain _contexto;

        public HorarioOperacionRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }
        public async Task ActualizarAsync(HorarioOperacion horarioOperacion)
        {
            _contexto.Update(horarioOperacion);
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarTodosAsync(List<HorarioOperacion> horarioOperaciones)
        {
            _contexto.UpdateRange(horarioOperaciones);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var horarioOperacion = await ObtenerAsync(id);
            _contexto.HorariosOperaciones.Remove(horarioOperacion);
            _contexto.SaveChanges();
        }

        public async Task InsertarAsync(HorarioOperacion horarioOperacion)
        {
            await _contexto.AddAsync(horarioOperacion);
            await _contexto.SaveChangesAsync();
        }

        public async Task<HorarioOperacion> ObtenerAsync(int id)
        {
            return await _contexto.HorariosOperaciones.FindAsync(id);
        }

        public async Task<IList<HorarioOperacion>> ObtenerTodosAsync()
        {
            return await _contexto.HorariosOperaciones.AsNoTracking().ToListAsync();
        }
    }
}
