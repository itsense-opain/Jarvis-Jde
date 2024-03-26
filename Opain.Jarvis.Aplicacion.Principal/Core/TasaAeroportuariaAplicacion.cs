using AutoMapper;
using Opain.Jarvis.Aplicacion;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Dominio.Entidades;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opain.Jarvis.Infraestructura.Datos.Core;

namespace Opain.Jarvis.Aplicacion
{
    public class TasaAeroportuariaAplicacion : ITasaAeroportuariaAplicacion
    {
        private readonly ITasaAeroportuariaRepositorio tasaRepositorio;
        private readonly IMapper mapper;

        public TasaAeroportuariaAplicacion(IMapper map, ITasaAeroportuariaRepositorio tasa)
        {
            mapper = map;
            tasaRepositorio = tasa;
        }

        public async Task ActualizarAsync(TasaAeroportuariaOtd tasaAeroportuariaOtd)
        {
            var tasa = mapper.Map<TasaAeroportuariaOtd, TasaAeroportuaria>(tasaAeroportuariaOtd);
            await tasaRepositorio.ActualizarAsync(tasa);
        }

        public async Task EliminarAsync(int id)
        {
            await tasaRepositorio.EliminarAsync(id);
        }

        public async Task InsertarAsync(TasaAeroportuariaOtd tasaAeroportuariaOtd)
        {
            var tasa = mapper.Map<TasaAeroportuariaOtd, TasaAeroportuaria>(tasaAeroportuariaOtd);
            await tasaRepositorio.InsertarAsync(tasa);
        }

        public async Task<TasaAeroportuariaOtd> ObtenerAsync(int id)
        {
            var tasa = await tasaRepositorio.ObtenerAsync(id);
            var tasaOdt = mapper.Map<TasaAeroportuaria, TasaAeroportuariaOtd>(tasa);

            return tasaOdt;
        }

        public async Task<IList<TasaAeroportuariaOtd>> ObtenerTodosAsync()
        {
            List<TasaAeroportuariaOtd> tasasOtd = new List<TasaAeroportuariaOtd>();

            var tasas = await tasaRepositorio.ObtenerTodosAsync().ConfigureAwait(false);

            foreach (var item in tasas)
            {
                var t = mapper.Map<TasaAeroportuaria, TasaAeroportuariaOtd>(item);
                tasasOtd.Add(t);
            }

            return tasasOtd;
        }

        public async Task<TasaAeroportuariaOtd> ObtenerUltimaAsync()
        {
            var tasas = await tasaRepositorio.ObtenerTodosAsync().ConfigureAwait(false);

            var tasa = tasas.OrderByDescending(x => x.Id).FirstOrDefault();

            var tasaOtd = mapper.Map<TasaAeroportuaria, TasaAeroportuariaOtd>(tasa);

            return tasaOtd;
        }
    }
}
