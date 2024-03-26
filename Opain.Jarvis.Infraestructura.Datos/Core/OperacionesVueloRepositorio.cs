using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class OperacionesVueloRepositorio : IOperacionesVueloRepositorio
    {
        private readonly ContextoOpain contexto;

        public OperacionesVueloRepositorio(ContextoOpain c)
        {
            contexto = c;
        }

        public async Task InsertarAsync(OperacionesVuelo operacionesVuelo)
        {
            await contexto.AddAsync(operacionesVuelo);
            await contexto.SaveChangesAsync();
        }

        public async Task InsertarMasivoAsync(IList<OperacionesVuelo> operacionesVuelos)
        {
            await contexto.AddRangeAsync(operacionesVuelos).ConfigureAwait(false);
            await contexto.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task ActualizarAsync(OperacionesVuelo operacionesVuelo)
        {
            contexto.Update(operacionesVuelo);
            await contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var tipoVuelo = await ObtenerAsync(id);
            contexto.OperacionesVuelos.Remove(tipoVuelo);
            contexto.SaveChanges();
        }
        private async Task<IList<OperacionesVuelo>> ObtenerSuspencionesRegistradas(DateTime fechaVuelo, string numeroVuelo)
        {
            IList<OperacionesVuelo> operaciones = null;
            operaciones = await contexto.OperacionesVuelos
                      .Where(op => op.FechaVuelo == fechaVuelo 
                      && op.NumeroVuelo == numeroVuelo 
                      && (op.EstadoProceso == -9 || op.EstadoProceso == -10 || op.EstadoProceso == -11))
                       .ToListAsync();

            return operaciones;
        }
        public async Task<IList<OperacionesVuelo>> ObtenerSuspencionesNotificar()
        {
            IList<OperacionesVuelo> operaciones = null;
            operaciones = await contexto.OperacionesVuelos
                      .Where(op => op.EstadoProceso == -11).ToListAsync();

            return operaciones;
        }
        public async Task<OperacionesVuelo> ObtenerAsync(int id)
        {
            var operaciones = await contexto.OperacionesVuelos
                      .Include(vuelo => vuelo.Aerolinea)
                      .Include(op => op.Archivos)
                      .Include(op => op.Cargue)
                      .Include(op => op.NovedadesProceso)
                      .Where(op => op.Id.Equals(id))
                      .FirstOrDefaultAsync();

            return operaciones;
        }

        public async Task<IList<OperacionesVuelo>> ObtenerTodosAsync(DateTime fechaVueloInicio, DateTime fechaVueloFinal, string bandera = "0")
        {
            try
            {
                IList<OperacionesVuelo> operaciones = null;

                if (bandera == "CON")
                {
                    operaciones = await contexto.OperacionesVuelos
                       .Include(vuelo => vuelo.Aerolinea)
                       .Include(op => op.Archivos)
                       .Include(op => op.Cargue)
                       .Include(op => op.NovedadesProceso)
                       .Include(op => op.Pasajeros)
                       .Include(op => op.PasajerosTransitos)
                       //.Include(op => op.Destino)
                       .Where(x => (x.FechaVuelo >= fechaVueloInicio && x.FechaVuelo <= fechaVueloFinal) && x.EstadoProceso != 2 && x.EstadoProceso != 0 && (x.Id_Daily != null || x.Id_Daily != "0")).AsNoTracking()
                       .ToListAsync();
                }
                else if (bandera == "CONA")
                {
                    operaciones = await contexto.OperacionesVuelos
                          .Include(vuelo => vuelo.Aerolinea)
                          .Include(op => op.Archivos)
                          .Include(op => op.Cargue)
                          .Include(op => op.NovedadesProceso)
                          .Include(op => op.Pasajeros)
                          .Include(op => op.PasajerosTransitos)
                          //.Include(op => op.Destino)
                          .Where(x => (x.FechaVuelo >= fechaVueloInicio && x.FechaVuelo <= fechaVueloFinal) && x.EstadoProceso == 6  && x.EnvioNotificacion ==0).Take(100).AsNoTracking()
                          .ToListAsync();


                }

                else
                {
                    //IList<OperacionesVuelo> listADOoPERACIONESDE = new List<OperacionesVuelo>();

                    operaciones = await contexto.OperacionesVuelos
                           .Include(vuelo => vuelo.Aerolinea)
                           .Include(op => op.Archivos)
                           .Include(op => op.Cargue)
                           .Include(op => op.NovedadesProceso)
                           .Include(op => op.Pasajeros)
                           .Include(op => op.PasajerosTransitos)
                           //.Include(op => op.Destino)
                           .Where(x => x.FechaVuelo >= fechaVueloInicio && x.FechaVuelo <= fechaVueloFinal
                           && x.EstadoProceso != 5
                           && x.EstadoProceso != 6
                           && x.EstadoProceso != -10
                           && x.EstadoProceso != -11
                           && x.EstadoProceso != 0
                           && x.EstadoProceso != 2)
                           .ToListAsync();


                    //var res = operaciones.ToArray();
                    // DbCommand dc = contexto.Database.CurrentTransaction(operaciones);
                    //IQueryable<OperacionesVuelo> query = operaciones.base;
                }
                return operaciones;

            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace.ToString());
                throw;
            }
        }

        public async Task<IList<OperacionesVuelo>> ObtenerTodosAerolineaAsync(int IDAerolinea, DateTime fechaVueloInicio, DateTime fechaVueloFinal, string bandera = "0", string tipoVueloHistorico = "")
        {
            try
            {
                IList<OperacionesVuelo> operaciones = null;

                if (bandera == "CON")
                {
                    operaciones = await contexto.OperacionesVuelos
                       .Include(vuelo => vuelo.Aerolinea)
                       .Include(op => op.Archivos)
                       .Include(op => op.Cargue)
                       .Include(op => op.NovedadesProceso)
                       .Include(op => op.Pasajeros)
                       .Include(op => op.PasajerosTransitos)
                       //.Include(op => op.Destino)
                       .Where(x => x.FechaVuelo >= fechaVueloInicio && x.FechaVuelo <= fechaVueloFinal)
                       .ToListAsync();
                }
                else if (bandera == "CARGUE")
                {

                    operaciones = await contexto.OperacionesVuelos
                           .Include(vuelo => vuelo.Aerolinea)
                           .Include(op => op.Archivos)
                           .Include(op => op.Cargue)
                           .Include(op => op.NovedadesProceso)
                           .Include(op => op.Pasajeros)
                           .Include(op => op.PasajerosTransitos)
                           //.Include(op => op.Destino)
                           //.Where(x => x.FechaVuelo >= fechaVueloInicio && x.FechaVuelo <= fechaVueloFinal && x.EstadoProceso != 5 && x.EstadoProceso != 6
                           .Where(x => x.FechaVuelo >= fechaVueloInicio && x.FechaVuelo <= fechaVueloFinal
                           && x.EstadoProceso != 5
                           && x.EstadoProceso != -10
                           && x.EstadoProceso != -11
                           && x.EstadoProceso != 2
                           && x.IdAerolinea == IDAerolinea)
                           .ToListAsync();
                }
                else if (bandera == "LIQ")
                {
                    operaciones = await contexto.OperacionesVuelos
                                  .Include(vuelo => vuelo.Aerolinea)
                                  .Include(op => op.Archivos)
                                  .Include(op => op.Cargue)
                                  .Include(op => op.NovedadesProceso)
                                  .Include(op => op.Pasajeros)
                                  .Include(op => op.PasajerosTransitos)
                                  //.Include(op => op.Destino)
                                  //.Where(x => x.FechaVuelo >= fechaVueloInicio && x.FechaVuelo <= fechaVueloFinal && x.EstadoProceso != 5 && x.EstadoProceso != 6
                                  .Where(x => x.FechaVuelo >= fechaVueloInicio && x.FechaVuelo <= fechaVueloFinal
                                  && x.EstadoProceso != 5
                                  && x.EstadoProceso != 6
                                  && x.EstadoProceso != -10
                                  && x.EstadoProceso != -11
                                  && x.EstadoProceso != 2
                                  && x.EstadoProceso ==1
                                  && x.IdAerolinea == IDAerolinea)
                                  .ToListAsync();


                }
                else
                {
                    operaciones = await contexto.OperacionesVuelos
                              .Include(vuelo => vuelo.Aerolinea)
                              .Include(op => op.Archivos)
                              .Include(op => op.Cargue)
                              .Include(op => op.NovedadesProceso)
                              .Include(op => op.Pasajeros)
                              .Include(op => op.PasajerosTransitos)
                              //.Include(op => op.Destino)
                              //.Where(x => x.FechaVuelo >= fechaVueloInicio && x.FechaVuelo <= fechaVueloFinal && x.EstadoProceso != 5 && x.EstadoProceso != 6
                              .Where(x => x.FechaVuelo >= fechaVueloInicio && x.FechaVuelo <= fechaVueloFinal &&
                                   (string.IsNullOrEmpty(tipoVueloHistorico) || x.TipoVuelo == tipoVueloHistorico)
                              && x.EstadoProceso != 5
                              && x.EstadoProceso != 6
                              && x.EstadoProceso != -10
                              && x.EstadoProceso != -11
                              && x.EstadoProceso != 2
                              && x.EstadoProceso != 0
                              && x.IdAerolinea == IDAerolinea)
                              .ToListAsync();

                }
                return operaciones;

            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace.ToString());
                throw;
            }
        }

        public async Task<IList<OperacionesVuelo>> ObtenerPorAerolineaYFechaAsync(int idAerolinea, DateTime fecha, string bandera)
        {
            IList<OperacionesVuelo> operaciones = null;

                operaciones = await contexto.OperacionesVuelos
                       .Include(vuelo => vuelo.Aerolinea)
                       .Include(op => op.Cargue)
                       .Where(x => x.FechaVuelo.Equals(fecha) && x.IdAerolinea.Equals(idAerolinea) && x.EstadoProceso != 5 && x.EstadoProceso != 6)
                       .ToListAsync();

            return operaciones;
        }

        public async Task<IList<Aerolinea>> ObtenerHorarioIdAerolineaAsync(int idAerolinea)
        {
            IList<Aerolinea> _aerolinea =null;

            _aerolinea = await contexto.Aerolineas
                   .Include(vuelo => vuelo.HorarioAerolinea)
                   .Where(x=>x.Id.Equals(idAerolinea))
                   .ToListAsync();
            return _aerolinea;
        }

        public async Task SuspenderAsync(int id)
        {
            OperacionesVuelo OperacionVuelo = await ObtenerAsync(id);
            IList<OperacionesVuelo> Suspenciones = await ObtenerSuspencionesRegistradas(OperacionVuelo.FechaVuelo, OperacionVuelo.NumeroVuelo);
            if(Suspenciones.Count>=1)
            {
                throw new Exception("Supero el límite de suspensiones.");
            }
            OperacionVuelo.FechaCreacion = DateTime.Now;
            OperacionVuelo.EstadoProceso = -9;
            contexto.SaveChanges();
        }

        public async Task ActualizarSuspencionAsync(int id)
        {
            OperacionesVuelo OperacionVuelo = await ObtenerAsync(id);            
            OperacionVuelo.FechaCreacion = DateTime.Now;
            OperacionVuelo.EstadoProceso = 4;
            contexto.SaveChanges();
        }
    }
}
