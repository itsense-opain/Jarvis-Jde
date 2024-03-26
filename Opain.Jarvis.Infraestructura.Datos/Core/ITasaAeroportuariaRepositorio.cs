using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface ITasaAeroportuariaRepositorio
    {
        Task InsertarAsync(TasaAeroportuaria tasaAeroportuaria);
        Task ActualizarAsync(TasaAeroportuaria tasaAeroportuaria);
        Task EliminarAsync(int id);
        Task<TasaAeroportuaria> ObtenerAsync(int id);
        Task<IList<TasaAeroportuaria>> ObtenerTodosAsync();
    }
}
