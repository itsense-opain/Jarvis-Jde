using AutoMapper;
using Opain.Jarvis.Aplicacion;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public class HorarioOperacionAplicacion : IHorarioOperacionAplicacion
    {
        private readonly IHorarioOperacionRepositorio horarioRepositorio;
        private readonly IMapper mapper;

        public HorarioOperacionAplicacion(IHorarioOperacionRepositorio horario, IMapper m)
        {
            mapper = m;
            horarioRepositorio = horario;
        }

        public async Task ActualizarAsync(HorarioOperacionOtd horarioOperacionOtd)
        {
            var horario = mapper.Map<HorarioOperacionOtd, HorarioOperacion>(horarioOperacionOtd);
            await horarioRepositorio.ActualizarAsync(horario);
        }

        public async Task ActualizarTodosAsync(List<HorarioOperacionOtd> horariosOperacionesOtd)
        {
            List<HorarioOperacion> horarios = new List<HorarioOperacion>();

            foreach (var item in horariosOperacionesOtd)
            {
                var horario = mapper.Map<HorarioOperacionOtd, HorarioOperacion>(item);
                horarios.Add(horario);
            }

            await horarioRepositorio.ActualizarTodosAsync(horarios);
        }

        public async Task EliminarAsync(int id)
        {
            await horarioRepositorio.EliminarAsync(id);
        }

        public async Task InsertarAsync(HorarioOperacionOtd horarioOperacionOtd)
        {
            var horarios = await ObtenerTodosAsync();

            var duplicado = horarios.FirstOrDefault(x => x.Dia.Equals(horarioOperacionOtd.Dia));

            if(duplicado != null)
            {
                HorarioOperacionOtd horarioOperacion = new HorarioOperacionOtd
                {
                     Id = duplicado.Id,
                     Dia = horarioOperacionOtd.Dia,
                     HoraInicio = horarioOperacionOtd.HoraInicio,
                     HoraFin = horarioOperacionOtd.HoraFin
                };

                var horario = mapper.Map<HorarioOperacionOtd, HorarioOperacion>(horarioOperacion);
                await horarioRepositorio.ActualizarAsync(horario);
            }
            else
            {
                var horario = mapper.Map<HorarioOperacionOtd, HorarioOperacion>(horarioOperacionOtd);
                await horarioRepositorio.InsertarAsync(horario);
            }            
        }

        public async Task<HorarioOperacionOtd> ObtenerAsync(int id)
        {
            var horario = await horarioRepositorio.ObtenerAsync(id);
            var horarioOtd = mapper.Map<HorarioOperacion, HorarioOperacionOtd>(horario);

            return horarioOtd;
        }

        public async Task<IList<HorarioOperacionOtd>> ObtenerTodosAsync()
        {
            List<HorarioOperacionOtd> horariosOtd = new List<HorarioOperacionOtd>();

            var horarios = await horarioRepositorio.ObtenerTodosAsync().ConfigureAwait(false);

            foreach (var item in horarios)
            {
                var h = mapper.Map<HorarioOperacion, HorarioOperacionOtd>(item);
                horariosOtd.Add(h);
            }

            return horariosOtd;
        }
    }
}
