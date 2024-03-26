using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class PoliticasDeTratamientoDeDatosRepositorio : IPoliticasDeTratamientoDeDatosRepositorio
    {
        private readonly ContextoOpain _contexto;

        public PoliticasDeTratamientoDeDatosRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task InsertarAsync(PoliticasDeTratamientoDeDatos politicasDeTratamientoDeDatos)
        {
            await _contexto.AddAsync(politicasDeTratamientoDeDatos);
            await _contexto.SaveChangesAsync();
        }

        public async Task<IList<PoliticasDeTratamientoDeDatos>> ObtenerTodosAsync(string NombreUsuario)
        {
            return await _contexto.PoliticasDeTratamientoDeDatos.Where(x => x.NombreUsuario.Equals(NombreUsuario) ).ToListAsync();

        }
    }
}
