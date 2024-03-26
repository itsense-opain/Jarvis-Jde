using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface INovedadRepositorio
    {
        Task InsertarAsync(NovedadProceso novedad);
        Task ActualizarAsync(NovedadProceso novedad);
        Task EliminarAsync(int id);
        Task<NovedadProceso> ObtenerAsync(int id);
        Task<IList<NovedadProceso>> ObtenerTodosPorOperacionAsync(int id);
        Task<IList<NovedadProceso>> ObtenerTodosAsync();
    }
}
