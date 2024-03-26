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
    public class ConsecutivoCargueRepositorio : IConsecutivoCargueRepositorio
    {
        private readonly ContextoOpain _contexto;

        public ConsecutivoCargueRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task<int> InsertarAsync(Cargue cargue)
        {
            await _contexto.AddAsync(cargue);
            await _contexto.SaveChangesAsync();

            return cargue.Id;
        }

        public async Task<Cargue> ObtenerAsync(int id)
        {
            return await _contexto.Cargues
                .Include(x => x.OperacionesVuelos)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}
