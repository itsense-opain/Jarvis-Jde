using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface IArchivoAplicacion
    {
        Task InsertarAsync(ArchivoOtd archivo);
        Task InsertarMasivoAsync(IList<ArchivoOtd> archivo);
        Task ActualizarAsync(ArchivoOtd archivo);
        Task  EliminarAsync(int id);
        Task<ArchivoOtd> ObtenerAsync(int id);
        Task<IList<ArchivoOtd>> ObtenerPorOperacionAsync(int idOperacion);
    }
}
