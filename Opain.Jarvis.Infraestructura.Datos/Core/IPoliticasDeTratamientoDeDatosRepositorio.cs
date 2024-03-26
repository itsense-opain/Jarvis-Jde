using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IPoliticasDeTratamientoDeDatosRepositorio
    {
        Task InsertarAsync(PoliticasDeTratamientoDeDatos politicasDeTratamientoDeDatos);

        Task<IList<PoliticasDeTratamientoDeDatos>> ObtenerTodosAsync(string NombreUsuario);
    }
}
