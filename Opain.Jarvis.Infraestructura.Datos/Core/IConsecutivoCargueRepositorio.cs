using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IConsecutivoCargueRepositorio
    {
        Task<int> InsertarAsync(Cargue cargue);
        Task<Cargue> ObtenerAsync(int id);
    }
}
