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
    public class OperacionVueloAplicacion : IOperacionVueloAplicacion
    {
        private readonly IOperacionesVueloRepositorio operacionesVueloRepositorio;
        private readonly IPasajeroAplicacion pasajeroAplicacion;
        private readonly IPasajeroTransitoAplicacion pasajeroTransitoAplicacion;
        private readonly IVueloRepositorio vueloRepositorio;
        private readonly INovedadAplicacion novedadAplicacion;
        private readonly ICausalAplicacion causalAplicacion;
        private readonly IMapper mapper;

        public OperacionVueloAplicacion(
            IOperacionesVueloRepositorio OperacionesVueloRepositorio,
            IVueloRepositorio VueloRepositorio,
            IPasajeroAplicacion pasajero,
            IPasajeroTransitoAplicacion pasajeroTransito,
            INovedadAplicacion novedad,
            ICausalAplicacion causal,
            IMapper Mapper)
        {
            operacionesVueloRepositorio = OperacionesVueloRepositorio;
            vueloRepositorio = VueloRepositorio;
            pasajeroAplicacion = pasajero;
            pasajeroTransitoAplicacion = pasajeroTransito;
            novedadAplicacion = novedad;
            causalAplicacion = causal;
            mapper = Mapper;
        }

        public async Task ActualizarAsync(OperacionVueloOtd operacionVueloOtd)
        {
            operacionVueloOtd.FechaCreacion = DateTime.Now;
            OperacionesVuelo operacionVuelo = mapper.Map<OperacionVueloOtd, OperacionesVuelo>(operacionVueloOtd);

            var vuelo = await vueloRepositorio.ObtenerPorNombreAsync(operacionVueloOtd.Vuelo);

            if (vuelo != null)
            {
                operacionVuelo.IdVuelo = vuelo.Id;
            }
            else
            {
                if(operacionVuelo.IdVuelo == null)
                    operacionVuelo.IdVuelo = 0;
            }

            await operacionesVueloRepositorio.ActualizarAsync(operacionVuelo).ConfigureAwait(false);
        }

        public async Task EliminarAsync(int id)
        {
            await operacionesVueloRepositorio.EliminarAsync(id).ConfigureAwait(false);
        }

        public async Task InsertarAsync(OperacionVueloOtd operacionVueloOtd)
        {
            operacionVueloOtd.ConfirmacionGenDec = 0;
            operacionVueloOtd.ConfirmacionManifiesto = 0;
            operacionVueloOtd.ConfirmacionOperacion = 0;
            operacionVueloOtd.ConfirmacionPasajeros = 0;
            operacionVueloOtd.ConfirmacionTransitos = 0;
            operacionVueloOtd.FechaCreacion = DateTime.Now;
            //    operacionVueloOtd.EstadoProceso = "En Proceso";

            var operacionVuelo = mapper.Map<OperacionVueloOtd, OperacionesVuelo>(operacionVueloOtd);

            var vuelo = await vueloRepositorio.ObtenerPorNombreAsync(operacionVueloOtd.Vuelo);
            if (vuelo != null)
            {
                operacionVuelo.IdVuelo = vuelo.Id;
            }
            else
            {
                operacionVuelo.IdVuelo = 0;
            }

            await operacionesVueloRepositorio.InsertarAsync(operacionVuelo).ConfigureAwait(false);
        }

        public async Task InsertarMasivoAsync(IList<OperacionVueloOtd> operacionVueloOtd)
        {
            IList<OperacionesVuelo> listadoOpVuelo = new List<OperacionesVuelo>();

            foreach (var item in operacionVueloOtd)
            {
                item.ConfirmacionGenDec = 0;
                item.ConfirmacionManifiesto = 0;
                item.ConfirmacionOperacion = 0;
                item.ConfirmacionPasajeros = 0;
                item.ConfirmacionTransitos = 0;
                item.FechaCreacion = DateTime.Now;
                // item.EstadoProceso = "En Proceso";

                var operacionVuelo = mapper.Map<OperacionVueloOtd, OperacionesVuelo>(item);

                var vuelo = await vueloRepositorio.ObtenerPorNombreAsync(item.Vuelo);
                if (vuelo != null)
                {
                    operacionVuelo.IdVuelo = vuelo.Id;
                }
                else
                {
                    operacionVuelo.IdVuelo = 0;
                }

                listadoOpVuelo.Add(operacionVuelo);
            }

            await operacionesVueloRepositorio.InsertarMasivoAsync(listadoOpVuelo).ConfigureAwait(false);
        }

        public async Task<OperacionVueloOtd> ObtenerAsync(int id)
        {
            var operacionVuelo = await operacionesVueloRepositorio.ObtenerAsync(id).ConfigureAwait(false);
            var vuelo = mapper.Map<OperacionesVuelo, OperacionVueloOtd>(operacionVuelo);

            return vuelo;
        }

        public async Task<IList<OperacionVueloOtd>> ObtenerTodosAsync(DateTime fechaVueloInicio, DateTime fechaVueloFinal, string bandera = "0")
        {
            List<OperacionVueloOtd> operacionesVueloOtd = new List<OperacionVueloOtd>();
            int cont=0;
            
            var operacionesVuelo = await operacionesVueloRepositorio.ObtenerTodosAsync(fechaVueloInicio, fechaVueloFinal, bandera).ConfigureAwait(false);
            var numeroOperacionesVuelos = operacionesVuelo.Count()==0?0: operacionesVuelo.Count();

            // List<string> names = operacionesVuelo.Select(user => user.PasajerosTransitos.Select(x=>x.Firmado.Equals(1)).Select).ToList();
            List<Opain.Jarvis.Dominio.Entidades.OperacionesVuelo> Lista = new List<OperacionesVuelo>();

            if (bandera == "LIQ")
            {
                bool SePuedeLiquidar = true;
                foreach (Opain.Jarvis.Dominio.Entidades.OperacionesVuelo Vuelo in operacionesVuelo)
                {
                    SePuedeLiquidar = true;
                    //PasajeroTransitoOtd consulta = new PasajeroTransitoOtd();
                    if (Vuelo.PasajerosTransitos.Count > 0)
                    {
                        foreach (Opain.Jarvis.Dominio.Entidades.PasajeroTransito Pasajero in Vuelo.PasajerosTransitos)
                        {
                            if (Pasajero.Firmado == 0)
                            {
                                SePuedeLiquidar = false;
                            }
                        }
                    }
                    if (SePuedeLiquidar)
                    {
                        Lista.Add(Vuelo);
                    }
                }
                foreach (var item in Lista)
                {
                    var vuelo = mapper.Map<OperacionesVuelo, OperacionVueloOtd>(item);
                    operacionesVueloOtd.Add(vuelo);                    
                }
            }
            else if (bandera == "INF")
            {
                Lista = (List<Opain.Jarvis.Dominio.Entidades.OperacionesVuelo>)operacionesVuelo;
                foreach (var item in Lista)
                {
                    var vuelo = mapper.Map<OperacionesVuelo, OperacionVueloOtd>(item);
                    operacionesVueloOtd.Add(vuelo);
                    cont++;
                    if (cont > numeroOperacionesVuelos)
                    {
                        break;
                    }
                }
            }
            else if (bandera == "0")
            {
                Lista = (List<Opain.Jarvis.Dominio.Entidades.OperacionesVuelo>)operacionesVuelo;
                foreach (var item in Lista)
                {
                    var vuelo = mapper.Map<OperacionesVuelo, OperacionVueloOtd>(item);
                    operacionesVueloOtd.Add(vuelo);
                }
            }
            else 
            {
                Lista = (List<Opain.Jarvis.Dominio.Entidades.OperacionesVuelo>)operacionesVuelo;
                foreach (var item in Lista)
                {
                    var vuelo = mapper.Map<OperacionesVuelo, OperacionVueloOtd>(item);
                    operacionesVueloOtd.Add(vuelo);
                }
            }
            return operacionesVueloOtd;
        }
        public async Task<IList<OperacionVueloOtd>> ObtenerTodosAerolineaAsync(int IDAerolinea, DateTime fechaVueloInicio, DateTime fechaVueloFinal, string bandera = "0", string tipoVueloHistorico = "")
        {
            List<OperacionVueloOtd> operacionesVueloOtd = new List<OperacionVueloOtd>();
            int cont = 0;
            List<Opain.Jarvis.Dominio.Entidades.OperacionesVuelo> Lista = new List<OperacionesVuelo>();
            var operacionesVuelo = await operacionesVueloRepositorio.ObtenerTodosAerolineaAsync(IDAerolinea,fechaVueloInicio, fechaVueloFinal, bandera, tipoVueloHistorico).ConfigureAwait(false);
            var numeroOperacionesVuelos = operacionesVuelo.Count() == 0 ? 0 : operacionesVuelo.Count();

            if (bandera == "LIQ")
            {
                bool SePuedeLiquidar = true;
                foreach (Opain.Jarvis.Dominio.Entidades.OperacionesVuelo Vuelo in operacionesVuelo)
                {
                    SePuedeLiquidar = true;
                    //PasajeroTransitoOtd consulta = new PasajeroTransitoOtd();
                    if (Vuelo.TipoVuelo == "INT")
                    {
                        if (Vuelo.PasajerosTransitos.Count > 0)
                        {
                            foreach (Opain.Jarvis.Dominio.Entidades.PasajeroTransito Pasajero in Vuelo.PasajerosTransitos)
                            {
                                if (Pasajero.Firmado == 0 && Pasajero.Categoria=="TTC")
                                {
                                    SePuedeLiquidar = false;
                                    break;
                                }
                            }
                        }
                    }                    
                    if (SePuedeLiquidar)
                    {
                        Lista.Add(Vuelo);
                    }
                }
                foreach (var item in Lista)
                {
                    var vuelo = mapper.Map<OperacionesVuelo, OperacionVueloOtd>(item);
                    operacionesVueloOtd.Add(vuelo);
                }
            }
            else if (bandera == "INF")
            {                
                foreach (var item in operacionesVuelo)
                {
                    var vuelo = mapper.Map<OperacionesVuelo, OperacionVueloOtd>(item);
                    operacionesVueloOtd.Add(vuelo);
                    cont = cont + 1;
                    if (cont > numeroOperacionesVuelos)
                    {
                        break;
                    }
                }
            }
            else
            {
                foreach (var item in operacionesVuelo)
                {
                    var vuelo = mapper.Map<OperacionesVuelo, OperacionVueloOtd>(item);
                    operacionesVueloOtd.Add(vuelo);
                }
            }
            return operacionesVueloOtd;
        }

        public async Task ProcesarArchivoAsync(StreamReader archivo)
        {
            IList<OperacionVueloOtd> listadoOtd = new List<OperacionVueloOtd>();
            IList<OperacionesVuelo> listadoOpVuelo = new List<OperacionesVuelo>();

            string linea;

            while ((linea = archivo.ReadLine()) != null)
            {
                var campos = linea.Split(",");

                int pax = int.Parse(campos[6]) - int.Parse(campos[7]) + int.Parse(campos[8]) + int.Parse(campos[9]) + int.Parse(campos[10]) + int.Parse(campos[11]);

                listadoOtd.Add(new OperacionVueloOtd
                {
                    Fecha = new DateTime(int.Parse(campos[0].Substring(6, 4)), int.Parse(campos[0].Substring(3, 2)), int.Parse(campos[0].Substring(0, 2))),
                    Matricula = campos[2],
                    Vuelo = campos[3],
                    Hora = campos[5].Substring(0, 2) + ":" + campos[5].Substring(2, 2),
                    TotalEmbarcados = int.Parse(campos[6]),
                    INF = int.Parse(campos[7]),
                    TTL = int.Parse(campos[8]),
                    TTC = int.Parse(campos[9]),
                    EX = int.Parse(campos[10]),
                    TRIP = int.Parse(campos[11]),
                    PagoCOP = int.Parse(campos[12]),
                    PagoUSD = int.Parse(campos[13]),
                    ConfirmacionGenDec = 0,
                    ConfirmacionManifiesto = 0,
                    ConfirmacionOperacion = 0,
                    ConfirmacionPasajeros = 0,
                    ConfirmacionTransitos = 0,
                    PAX = pax
                });
            }

            foreach (var item in listadoOtd)
            {
                var vuelo = mapper.Map<OperacionVueloOtd, OperacionesVuelo>(item);
                vuelo.IdVuelo = vueloRepositorio.ObtenerPorNombreAsync(item.Vuelo).Result.Id;
                listadoOpVuelo.Add(vuelo);
            }

            await operacionesVueloRepositorio.InsertarMasivoAsync(listadoOpVuelo).ConfigureAwait(false);

        }

        public async Task<string> ConfirmarVuelosAsync(List<int> ids)
        {
            foreach (var item in ids)
            {
                var operacion = await ObtenerAsync(item);
                var pasajeros = await pasajeroAplicacion.ObtenerTodosAsync(item);
                var transitos = await pasajeroTransitoAplicacion.ObtenerTodosAsync(item);

                NovedadesPasajeros(operacion, pasajeros);
                NovedadesTransitos(operacion, transitos, pasajeros);
            }

            return "Vuelos han sido confirmados";
        }

        public void NovedadesPasajeros(OperacionVueloOtd operacion, IList<PasajeroOtd> pasajeros)
        {
            if(pasajeros.Count != operacion.TotalEmbarcados)
            {
                novedadAplicacion.InsertarAsync(
                    new NovedadOtd
                    {
                        Operacion = operacion.Id,
                        TipoNovedad = "Cargue",
                        IdCausal = 14,
                        DescNovedad = $"La cantidad de pasajeros reportados en el cierre ({operacion.TotalEmbarcados}) no es igual a la cantidad de pasajeros cargados ({pasajeros.Count}).",
                        FechaHora = DateTime.Now,
                        IdRegistro = 0
                    });
            }

            if(pasajeros.Count(x => x.Categoria.Equals("INF")) != operacion.INF)
            {
                novedadAplicacion.InsertarAsync(
                    new NovedadOtd
                    {
                        Operacion = operacion.Id,
                        TipoNovedad = "Cargue",
                        IdCausal = 15,
                        DescNovedad = $"La cantidad de infantes reportados en el cierre ({operacion.INF}) no es igual a la cantidad de pasajeros con Categoría infante ({pasajeros.Count(x => x.Categoria.Equals("INF"))}).",
                        FechaHora = DateTime.Now,
                        IdRegistro = 0
                    });
            }

            if(pasajeros.Count(x => x.Categoria.Equals("TRIP")) != operacion.TRIP)
            {
                novedadAplicacion.InsertarAsync(
                    new NovedadOtd
                    {
                        Operacion = operacion.Id,
                        TipoNovedad = "Cargue",
                        IdCausal = 19,
                        DescNovedad = $"La cantidad de tripulación reportados en el cierre ({operacion.TRIP}) no es igual a la cantidad de pasajeros con Categoría tripulante ({pasajeros.Count(x => x.Categoria.Equals("TRIP"))}).",
                        FechaHora = DateTime.Now,
                        IdRegistro = 0
                    });
            }

            if(pasajeros.Count(x => x.Categoria.Equals("TTL")) != operacion.TTL)
            {
                novedadAplicacion.InsertarAsync(
                    new NovedadOtd
                    {
                        Operacion = operacion.Id,
                        TipoNovedad = "Cargue",
                        IdCausal = 23,
                        DescNovedad = $"La cantidad de tránsitos en línea reportados en el cierre ({operacion.TTL}) no es igual a la cantidad de pasajeros con Categoría tránsito en línea ({pasajeros.Count(x => x.Categoria.Equals("TTL"))}).",
                        FechaHora = DateTime.Now,
                        IdRegistro = 0
                    });
            }

            if(pasajeros.Count(x => x.Categoria.Equals("TTC")) != operacion.TTC)
            {
                novedadAplicacion.InsertarAsync(
                    new NovedadOtd
                    {
                        Operacion = operacion.Id,
                        TipoNovedad = "Cargue",
                        IdCausal = 24,
                        DescNovedad = $"La cantidad de tránsitos en conexión reportados en el cierre ({operacion.TTC}) no es igual a la cantidad de pasajeros con categoria tránsito en conexión ({pasajeros.Count(x => x.Categoria.Equals("TTC"))}).",
                        FechaHora = DateTime.Now,
                        IdRegistro = 0
                    });
            }

            if (pasajeros.Count(x => x.Categoria.Equals("EX")) != operacion.EX)
            {
                novedadAplicacion.InsertarAsync(
                    new NovedadOtd
                    {
                        Operacion = operacion.Id,
                        TipoNovedad = "Cargue",
                        IdCausal = 25,
                        DescNovedad = $"La cantidad de excentos reportados en el cierre ({operacion.TTC}) no es igual a la cantidad de pasajeros con categoria excentos ({pasajeros.Count(x => x.Categoria.Equals("TTC"))}).",
                        FechaHora = DateTime.Now,
                        IdRegistro = 0
                    });
            }

            Parallel.ForEach(pasajeros, (pasajero) => {
                if (pasajero.Fecha != operacion.Fecha)
                {
                    novedadAplicacion.InsertarAsync(
                        new NovedadOtd
                        {
                            Operacion = operacion.Id,
                            TipoNovedad = "Cargue",
                            IdCausal = 16,
                            DescNovedad = $"La fecha del vuelo cierre ({operacion.Fecha}) no concuerda con la fecha del pasajero {pasajero.NombrePasajero} ({pasajero.Fecha}).",
                            FechaHora = DateTime.Now,
                            IdRegistro = pasajero.Id
                        });
                }

                if (pasajero.NumeroVuelo != operacion.Vuelo)
                {
                    novedadAplicacion.InsertarAsync(
                        new NovedadOtd
                        {
                            Operacion = operacion.Id,
                            TipoNovedad = "Cargue",
                            IdCausal = 18,
                            DescNovedad = $"El Número de vuelo cierre ({operacion.Vuelo}) no concuerda con el número de vuelo del pasajero {pasajero.NombrePasajero} ({pasajero.NumeroVuelo}).",
                            FechaHora = DateTime.Now,
                            IdRegistro = pasajero.Id
                        });
                }

                if (pasajero.MatriculaVuelo != operacion.Matricula)
                {
                    novedadAplicacion.InsertarAsync(
                        new NovedadOtd
                        {
                            Operacion = operacion.Id,
                            TipoNovedad = "Cargue",
                            IdCausal = 22,
                            DescNovedad = $"El Número de matrícula del vuelo cierre ({operacion.Matricula}) no concuerda con el número de matrícula del vuelo del pasajero {pasajero.NombrePasajero} ({pasajero.MatriculaVuelo}).",
                            FechaHora = DateTime.Now,
                            IdRegistro = pasajero.Id
                        });
                }
            });
        }

        public void NovedadesTransitos(OperacionVueloOtd operacion, IList<PasajeroTransitoOtd> transitos, IList<PasajeroOtd> pasajeros)
        {
            if(transitos.Count(x => x.TTC.Equals(1)) != operacion.TTC)
            {
                novedadAplicacion.InsertarAsync(
                    new NovedadOtd
                    {
                        Operacion = operacion.Id,
                        TipoNovedad = "Cargue",
                        IdCausal = 26,
                        DescNovedad = $"La cantidad de tránsitos en conexión reportados en el cierre ({operacion.TTC}) no es igual a la cantidad de tránsito en conexión cargados ({transitos.Count(x => x.TTC.Equals(1))}).",
                        FechaHora = DateTime.Now,
                        IdRegistro = 0
                    });
            }

            if(transitos.Count(x => x.TTL.Equals(1)) != operacion.TTL)
            {
                novedadAplicacion.InsertarAsync(
                    new NovedadOtd
                    {
                        Operacion = operacion.Id,
                        TipoNovedad = "Cargue",
                        IdCausal = 27,
                        DescNovedad = $"La cantidad de tránsitos en línea reportados en el cierre ({operacion.TTL}) no es igual a la cantidad de tránsito en línea cargados ({transitos.Count(x => x.TTL.Equals(1))}).",
                        FechaHora = DateTime.Now,
                        IdRegistro = 0
                    });
            }

            Parallel.ForEach(transitos, (transito) =>
            {
                if (transito.FechaSalida != operacion.Fecha)
                {
                    novedadAplicacion.InsertarAsync(
                        new NovedadOtd
                        {
                            Operacion = operacion.Id,
                            TipoNovedad = "Cargue",
                            IdCausal = 31,
                            DescNovedad = $"La fecha del vuelo cierre ({operacion.Fecha}) no concuerda con la fecha de salida del pasajero {transito.NombrePasajero} ({transito.FechaSalida}).",
                            FechaHora = DateTime.Now,
                            IdRegistro = transito.Id
                        });
                }

                if (transito.NumeroVueloSalida != operacion.Vuelo)
                {
                    novedadAplicacion.InsertarAsync(
                        new NovedadOtd
                        {
                            Operacion = operacion.Id,
                            TipoNovedad = "Cargue",
                            IdCausal = 33,
                            DescNovedad = $"El Número de vuelo cierre ({operacion.Vuelo}) no concuerda con el número de vuelo de salida del pasajero {transito.NombrePasajero} ({transito.NumeroVueloSalida}).",
                            FechaHora = DateTime.Now,
                            IdRegistro = transito.Id
                        });
                }

                if (transito.Destino != operacion.Destino)
                {
                    novedadAplicacion.InsertarAsync(
                        new NovedadOtd
                        {
                            Operacion = operacion.Id,
                            TipoNovedad = "Cargue",
                            IdCausal = 32,
                            DescNovedad = $"El destino del vuelo cierre ({operacion.Destino}) no concuerda con el destino del pasajero {transito.NombrePasajero} ({transito.Destino}).",
                            FechaHora = DateTime.Now,
                            IdRegistro = transito.Id
                        });
                }

                if (transito.HoraSalida != operacion.Hora)
                {
                    novedadAplicacion.InsertarAsync(
                        new NovedadOtd
                        {
                            Operacion = operacion.Id,
                            TipoNovedad = "Cargue",
                            IdCausal = 34,
                            DescNovedad = $"La hora de salida del vuelo cierre ({operacion.Hora}) no concuerda con la hora de salida del pasajero {transito.NombrePasajero} ({transito.HoraSalida}).",
                            FechaHora = DateTime.Now,
                            IdRegistro = transito.Id
                        });
                }

                if (transito.Origen == transito.Destino)
                {
                    novedadAplicacion.InsertarAsync(
                        new NovedadOtd
                        {
                            Operacion = operacion.Id,
                            TipoNovedad = "Cargue",
                            IdCausal = 45,
                            DescNovedad = $"El origen del pasajero en tránsito ({transito.Origen}) es igual al destino del pasajero {transito.NombrePasajero} ({transito.Destino}).",
                            FechaHora = DateTime.Now,
                            IdRegistro = transito.Id
                        });
                }

                if (!transito.Origen.Equals("BAQ") && !transito.Origen.Equals("CLO") && !transito.Origen.Equals("PEI"))
                {
                    novedadAplicacion.InsertarAsync(
                        new NovedadOtd
                        {
                            Operacion = operacion.Id,
                            TipoNovedad = "Cargue",
                            IdCausal = 53,
                            DescNovedad = $"El origen del pasajero en tránsito {transito.NombrePasajero} ({transito.Origen}) no es valido.",
                            FechaHora = DateTime.Now,
                            IdRegistro = transito.Id
                        });
                }

                var pasajero = pasajeros.Where(x => x.NombrePasajero.Equals(transito.NombrePasajero)).FirstOrDefault();

                if (pasajero == null)
                {
                    novedadAplicacion.InsertarAsync(
                        new NovedadOtd
                        {
                            Operacion = operacion.Id,
                            TipoNovedad = "Cargue",
                            IdCausal = 48,
                            DescNovedad = $"El nombre del pasajero en tránsito ({transito.NombrePasajero}) no coincide con la la lista de pasajeros cargados.",
                            FechaHora = DateTime.Now,
                            IdRegistro = transito.Id
                        });

                    novedadAplicacion.InsertarAsync(
                        new NovedadOtd
                        {
                            Operacion = operacion.Id,
                            TipoNovedad = "Cargue",
                            IdCausal = 51,
                            DescNovedad = $"El pasajero en transito ({transito.NombrePasajero}) no se encuentra en la lista de pasajeros cargados.",
                            FechaHora = DateTime.Now,
                            IdRegistro = transito.Id
                        });
                }
                else
                {
                    var catTransito = transito.TTC.Equals(1) ? "TTC" : "TTL";

                    if (catTransito != pasajero.Categoria)
                    {
                        novedadAplicacion.InsertarAsync(
                            new NovedadOtd
                            {
                                Operacion = operacion.Id,
                                TipoNovedad = "Cargue",
                                IdCausal = 54,
                                DescNovedad = $"La categoría del pasajero ({pasajero.Categoria}) es diferente a la categoría cargada en tránsitos ({catTransito}).",
                                FechaHora = DateTime.Now,
                                IdRegistro = transito.Id
                            });
                    }
                }

                DateTime fechaHoraLlegada = new DateTime(
                    transito.FechaLlegada.Year,
                    transito.FechaLlegada.Month,
                    transito.FechaLlegada.Day,
                    int.Parse(transito.HoraLlegada.Substring(0, 2)),
                    int.Parse(transito.HoraLlegada.Substring(3, 2)), 0);

                DateTime fechaHoraSalida = new DateTime(
                    transito.FechaSalida.Year,
                    transito.FechaSalida.Month,
                    transito.FechaSalida.Day,
                    int.Parse(transito.HoraSalida.Substring(0, 2)),
                    int.Parse(transito.HoraSalida.Substring(3, 2)), 0);

                if (fechaHoraLlegada > fechaHoraSalida)
                {
                    novedadAplicacion.InsertarAsync(
                        new NovedadOtd
                        {
                            Operacion = operacion.Id,
                            TipoNovedad = "Cargue",
                            IdCausal = 55,
                            DescNovedad = $"La fecha de llegada del pasajero {transito.NombrePasajero} ({fechaHoraLlegada}) es mayor a la fecha de salida ({fechaHoraSalida}).",
                            FechaHora = DateTime.Now,
                            IdRegistro = transito.Id
                        });
                }

                if (operacion.Tipo.Equals("DOM"))
                {
                    if ((fechaHoraLlegada.AddHours(6)) < fechaHoraSalida)
                    {
                        novedadAplicacion.InsertarAsync(
                            new NovedadOtd
                            {
                                Operacion = operacion.Id,
                                TipoNovedad = "Cargue",
                                IdCausal = 46,
                                DescNovedad = $"No se considera tránsito ya que la hora de llegada +6 horas ({fechaHoraLlegada}) no puede ser menor de la hora de salida del pasajero {transito.NombrePasajero} ({fechaHoraSalida}).",
                                FechaHora = DateTime.Now,
                                IdRegistro = transito.Id
                            });
                    }
                }

                if (operacion.Tipo.Equals("INT"))
                {
                    if ((fechaHoraLlegada.AddHours(24)) < fechaHoraSalida)
                    {
                        novedadAplicacion.InsertarAsync(
                            new NovedadOtd
                            {
                                Operacion = operacion.Id,
                                TipoNovedad = "Cargue",
                                IdCausal = 46,
                                DescNovedad = $"No se considera tránsito ya que la hora de llegada +24 horas ({fechaHoraLlegada}) no puede ser menor de la hora de salida del pasajero {transito.NombrePasajero} ({fechaHoraSalida}).",
                                FechaHora = DateTime.Now,
                                IdRegistro = transito.Id
                            });
                    }
                }
            });        
        }

        public async Task SuspenderAsync(int id)
        {
            await operacionesVueloRepositorio.SuspenderAsync(id).ConfigureAwait(false);
        }
        public async Task<IList<OperacionVueloOtd>> ObtenerSuspencionesNotificar()
        {
            List<OperacionVueloOtd> operacionesVueloOtd = new List<OperacionVueloOtd>();
            IList<OperacionesVuelo> List = await operacionesVueloRepositorio.ObtenerSuspencionesNotificar().ConfigureAwait(false);
            foreach(var item in List)
            {
                var vuelo = mapper.Map<OperacionesVuelo, OperacionVueloOtd>(item);
                operacionesVueloOtd.Add(vuelo);
            }
            return operacionesVueloOtd;
        }
        public async Task ActualizarSuspencionAsync(int id)
        {
            await operacionesVueloRepositorio.ActualizarSuspencionAsync(id).ConfigureAwait(false);
        }
    }
}
