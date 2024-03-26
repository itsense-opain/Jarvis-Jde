using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface IPoliticasDeTratamientoDeDatosAplicacion
    {
        Task InsertarAsync(PoliticasDeTratamientoDeDatosOtd politicasDeTratamientoDeDatos);

        Task<bool> ObtenerTodosAsync(string NombreUsuario);
    }
}
