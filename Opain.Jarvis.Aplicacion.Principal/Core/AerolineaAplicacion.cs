using AutoMapper;
using Opain.Jarvis.Aplicacion;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Dominio.Entidades;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Opain.Jarvis.Infraestructura.Datos.Core;
namespace Opain.Jarvis.Aplicacion
{
    public class AerolineaAplicacion : IAerolineaAplicacion
    {
        private readonly IAerolineaRepositorio aerolineaRepositorio;
        private readonly IMapper mapper;

        public AerolineaAplicacion(IAerolineaRepositorio aerolinea, IMapper m)
        {
            mapper = m;
            aerolineaRepositorio = aerolinea;
        }

        public async Task ActualizarAsync(AerolineaOtd aerolineaOtd)
        {
            var aerolinea = mapper.Map<AerolineaOtd, Aerolinea>(aerolineaOtd);
            await aerolineaRepositorio.ActualizarAsync(aerolinea);
        }

        public async Task ActualizarTodosAsync(List<AerolineaOtd> aerolineasOtd)
        {
            List<Aerolinea> aerolineas = new List<Aerolinea>();

            foreach (var item in aerolineasOtd)
            {
                var aerolinea = mapper.Map<AerolineaOtd, Aerolinea>(item);
                aerolineas.Add(aerolinea);
            }
            
            await aerolineaRepositorio.ActualizarTodosAsync(aerolineas);
        }

        public async Task EliminarAsync(int id)
        {
            await aerolineaRepositorio.EliminarAsync(id);
        }

        public async Task InsertarAsync(AerolineaOtd aerolineaOtd)
        {
            var aerolinea = mapper.Map<AerolineaOtd, Aerolinea>(aerolineaOtd);
            await aerolineaRepositorio.InsertarAsync(aerolinea);
        }

        public async Task<AerolineaOtd> ObtenerAsync(int id)
        {
            var aerolinea = await aerolineaRepositorio.ObtenerAsync(id);
            var aerolineaOtd = mapper.Map<Aerolinea, AerolineaOtd>(aerolinea);

            return aerolineaOtd;
        }

        public async Task<IList<AerolineaOtd>> ObtenerTodosAsync()
        {
            List<AerolineaOtd> aerolineasOtd = new List<AerolineaOtd>();

            var aerolineas = await aerolineaRepositorio.ObtenerTodosAsync().ConfigureAwait(false);

            foreach (var item in aerolineas)
            {
                var a = mapper.Map<Aerolinea, AerolineaOtd>(item);
                aerolineasOtd.Add(a);
            }

            return aerolineasOtd;
        }



        public async Task<IList<AerolineaOtd>> ObtenerHorarioIdAerolineaAsync(int IdAeroinea)
        {
            List<AerolineaOtd> aerolineasOtd = new List<AerolineaOtd>();

            var aerolineas = await aerolineaRepositorio.ObtenerHorarioIdAerolineaAsync(IdAeroinea);

            foreach (var item in aerolineas)
            {
                var a = mapper.Map<Aerolinea, AerolineaOtd>(item);
                aerolineasOtd.Add(a);
            }

            return aerolineasOtd;
        }


    }
}
