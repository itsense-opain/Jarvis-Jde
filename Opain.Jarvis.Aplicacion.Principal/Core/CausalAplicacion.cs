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
    public class CausalAplicacion : ICausalAplicacion
    {
        private readonly ICausalRepositorio causalRepositorio;
        private readonly IMapper mapper;

        public CausalAplicacion(ICausalRepositorio causal, IMapper m)
        {
            causalRepositorio = causal;
            mapper = m;
        }

        public async Task ActualizarAsync(CausalOtd causalOtd)
        {
            var causal = mapper.Map<CausalOtd, Causal>(causalOtd);
            await causalRepositorio.ActualizarAsync(causal);
        }

        public async Task EliminarAsync(int id)
        {
            await causalRepositorio.EliminarAsync(id);
        }

        public async Task InsertarAsync(CausalOtd causalOtd)
        {
            var causal = mapper.Map<CausalOtd, Causal>(causalOtd);
            await causalRepositorio.InsertarAsync(causal);
        }

        public async Task<CausalOtd> ObtenerAsync(int id)
        {
            var causal = await causalRepositorio.ObtenerAsync(id);
            var causalOtd = mapper.Map<Causal, CausalOtd>(causal);

            return causalOtd;
        }

        public async Task<CausalOtd> ObtenerPorCodigoAsync(string id)
        {
            var causal = await causalRepositorio.ObtenerPorCodigoAsync(id);
            var causalOtd = mapper.Map<Causal, CausalOtd>(causal);

            return causalOtd;
        }

        public async Task<IList<CausalOtd>> ObtenerTodosAsync()
        {
            List<CausalOtd> causalesOtd = new List<CausalOtd>();

            var causales = await causalRepositorio.ObtenerTodosAsync().ConfigureAwait(false);

            foreach (var item in causales)
            {
                var a = mapper.Map<Causal, CausalOtd>(item);
                causalesOtd.Add(a);
            }

            return causalesOtd;
        }
        public async Task<IList<CausalOtd>> ObtenerPorTipoAsync(int tipo)
        {
            List<CausalOtd> causalesOtd = new List<CausalOtd>();

            var causales = await causalRepositorio.ObtenerPorTipoAsync(tipo).ConfigureAwait(false);

            foreach (var item in causales)
            {
                var a = mapper.Map<Causal, CausalOtd>(item);
                causalesOtd.Add(a);
            }

            return causalesOtd;
        }
    }
}
