using Opain.Jarvis.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public interface IOperacionVueloAplicacion
    {
        Task InsertarAsync(OperacionVueloOtd operacionVueloOtd);
        Task InsertarMasivoAsync(IList<OperacionVueloOtd> operacionVueloOtd);
        Task ActualizarAsync(OperacionVueloOtd operacionVueloOtd);
        Task EliminarAsync(int id);
        Task<OperacionVueloOtd> ObtenerAsync(int id);
        Task<IList<OperacionVueloOtd>> ObtenerTodosAsync(DateTime fechaVueloInicio, DateTime fechaVueloFinal, string bandera);
        Task<IList<OperacionVueloOtd>> ObtenerTodosAerolineaAsync(int IDAerolinea, DateTime fechaVueloInicio, DateTime fechaVueloFinal, string bandera, string tipoVueloHistorico);
        Task ProcesarArchivoAsync(StreamReader archivo);
        Task<string> ConfirmarVuelosAsync(List<int> ids);
        Task SuspenderAsync(int id);
        Task<IList<OperacionVueloOtd>> ObtenerSuspencionesNotificar();
        Task ActualizarSuspencionAsync(int id);

    }
}
