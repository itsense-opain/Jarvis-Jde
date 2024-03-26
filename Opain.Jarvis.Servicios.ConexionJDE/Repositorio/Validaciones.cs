using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Repositorio
{
    public static class Validaciones
    {
        public static bool ValidacionesDatosEntrada(ValidacionTipoAnexo tipoValidacion, FiltroBusqueda filtro)
        {
            switch (tipoValidacion)
            {
                case ValidacionTipoAnexo.Anexo1:
                case ValidacionTipoAnexo.Anexo2:
                case ValidacionTipoAnexo.Anexo4:

                    if (string.IsNullOrEmpty(filtro.facturaDesde) ||
                        string.IsNullOrEmpty(filtro.facturaHasta) ||
                        string.IsNullOrEmpty(filtro.tipo))
                        return false;
                    
                    if (!ValidarFactura(filtro.facturaDesde)) return false;
                    if (!ValidarFactura(filtro.facturaHasta)) return false;
                    if (!ValidarTipo(filtro.tipo)) return false;

                    break;
                case ValidacionTipoAnexo.Anexo3:
                case ValidacionTipoAnexo.Anexo5:

                    if (string.IsNullOrEmpty(filtro.facturaDesde) ||
                        string.IsNullOrEmpty(filtro.facturaHasta))
                        return false;

                    if (!ValidarFactura(filtro.facturaDesde)) return false;
                    if (!ValidarFactura(filtro.facturaHasta)) return false;

                    break;
                case ValidacionTipoAnexo.Anexo6:

                    if (string.IsNullOrEmpty(filtro.fechaDesde) ||
                        string.IsNullOrEmpty(filtro.fechaHasta) ||
                        string.IsNullOrEmpty(filtro.tipoVuelo) ||
                        string.IsNullOrEmpty(filtro.estado))
                        return false;

                    if (!ValidarFecha(filtro.fechaDesde)) return false;
                    if (!ValidarFecha(filtro.fechaHasta)) return false;
                    if (!ValidarTipoVuelo(filtro.tipoVuelo)) return false;
                    if (!ValidarEstado(filtro.estado)) return false;

                    break;
                case ValidacionTipoAnexo.Anexo7:

                    if (string.IsNullOrEmpty(filtro.fechaDesde) ||
                        string.IsNullOrEmpty(filtro.fechaHasta) ||
                        string.IsNullOrEmpty(filtro.tipoVuelo) ||
                        string.IsNullOrEmpty(filtro.aerolinea))
                        return false;

                    if (!ValidarFecha(filtro.fechaDesde)) return false;
                    if (!ValidarFecha(filtro.fechaHasta)) return false;
                    if (!ValidarTipoVuelo(filtro.tipoVuelo)) return false;
                    if (!ValidarAerolinea(filtro.aerolinea)) return false;

                    break;
                case ValidacionTipoAnexo.Anexo8:

                    if (string.IsNullOrEmpty(filtro.facturaDesde) ||
                        string.IsNullOrEmpty(filtro.facturaHasta))
                        return false;

                    if (!ValidarFactura(filtro.facturaDesde)) return false;
                    if (!ValidarFactura(filtro.facturaHasta)) return false;

                    break;
                case ValidacionTipoAnexo.Anexo9:

                    if (string.IsNullOrEmpty(filtro.fechaDesde) ||
                       string.IsNullOrEmpty(filtro.fechaHasta) ||
                       string.IsNullOrEmpty(filtro.estado) ||
                       string.IsNullOrEmpty(filtro.tipoVuelo))
                        return false;

                    if (!ValidarFecha(filtro.fechaDesde)) return false;
                    if (!ValidarFecha(filtro.fechaHasta)) return false;
                    if (!ValidarTipoVuelo(filtro.tipoVuelo)) return false;

                    break;
                case ValidacionTipoAnexo.Anexo10:

                    if (string.IsNullOrEmpty(filtro.fechaDesde) || 
                        string.IsNullOrEmpty(filtro.fechaHasta))
                        return false;

                    if (!ValidarFecha(filtro.fechaDesde)) return false;
                    if (!ValidarFecha(filtro.fechaHasta)) return false;

                    break;
                case ValidacionTipoAnexo.Anexo11:
                    if (string.IsNullOrEmpty(filtro.fechaDesde) ||
                       string.IsNullOrEmpty(filtro.fechaHasta) || 
                       string.IsNullOrEmpty(filtro.tipoVuelo))
                        return false;

                    if (!ValidarFecha(filtro.fechaDesde)) return false;
                    if (!ValidarFecha(filtro.fechaHasta)) return false;
                    if (!ValidarTipoVuelo(filtro.tipoVuelo)) return false;
                    break;
                case ValidacionTipoAnexo.Anexo12:
                    if (string.IsNullOrEmpty(filtro.fechaDesde) ||
                         string.IsNullOrEmpty(filtro.fechaHasta))
                        return false;
                    break;
                case ValidacionTipoAnexo.Anexo13:
                    if ( string.IsNullOrEmpty(filtro.facturaDesde) ||
                         string.IsNullOrEmpty(filtro.facturaHasta) ) 
                        return false;
                    break;
                case ValidacionTipoAnexo.Anexo14:
                    if ( string.IsNullOrEmpty(filtro.facturaDesde) ||
                         string.IsNullOrEmpty(filtro.facturaHasta))
                        return false;
                    break;
                case ValidacionTipoAnexo.Anexo15:
                    if (string.IsNullOrEmpty(filtro.facturaDesde) ||
                       string.IsNullOrEmpty(filtro.facturaHasta) ||
                       string.IsNullOrEmpty(filtro.aerolinea)
                       )
                        return false;

                    if (!ValidarFactura(filtro.facturaDesde)) return false;
                    if (!ValidarFactura(filtro.facturaHasta)) return false;
                    break;
                case ValidacionTipoAnexo.Anexo16:
                    if (!ValidarFactura(filtro.facturaDesde)) return false;
                    if (!ValidarFactura(filtro.facturaHasta)) return false;
                    if (!ValidarAerolinea(filtro.aerolinea)) return false;
                    break;
                case ValidacionTipoAnexo.Anexo17:
                    break;
                case ValidacionTipoAnexo.Anexo19:

                    if (string.IsNullOrEmpty(filtro.fechaDesde) ||
                        string.IsNullOrEmpty(filtro.fechaHasta))
                        return false;

                    if (!ValidarFecha(filtro.fechaDesde)) return false;
                    if (!ValidarFecha(filtro.fechaHasta)) return false;

                    break;
                default:
                    break;
            }

            return true;
        }

        public static bool ValidarAerolinea(string aerolinea)
        {

            return true;
        }

        public static bool ValidarFecha(string fecha)
        {


            return true;
        }

        public static bool ValidarRangoFecha(string fechaInicio, string fechaHasta)
        {


            return true;
        }

        public static bool ValidarFactura(string factura)
        {
            if (!int.TryParse(factura, out int Factura))
                return false;

            if (Factura <= 0)
                return false;

            return true;
        }

        public static bool ValidarTipo(string factura)
        {

            return true;
        }

        public static bool ValidarTipoVuelo(string factura)
        {

            return true;
        }

        public static bool ValidarEstado(string factura)
        {

            return true;
        }

        public enum ValidacionTipoAnexo
        {
            Anexo1,
            Anexo2,
            Anexo3,
            Anexo4,
            Anexo5,
            Anexo6,
            Anexo7,
            Anexo8,
            Anexo9,
            Anexo10,
            Anexo11,
            Anexo12,
            Anexo13,
            Anexo14,
            Anexo15,
            Anexo16,
            Anexo17,
            Anexo19
        }
    }
}
