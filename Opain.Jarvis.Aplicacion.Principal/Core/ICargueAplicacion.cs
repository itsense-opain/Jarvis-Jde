using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface ICargueAplicacion
    {
        Task <CargueOtd> InsertarAsync(CargueOtd cargue);

        Task<IList<CargueOtd>> ObtenerTodosAsync(DateTime inicio, DateTime fin);
    }
}
