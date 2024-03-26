using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Opain.Jarvis.Infraestructura.Datos.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Opain.Jarvis.Infraestructura.Datos.Entidades.Informes;

namespace Opain.Jarvis.Servicios.ConexionJDE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class InformesController : Controller
    {
        #region Constructor y variables
        private readonly Repositorio.EjecutorInforme EjecutorInforme;
        private readonly Repositorio.Simulacion Simulacion;
        private readonly ILogger<InformesController> _logger;

        public InformesController(  Repositorio.EjecutorInforme ejecutorInforme,
                                    Repositorio.Simulacion simulacion, ILogger<InformesController> logger)
        {
            this.EjecutorInforme = ejecutorInforme;
            this.Simulacion = simulacion;
            _logger = logger;
        }
        #endregion

        [HttpGet]
        [ProducesResponseType(typeof(IList<AerolineaInforme>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<AerolineaInforme>>> TraerAerolineas()
        {
            List<AerolineaInforme> resultado;

            if (this.Simulacion.TraerAerolineas)
            {
                resultado = Helpers.Simulador.SimularAerolineas();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.TraerAerolineas();
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<AerolineaInforme>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<AerolineaInforme>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<AerolineaInforme>>> TraerAerolineas2()
        {
            List<AerolineaInforme> resultado;

            if (this.Simulacion.TraerAerolineas)
            {
                resultado = Helpers.Simulador.SimularAerolineas();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.TraerAerolineas2();
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<AerolineaInforme>();

            return Ok(resultado);
        }
        [HttpGet]
        [ProducesResponseType(typeof(IList<Anexo1>), StatusCodes.Status200OK)]
        
        public async Task<ActionResult<IList<Anexo1>>> Anexo1(string aerolinea, string fechaDesde, string fechaHasta, string facturaDesde, string facturaHasta, string tipo,bool Descarga)
        {
            Repositorio.FiltroBusqueda Filtro = new Repositorio.FiltroBusqueda
            {                
                facturaDesde = facturaDesde,
                facturaHasta = facturaHasta,
                tipo = tipo,
                aerolinea = aerolinea,
                fechaDesde = fechaDesde,
                fechaHasta=fechaHasta,
                Descarga = Descarga                
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo1, Filtro))
                return NoContent();

            List<Anexo1> resultado;

            if (this.Simulacion.SimularAnexo1)
            {
                resultado = Helpers.Simulador.SimularAnexo1();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe1(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo1>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo2>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo2>>> Anexo2(string aerolinea, string fechaDesde, string fechaHasta, string facturaDesde, string facturaHasta, string tipo,bool Descarga)
        {
            Repositorio.FiltroBusqueda Filtro = new Repositorio.FiltroBusqueda
            {                
                facturaDesde = facturaDesde,
                facturaHasta = facturaHasta,
                tipo = tipo,
                aerolinea = aerolinea,
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                Descarga = Descarga
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo2, Filtro))
                return NoContent();

            List<Anexo2> resultado;

            if (this.Simulacion.SimularAnexo2)
            {
                resultado = Helpers.Simulador.SimularAnexo2();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe2(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo2>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo3>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo3>>> Anexo3(string aerolinea, string fechaDesde, string fechaHasta, string facturaDesde, string facturaHasta, string tipo,bool Descarga)
        {
            Repositorio.FiltroBusqueda Filtro = new Repositorio.FiltroBusqueda
            {
                facturaDesde = facturaDesde,
                facturaHasta = facturaHasta,
                tipo = tipo,
                aerolinea = aerolinea,
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                Descarga = Descarga
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo3, Filtro))
                return NoContent();

            List<Anexo3> resultado;

            if (this.Simulacion.SimularAnexo3)
            {
                resultado = Helpers.Simulador.SimularAnexo3();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe3(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo3>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo4>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo4>>> Anexo4(string aerolinea, string fechaDesde, string fechaHasta, string facturaDesde, string facturaHasta, string tipo, bool Descarga)
        {
            var Filtro = new Repositorio.FiltroBusqueda
            {
                facturaDesde = facturaDesde,
                facturaHasta = facturaHasta,
                aerolinea= aerolinea,
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                Descarga = Descarga
            };

            List<Anexo4> resultado;

            if (this.Simulacion.SimularAnexo4)
            {
                resultado = Helpers.Simulador.SimularAnexo4();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe4(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo4>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo5>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo5>>> Anexo5(string aerolinea, string fechaDesde, string fechaHasta, string facturaDesde, string facturaHasta, bool Descarga)
        {
            var Filtro = new Repositorio.FiltroBusqueda
            {
                facturaDesde = facturaDesde,
                facturaHasta = facturaHasta,
                aerolinea = aerolinea,
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                Descarga=Descarga
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo5, Filtro))
                return NoContent();

            List<Anexo5> resultado;

            if (this.Simulacion.SimularAnexo5)
            {
                resultado = Helpers.Simulador.SimularAnexo5();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe5(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo5>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo6>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo6>>> Anexo6(string fechaDesde, string fechaHasta, string tipoVuelo, string estado, string aerolinea, bool Descarga)
        {
            var Filtro = new Repositorio.FiltroBusqueda
            {
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                tipoVuelo = tipoVuelo,
                estado = estado,
                aerolinea= aerolinea,
                Descarga = Descarga
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo6, Filtro))
                return NoContent();

            List<Anexo6> resultado;

            if (this.Simulacion.SimularAnexo6)
            {
                resultado = Helpers.Simulador.SimularAnexo6();
                return Ok(FiltrarTipoMonedaAnexo6(resultado, tipoVuelo));
            }

            var Respuesta = await EjecutorInforme.Informe6(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo6>();

            return Ok(FiltrarTipoMonedaAnexo6(resultado, tipoVuelo));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo7A>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo7A>>> Anexo7A()
        {
            List<Anexo7A> resultado;

            if (this.Simulacion.SimularAnexo7)
            {
                resultado = Helpers.Simulador.SimularAnexo7A();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe7A();
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo7A>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo7B>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo7B>>> Anexo7B(string fechaDesde, string fechaHasta, string tipoVuelo, string aerolinea,  bool Descarga)
        {
            Repositorio.FiltroBusqueda Filtro = new Repositorio.FiltroBusqueda
            {
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                tipoVuelo = tipoVuelo,
                aerolinea = aerolinea,
                Descarga = Descarga
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo7, Filtro))
                return NoContent();

            List<Anexo7B> resultado;

            if (this.Simulacion.SimularAnexo7)
            {
                resultado = Helpers.Simulador.SimularAnexo7B();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe7B(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo7B>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo8>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo8>>> Anexo8(string fechaDesde, string fechaHasta, string facturaDesde, string facturaHasta, string aerolinea, bool Descarga)
        {
            var Filtro = new Repositorio.FiltroBusqueda
            {
                facturaDesde = facturaDesde,
                facturaHasta = facturaHasta,
                aerolinea = aerolinea,
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                Descarga = Descarga
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo8, Filtro))
                return NoContent();

            List<Anexo8> resultado;

            if (this.Simulacion.SimularAnexo7)
            {
                resultado = Helpers.Simulador.SimularAnexo8();
                return Ok(resultado);
            }

            var Respuesta = await EjecutorInforme.Informe8(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo8>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo9>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo9>>> Anexo9(string fechaDesde, string fechaHasta, string tipoVuelo, string aerolinea, string estado)
        {
            var Filtro = new Repositorio.FiltroBusqueda
            {
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                tipoVuelo = tipoVuelo,
                estado = estado,
                aerolinea = aerolinea
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo9, Filtro))
                return NoContent();

            List<Anexo9> resultado;

            if (this.Simulacion.SimularAnexo9)
            {
                resultado = Helpers.Simulador.SimularAnexo9();
                return Ok(resultado);
            }

            var Respuesta = await EjecutorInforme.Informe9(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo9>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo10>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo10>>> Anexo10(string fechaDesde, string fechaHasta, string tipoVuelo,string aerolinea)
        {
            Repositorio.FiltroBusqueda Filtro = new Repositorio.FiltroBusqueda
            {
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                tipo = tipoVuelo,
                aerolinea=aerolinea
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo10, Filtro))
                return NoContent();

            List<Anexo10> resultado;

            if (this.Simulacion.SimularAnexo10)
            {
                resultado = Helpers.Simulador.SimularAnexo10();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe10(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo10>();

            return Ok(resultado);
        }

        #region por completar filtros
        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo11>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo11>>> Anexo11(string fechaDesde, string fechaHasta, string tipoVuelo,bool Descarga)
        {
            Repositorio.FiltroBusqueda Filtro = new Repositorio.FiltroBusqueda
            {
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                tipoVuelo = tipoVuelo,
                Descarga = Descarga
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo11, Filtro))
                return NoContent();

            List<Anexo11> resultado;

            if (this.Simulacion.SimularAnexo11)
            {
                resultado = Helpers.Simulador.SimularAnexo11();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe11(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo11>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo12>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo12>>> Anexo12(string fechaDesde, string fechaHasta,string aerolinea, bool Descarga)
        {
            Repositorio.FiltroBusqueda Filtro = new Repositorio.FiltroBusqueda
            {
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta ,
                aerolinea = aerolinea,
                Descarga = Descarga
            };


            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo12, Filtro))
                return NoContent();

            List<Anexo12> resultado;

            if (this.Simulacion.SimularAnexo12)
            {
                resultado = Helpers.Simulador.SimularAnexo12();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe12(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo12>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo13>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo13>>> Anexo13(string facturaDesde, string facturaHasta, string tipo,string aerolinea, bool Descarga,string fechaDesde, string fechahasta)
        {
            Repositorio.FiltroBusqueda Filtro = new Repositorio.FiltroBusqueda
            {
                facturaDesde = facturaDesde,
                facturaHasta= facturaHasta,
                tipo = tipo,
                aerolinea = aerolinea,
                Descarga = Descarga,
                fechaDesde = fechaDesde,
                fechaHasta = fechahasta
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo13, Filtro))
                return NoContent();

            List<Anexo13> resultado;

            if (this.Simulacion.SimularAnexo13)
            {
                resultado = Helpers.Simulador.SimularAnexo13();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe13(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo13>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo14>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo14>>> Anexo14(string aerolinea, string facturaDesde, string facturaHasta, string tipo, bool Descarga, string fechaDesde, string fechahasta)
        {
            var Filtro = new Repositorio.FiltroBusqueda
            {
                aerolinea = aerolinea,
                facturaDesde = facturaDesde,
                facturaHasta = facturaHasta,
                tipo = tipo,
                Descarga= Descarga,
                 fechaDesde = fechaDesde,
                fechaHasta = fechahasta
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo14, Filtro))
                return NoContent();

            List<Anexo14> resultado;

            if (this.Simulacion.SimularAnexo14)
            {
                resultado = Helpers.Simulador.SimularAnexo14();
                return Ok(FiltrarTipoMonedaAnexo14(resultado, tipo));
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe14(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo14>();

            return Ok(FiltrarTipoMonedaAnexo14(resultado, tipo));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo15>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo15>>> Anexo15(string fechaDesde, string fechaHasta, string tipo, string aerolinea, string facturaDesde, string facturaHasta)
        {
            Repositorio.FiltroBusqueda Filtro = new Repositorio.FiltroBusqueda
            {
                facturaDesde = facturaDesde,
                facturaHasta = facturaHasta,
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                aerolinea= aerolinea
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo15, Filtro))
                return NoContent();

            List<Anexo15> resultado;

            if (this.Simulacion.SimularAnexo15)
            {
                resultado = Helpers.Simulador.SimularAnexo15();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe15(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo15>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo16>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo16>>> Anexo16(string fechaDesde, string fechaHasta, string tipo, string aerolinea, string facturaDesde, string facturaHasta, bool Descarga)
        {
            Repositorio.FiltroBusqueda Filtro = new Repositorio.FiltroBusqueda
            {
                facturaDesde = facturaDesde,
                facturaHasta = facturaHasta, 
                tipo = tipo,
                aerolinea = aerolinea,
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                Descarga = Descarga
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo16, Filtro))
                return NoContent();

            List<Anexo16> resultado;

            if (this.Simulacion.SimularAnexo16)
            {
                resultado = Helpers.Simulador.SimularAnexo16();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe16(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo16>();

            return Ok(resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo17>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo17>>> Anexo17(string fechaDesde, string fechaHasta, string tipo, string aerolinea, string facturaDesde, string facturaHasta)
        {
            Repositorio.FiltroBusqueda Filtro = new Repositorio.FiltroBusqueda
            {
                facturaDesde = facturaDesde,
                facturaHasta = facturaHasta,
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                tipo = tipo,
                aerolinea = aerolinea
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo17, Filtro))
                return NoContent();

            List<Anexo17> resultado;

            if (this.Simulacion.SimularAnexo17)
            {
                resultado = Helpers.Simulador.SimularAnexo17();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe17(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo17>();

            return Ok(resultado);
        }
        #endregion

        [HttpGet]
        [ProducesResponseType(typeof(List<Anexo19>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Anexo19>>> Anexo19(string fechaDesde, string fechaHasta,bool Descarga)
        {
            Repositorio.FiltroBusqueda Filtro = new Repositorio.FiltroBusqueda
            {
                fechaDesde = fechaDesde,
                fechaHasta = fechaHasta,
                Descarga= Descarga
            };

            if (!Repositorio.Validaciones.ValidacionesDatosEntrada(Repositorio.Validaciones.ValidacionTipoAnexo.Anexo19, Filtro))
                return NoContent();

            List<Anexo19> resultado;

            if (this.Simulacion.SimularAnexo19)
            {
                resultado = Helpers.Simulador.SimularAnexo19();
                return Ok(resultado);
            }

            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.Informe19(Filtro);
            if (!Respuesta.Exito)
            {
                RegistrarLog(Respuesta.MotivoNoExito);
                return NotFound();
            }

            resultado = Respuesta.Resultado<Anexo19>();

            return Ok(resultado);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IList<Infraestructura.Datos.EntidadesIntegracion.DatosExentos>>> TraerExtentosJDE(string numerovuelo, string dia, string mes, string anio)
        {
            DateTime fecha = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), Convert.ToInt32(dia));
            List<Infraestructura.Datos.EntidadesIntegracion.DatosExentos> resultado = null;
            Repositorio.RespuestaGenerica Respuesta = await EjecutorInforme.TraerExentos(numerovuelo,fecha);
            if (!Respuesta.Exito)
            {
                return NotFound();
            }
            resultado = Respuesta.Resultado<Infraestructura.Datos.EntidadesIntegracion.DatosExentos>();
            return Ok(resultado);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> TraerAeroJDE()
        {
            bool Respuesta = await EjecutorInforme.TraerAeroJDE();

            if (!Respuesta)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> TraerVuelosJDE()
        {
            bool Respuesta = await EjecutorInforme.TraerVuelosJDE();

            if (!Respuesta)
            {
                return NotFound();
            }

            return Ok();
        }

        //

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> UpdVueloValidJDE()
        {
            _logger.LogInformation("Test");
            bool Respuesta = await EjecutorInforme.UpdVueloValidJDE();

            if (!Respuesta)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> UpdVueloValidJDEVuelo(string vuelo)
        {
            _logger.LogInformation("Test");
            bool Respuesta = await EjecutorInforme.UpdVueloValidJDEVuelo(vuelo);

            if (!Respuesta)
            {
                return NotFound();
            }

            return Ok();
        }

        private void RegistrarLog(string ex)
        {
            Console.WriteLine(ex);
        }

        private static List<Anexo14> FiltrarTipoMonedaAnexo14(List<Anexo14> resultado, string tipo)
        {
            //if (tipo.Equals("USD"))
            //{
            //    resultado = resultado.Where(x => !string.IsNullOrEmpty(x.TarifaUSD) && x.TarifaUSD != "0").ToList();
            //}
            //else if (tipo.Equals("COP"))
            //{
            //    resultado = resultado.Where(x => !string.IsNullOrEmpty(x.TarifaCOP) && x.TarifaCOP != "0").ToList();
            //}

            return resultado;
        }

        private static List<Anexo6> FiltrarTipoMonedaAnexo6(List<Anexo6> resultado, string tipo)
        {
            List<Anexo6> listafiltroTipoMonedaAnexo6 = new List<Anexo6>();

            //if (tipo.Equals("INT"))
            //{
            //    listafiltroTipoMonedaAnexo6 = resultado.Where(x => !string.IsNullOrEmpty(x.PaganUSD) && x.PaganUSD != "0").ToList();
                
            //}
            //else if (tipo.Equals("NAL"))
            //{
            //    listafiltroTipoMonedaAnexo6 = resultado.Where(x => !string.IsNullOrEmpty(x.PaganCOP) && x.PaganCOP != "0").ToList();
            //}

            return resultado;
        }
    }
}