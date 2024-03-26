using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface ITasaAeroportuariaAplicacion
    {
        Task InsertarAsync(TasaAeroportuariaOtd tasaAeroportuariaOtd);
        Task ActualizarAsync(TasaAeroportuariaOtd tasaAeroportuariaOtd);
        Task EliminarAsync(int id);
        Task<TasaAeroportuariaOtd> ObtenerAsync(int id);
        Task<IList<TasaAeroportuariaOtd>> ObtenerTodosAsync();
        Task<TasaAeroportuariaOtd> ObtenerUltimaAsync();
    }
}
