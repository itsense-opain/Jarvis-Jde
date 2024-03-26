using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class VueloRepositorio : IVueloRepositorio
    {
        private readonly ContextoOpain contexto;


        public VueloRepositorio(ContextoOpain c)
        {
            contexto = c;
        }

        public async Task InsertarAsync(Vuelo vuelo)
        {
            await contexto.AddAsync(vuelo);
            await contexto.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Vuelo vuelo)
        {
            contexto.Update(vuelo);
            await contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var vuelo = await ObtenerAsync(id);
            contexto.Vuelos.Remove(vuelo);
            contexto.SaveChanges();
        }

        public async Task<Vuelo> ObtenerAsync(int id)
        {
            return await contexto.Vuelos.FindAsync(id);
        }

        public async Task<Vuelo> ObtenerPorNombreAsync(string nombre)
        {
            return await contexto.Vuelos.Include(X => X.Aerolinea).Where(x => x.NumeroVuelo.ToUpper() == nombre.ToUpper()).FirstOrDefaultAsync();
        }

        public async Task<IList<Vuelo>> ObtenerTodosAsync()
        {
            return await contexto.Vuelos.ToListAsync();
        }

    }
}
