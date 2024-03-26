using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface ICargueRepositorio
    {
        Task<CargueArchivo> InsertarAsync(CargueArchivo cargue);

        Task<IList<CargueArchivo>> ObtenerTodosAsync(DateTime inicio, DateTime fin);
    }
}
