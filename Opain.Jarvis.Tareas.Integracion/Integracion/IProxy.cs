using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Tareas.Integracion
{
    interface IProxy
    {
        Task<T> GetAsync<T>(string servicio);
        Task<T> PostAsync<T>(string servicio, object parametros);
        Task<T> DeleteAsync<T>(string servicio);
        Task<T> PutAsync<T>(string servicio, object parametros);
    }
}
