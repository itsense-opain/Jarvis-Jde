using AutoMapper;
using Opain.Jarvis.Aplicacion;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public class PasajeroTransitoAplicacion : IPasajeroTransitoAplicacion
    {
        private readonly IPasajeroTransitoRepositorio pasajeroTransitoRepositorio;
        
        private readonly IVueloRepositorio vueloRepositorio;
        private readonly IMapper mapper;

        public PasajeroTransitoAplicacion(IPasajeroTransitoRepositorio ptr, IVueloRepositorio vr, IMapper m)
        {
            pasajeroTransitoRepositorio = ptr;
            vueloRepositorio = vr;
            mapper = m;
        }

        public async Task ActualizarAsync(PasajeroTransitoOtd pasajeroTransitoOtd)
        {
            PasajeroTransito p = mapper.Map<PasajeroTransitoOtd, PasajeroTransito>(pasajeroTransitoOtd);

            var vueloLlegada = await vueloRepositorio.ObtenerPorNombreAsync(pasajeroTransitoOtd.NumeroVueloLlegada);
            var vueloSalida = await vueloRepositorio.ObtenerPorNombreAsync(pasajeroTransitoOtd.NumeroVueloSalida);

            if (vueloLlegada != null)
            {
                p.IdVueloLlegada = vueloLlegada.Id;
            }
            else
            {
                p.IdVueloLlegada = 0;
            }

            if (vueloSalida != null)
            {
                p.IdVueloSalida = vueloSalida.Id;
            }
            else
            {
                p.IdVueloSalida = 0;
            }

            await pasajeroTransitoRepositorio.ActualizarAsync(p).ConfigureAwait(false);
        }

        public async Task EliminarAsync(int id)
        {
            await pasajeroTransitoRepositorio.EliminarAsync(id).ConfigureAwait(false);
        }

        public async Task InsertarAsync(PasajeroTransitoOtd pasajeroTransitoOtd)
        {
            PasajeroTransito p = mapper.Map<PasajeroTransitoOtd, PasajeroTransito>(pasajeroTransitoOtd);

            var vueloLlegada = await vueloRepositorio.ObtenerPorNombreAsync(pasajeroTransitoOtd.NumeroVueloLlegada);
            var vueloSalida = await vueloRepositorio.ObtenerPorNombreAsync(pasajeroTransitoOtd.NumeroVueloSalida);

            if (vueloLlegada != null)
            {
                p.IdVueloLlegada = vueloLlegada.Id;
                p.AerolineaLlegada = vueloLlegada.Aerolinea.Nombre;
            }
            else
            {
                p.IdVueloLlegada = 0;
            }

            if (vueloSalida != null)
            {
                p.IdVueloSalida = vueloSalida.Id;
                p.AerolineaSalida = vueloSalida.Aerolinea.Nombre;
            }
            else
            {
                p.IdVueloSalida = 0;
            }
            p.FechaHoraCargue = DateTime.Now;

            await pasajeroTransitoRepositorio.InsertarAsync(p).ConfigureAwait(false);
        }

        public async Task ProcesarArchivoAsync(StreamReader archivo, string nombreArchivo)
        {
            DateTime fecha = DateTime.Now;
            IList<PasajeroTransito> pasajeros = new List<PasajeroTransito>();
            IList<PasajeroTransitoOtd> pasajerosOtd = new List<PasajeroTransitoOtd>();
            string nombre = nombreArchivo.Replace(".txt", "").Replace(".csv", "").Replace("TT", "");
            var datos = nombre.Split("-");
            int idOperacionVuelo = int.Parse(datos[0]);

            string linea;
            while ((linea = archivo.ReadLine()) != null)
            {
                var campos = linea.Split(",");
                pasajerosOtd.Add(new PasajeroTransitoOtd()
                {
                    FechaLlegada = new DateTime(int.Parse(campos[0].Substring(6, 4)), int.Parse(campos[0].Substring(3, 2)), int.Parse(campos[0].Substring(0, 2))),
                    HoraLlegada = campos[1].Substring(0, 2) + ":" + campos[1].Substring(3, 2),
                    NumeroVueloLlegada = campos[2],
                    Origen = campos[3],
                    FechaSalida = new DateTime(int.Parse(campos[4].Substring(6, 4)), int.Parse(campos[4].Substring(3, 2)), int.Parse(campos[4].Substring(0, 2))),
                    HoraSalida = campos[5].Substring(0, 2) + ":" + campos[5].Substring(3, 2),
                    NumeroVueloSalida = campos[6],
                    Destino = campos[7],
                    NombrePasajero = campos[8],
                    TTC  = int.Parse(campos[9]),
                    TTL = int.Parse(campos[10])
                });
            }

            foreach (var item in pasajerosOtd)
            {
                var pasajero = mapper.Map<PasajeroTransitoOtd, PasajeroTransito>(item);
                pasajero.IdOperacionVuelo = idOperacionVuelo;
                pasajero.IdVueloLlegada = vueloRepositorio.ObtenerPorNombreAsync(item.NumeroVueloLlegada).Result.Id;
                pasajero.IdVueloSalida = vueloRepositorio.ObtenerPorNombreAsync(item.NumeroVueloSalida).Result.Id;
                pasajero.FechaHoraCargue = fecha;
                pasajeros.Add(pasajero);
            }

            await pasajeroTransitoRepositorio.EliminarOperacionAsync(idOperacionVuelo);

            await pasajeroTransitoRepositorio.InsertarMasivoAsync(pasajeros);
        }

        public async Task<PasajeroTransitoOtd> ObtenerAsync(int id)
        {
            var pasajeroTransito = await pasajeroTransitoRepositorio.ObtenerAsync(id).ConfigureAwait(false);
            var p = mapper.Map<PasajeroTransito, PasajeroTransitoOtd>(pasajeroTransito);

            return p;
        }

        public async Task<IList<PasajeroTransitoOtd>> ObtenerTodosAsync(int idOperacion)
        {
            List<PasajeroTransitoOtd> pasajeros = new List<PasajeroTransitoOtd>();

            var pasajeroRep = await pasajeroTransitoRepositorio.ObtenerTodosAsync(idOperacion).ConfigureAwait(false);

            foreach (var item in pasajeroRep)
            {
                var p = mapper.Map<PasajeroTransito, PasajeroTransitoOtd>(item);
                pasajeros.Add(p);
            }

            return pasajeros;
        }

        public async Task<IList<PasajeroTransitoOtd>> ObtenerAerolineaAsync(string Aerolinea)
        {
            List<PasajeroTransitoOtd> pasajeros = new List<PasajeroTransitoOtd>();

            var pasajeroRep = await pasajeroTransitoRepositorio.ObtenerAerolineaAsync(Aerolinea).ConfigureAwait(false);

            foreach (var item in pasajeroRep)
            {
                var p = mapper.Map<PasajeroTransito, PasajeroTransitoOtd>(item);
                pasajeros.Add(p);
            }

            return pasajeros;
        }


        public async Task InsertarMasivoAsync(IList<PasajeroTransitoOtd> pasajerosTransitoOtd)
        {
            int idOperacionVuelo = pasajerosTransitoOtd.FirstOrDefault().Operacion;
            DateTime fecha = DateTime.Now;
            IList<PasajeroTransito> pasajeros = new List<PasajeroTransito>();
            foreach (var item in pasajerosTransitoOtd)
            {
                var pasajero = mapper.Map<PasajeroTransitoOtd, PasajeroTransito>(item);
                pasajero.IdVueloLlegada = 0;
                pasajero.IdVueloSalida = 0;
                pasajero.FechaHoraCargue = fecha;
                pasajeros.Add(pasajero);
                // aca va el insert into notify multiple
                NotificacionODT notify = new NotificacionODT();
            }

            await pasajeroTransitoRepositorio.EliminarOperacionAsync(idOperacionVuelo);

            await pasajeroTransitoRepositorio.InsertarMasivoAsync(pasajeros);


        }

         
        public async Task ActualizarVuelosFirmadosAsync(int IdOperacionVuelo)
        {
             

            //PasajeroTransito p = mapper.Map<PasajeroTransitoOtd, PasajeroTransito>(pasajeroTransitoOtd);

            //var vueloLlegada = await vueloRepositorio.ObtenerPorNombreAsync(pasajeroTransitoOtd.NumeroVueloLlegada);
            //var vueloSalida = await vueloRepositorio.ObtenerPorNombreAsync(pasajeroTransitoOtd.NumeroVueloSalida);

            //if (vueloLlegada != null)
            //{
            //    p.IdVueloLlegada = vueloLlegada.Id;
            //}
            //else
            //{
            //    p.IdVueloLlegada = 0;
            //}

            //if (vueloSalida != null)
            //{
            //    p.IdVueloSalida = vueloSalida.Id;
            //}
            //else
            //{
            //    p.IdVueloSalida = 0;
            //}

            //await pasajeroTransitoRepositorio.ActualizarAsync(p).ConfigureAwait(false);
        }
    }
}
