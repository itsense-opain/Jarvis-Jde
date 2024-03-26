using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;




namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class CargueRepositorio : ICargueRepositorio
    {
        private readonly ContextoOpain _contexto;





        public CargueRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;

        }

        public async Task<CargueArchivo> InsertarAsync(CargueArchivo cargue)
        {
            await _contexto.AddAsync(cargue);
            await _contexto.SaveChangesAsync();
            return cargue;

        }

        public async Task<IList<CargueArchivo>> ObtenerTodosAsync(DateTime inicio, DateTime fin)
        {
            return await _contexto.CargueArchivos.Where(x => x.Fecha <= fin && x.Fecha >= inicio).ToListAsync();
        }
    }
}
