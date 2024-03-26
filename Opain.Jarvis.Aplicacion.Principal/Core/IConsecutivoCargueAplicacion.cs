using System;
using Opain.Jarvis.Dominio.Entidades;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface IConsecutivoCargueAplicacion
    {
        Task<int> InsertarAsync(ConsecutivoCargueOtd cargueOtd);
        Task<ConsecutivoCargueOtd> ObtenerAsync(int id);
    }
}
