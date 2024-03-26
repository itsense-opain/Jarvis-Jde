using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class TasaAeroportuariaRepositorio : ITasaAeroportuariaRepositorio
    {
        private readonly ContextoOpain _contexto;

        public TasaAeroportuariaRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }
        public async Task ActualizarAsync(TasaAeroportuaria tasaAeroportuaria)
        {
            _contexto.Update(tasaAeroportuaria);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var tasa = await ObtenerAsync(id);
            _contexto.TasasAeroportuarias.Remove(tasa);
            _contexto.SaveChanges();
        }

        public async Task InsertarAsync(TasaAeroportuaria tasaAeroportuaria)
        {
            await _contexto.AddAsync(tasaAeroportuaria);
            await _contexto.SaveChangesAsync();
        }

        public async Task<TasaAeroportuaria> ObtenerAsync(int id)
        {
            return await _contexto.TasasAeroportuarias.FindAsync(id);
        }

        public async Task<IList<TasaAeroportuaria>> ObtenerTodosAsync()
        {
            return await _contexto.TasasAeroportuarias.ToListAsync();
        }
    }
}
