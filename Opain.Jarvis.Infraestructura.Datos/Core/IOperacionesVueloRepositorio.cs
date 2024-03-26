using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public interface IOperacionesVueloRepositorio
    {
        Task InsertarAsync(OperacionesVuelo operacionesVuelo);
        Task InsertarMasivoAsync(IList<OperacionesVuelo> operacionesVuelos);
        Task ActualizarAsync(OperacionesVuelo operacionesVuelo);
        Task EliminarAsync(int id);
        Task<OperacionesVuelo> ObtenerAsync(int id);
        Task<IList<OperacionesVuelo>> ObtenerTodosAsync(DateTime fechaVueloInicio, DateTime fechaVueloFinal, string bandera);
        Task<IList<OperacionesVuelo>> ObtenerTodosAerolineaAsync(int IDAerolinea, DateTime fechaVueloInicio, DateTime fechaVueloFinal, string bandera, string tipoVueloHistorico);
        Task SuspenderAsync(int id);
        Task<IList<OperacionesVuelo>> ObtenerSuspencionesNotificar();
        Task ActualizarSuspencionAsync(int id);
    }
}
