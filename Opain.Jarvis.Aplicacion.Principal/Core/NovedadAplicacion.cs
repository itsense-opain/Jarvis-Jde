using AutoMapper;
using Opain.Jarvis.Aplicacion;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public class NovedadAplicacion : INovedadAplicacion
    {
        private readonly INovedadRepositorio novedadRepositorio;
        private readonly IMapper mapper;

        public NovedadAplicacion(INovedadRepositorio novedad, IMapper m)
        {
            novedadRepositorio = novedad;
            mapper = m;
        }

        public async Task ActualizarAsync(NovedadOtd novedadOtd)
        {
            var novedad = mapper.Map<NovedadOtd, NovedadProceso>(novedadOtd);
            await novedadRepositorio.ActualizarAsync(novedad);
        }

        public async Task EliminarAsync(int id)
        {
            await novedadRepositorio.EliminarAsync(id);
        }

        public async Task InsertarAsync(NovedadOtd novedadOtd)
        {
            var novedad = mapper.Map<NovedadOtd, NovedadProceso>(novedadOtd);
            await novedadRepositorio.InsertarAsync(novedad);
        }

        public async Task<NovedadOtd> ObtenerAsync(int id)
        {
            var novedad = await novedadRepositorio.ObtenerAsync(id);
            var novedadOtd = mapper.Map<NovedadProceso, NovedadOtd>(novedad);

            return novedadOtd;
        }

        public async Task<IList<NovedadOtd>> ObtenerTodosAsync()
        {
            List<NovedadOtd> novedadesOtd = new List<NovedadOtd>();

            var novedades = await novedadRepositorio.ObtenerTodosAsync().ConfigureAwait(false);

            foreach (var item in novedades)
            {
                var a = mapper.Map<NovedadProceso, NovedadOtd>(item);
                novedadesOtd.Add(a);
            }

            return novedadesOtd;
        }

        public async Task<IList<NovedadOtd>> ObtenerTodosPorOperacionAsync(int id)
        {
            List<NovedadOtd> novedadesOtd = new List<NovedadOtd>();

            var novedades = await novedadRepositorio.ObtenerTodosPorOperacionAsync(id).ConfigureAwait(false);

            foreach (var item in novedades)
            {
                var a = mapper.Map<NovedadProceso, NovedadOtd>(item);
                novedadesOtd.Add(a);
            }

            return novedadesOtd;
        }
    }
}
