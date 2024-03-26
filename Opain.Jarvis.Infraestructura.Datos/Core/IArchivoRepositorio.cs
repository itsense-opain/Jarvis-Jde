using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IArchivoRepositorio
    {
        Task InsertarAsync(Archivo archivo);
        Task InsertarMasivoAsync(IList<Archivo> archivo);
        Task ActualizarAsync(Archivo archivo);
        Task EliminarAsync(int id);
        Task<Archivo> ObtenerAsync(int id);
        Task<IList<Archivo>> ObtenerPorOperacionAsync(int idOperacion);
    }
}