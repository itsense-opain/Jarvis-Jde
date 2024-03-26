using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class ArchivoRepositorio : IArchivoRepositorio
    {
        private readonly ContextoOpain _contexto;


        public ArchivoRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task InsertarAsync(Archivo archivo)
        {
            await _contexto.AddAsync(archivo);
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Archivo archivo)
        {

            _contexto.Update(archivo);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {

            var tipoVuelo = await ObtenerAsync(id);
            _contexto.Archivos.Remove(tipoVuelo);
            _contexto.SaveChanges();


        }

        public async Task<Archivo> ObtenerAsync(int id)
        {
            return await _contexto.Archivos.FindAsync(id);
        }

        public async Task InsertarMasivoAsync(IList<Archivo> archivo)
        {
            await _contexto.AddRangeAsync(archivo);
            await _contexto.SaveChangesAsync();
        }

        public async Task<IList<Archivo>> ObtenerPorOperacionAsync(int idOperacion)
        {
            return await _contexto.Archivos.Where(x => x.IdOperacionVuelo == idOperacion).ToListAsync();
        }
    }
}
