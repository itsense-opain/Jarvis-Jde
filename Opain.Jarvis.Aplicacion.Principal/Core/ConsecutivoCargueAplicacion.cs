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
    public class ConsecutivoCargueAplicacion : IConsecutivoCargueAplicacion
    {
        private readonly IConsecutivoCargueRepositorio consecutivoCargueRepositorio;
        private readonly IMapper mapper;

        public ConsecutivoCargueAplicacion(IConsecutivoCargueRepositorio ConsecutivoCargue, IMapper m)
        {
            consecutivoCargueRepositorio = ConsecutivoCargue;
            mapper = m;
        }

        public async Task<int> InsertarAsync(ConsecutivoCargueOtd cargueOtd)
        {
            var cargue = mapper.Map<ConsecutivoCargueOtd, Cargue>(cargueOtd);
           return await consecutivoCargueRepositorio.InsertarAsync(cargue);
        }

        public async Task<ConsecutivoCargueOtd> ObtenerAsync(int id)
        {
            var cargue = await consecutivoCargueRepositorio.ObtenerAsync(id);
            var cargueOtd = mapper.Map<Cargue, ConsecutivoCargueOtd>(cargue);

            return cargueOtd;
        }
    }
}
