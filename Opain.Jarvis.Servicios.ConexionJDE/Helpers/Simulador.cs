using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Opain.Jarvis.Infraestructura.Datos.Entidades;
using Opain.Jarvis.Infraestructura.Datos.Entidades.Informes;

namespace Opain.Jarvis.Servicios.ConexionJDE.Helpers
{
    public static class Simulador
    {
        public static List<AerolineaInforme> SimularAerolineas()
        {
            List<AerolineaInforme> resultado = new List<AerolineaInforme>();

            resultado.Add(new AerolineaInforme
            {
                Valor = "ONE",
                Texto = "AVA 	AVIANCA S.A"
            });

            resultado.Add(new AerolineaInforme
            {
                Valor = "ONE",
                Texto = "AVIANCA BRASIL (OCEANAIR LIHNAS AEREAS S"
            });

            resultado.Add(new AerolineaInforme
            {
                Valor = "SAI",
                Texto = "SAI PASSENGER"
            });

            resultado.Add(new AerolineaInforme
            {
                Valor = "SAR",
                Texto = "SARPA"
            });

            return resultado;
        }

        public static List<Anexo1> SimularAnexo1()
        {
            List<Anexo1> resultado = new List<Anexo1>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo1
                {
                    CA = "DOQ70CVL_" + i.ToString(),
                    Cantidad = "DOQ70CVS_" + i.ToString(),
                    CobroCOP = "DOQ70HLP_" + i.ToString(),
                    CobroUSD = "DOQ70HSP_" + i.ToString(),
                    Factura = "DOQ70IDD_" + i.ToString(),
                    HoraIngreso = "DOQ70IDD_" + i.ToString(),
                    HoraSalida = "MAALPH_" + i.ToString(),
                    Matricula = "MAQ70OACI_" + i.ToString(),
                    NombreAerolinea = "SDAEXP_" + i.ToString(),
                    POSCobroCOP = "SDAN8_" + i.ToString(),
                    SiglaAerolinea = "SDDCT_" + i.ToString(),
                    TotalCOP = "SDDCTO_" + i.ToString(),
                    TotalUSD = "SDDOC_" + i.ToString(),
                    VueloIngreso = "SDDSC1_" + i.ToString(),
                    VueloSalida = "SDFEA_" + i.ToString(),
                    FechaIngreso = "FechaIngreso_" + i.ToString(),
                    FechaSalida = "FechaSalida_" + i.ToString(),
                    POS = "B13"
                });
            }

            return resultado;
        }

        public static List<Anexo2> SimularAnexo2()
        {
            List<Anexo2> resultado = new List<Anexo2>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo2
                {
                    CA = "DOQ70CVL_" + i.ToString(),
                    CobroCOP = "DOQ70HLP_" + i.ToString(),
                    CobroUSD = "DOQ70HSP_" + i.ToString(),
                    Factura = "DOQ70IDD_" + i.ToString(),
                    HoraIngreso = "DOQ70IDD_" + i.ToString(),
                    HoraSalida = "MAALPH_" + i.ToString(),
                    Matricula = "MAQ70OACI_" + i.ToString(),
                    NombreAerolinea = "SDAEXP_" + i.ToString(),
                    TotalCOP = "SDDCTO_" + i.ToString(),
                    TotalUSD = "SDDOC_" + i.ToString(),
                    VueloIngreso = "SDDSC1_" + i.ToString(),
                    VueloSalida = "SDFEA_" + i.ToString(),
                    FechaIngreso = "FechaIngreso_" + i.ToString(),
                    FechaSalida = "FechaSalida_" + i.ToString(),
                    OACIAerolinea = "FechaSalida_" + i.ToString(),
                    POS = "FechaSalida_" + i.ToString(),
                    TipoConexion = "FechaSalida_" + i.ToString(),
                    TotalHoras = "FechaSalida_" + i.ToString()
                });
            }

            return resultado;
        }

        public static List<Anexo3> SimularAnexo3()
        {
            List<Anexo3> resultado = new List<Anexo3>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo3
                {
                    CobroCOP = "DOQ70HLP_" + i.ToString(),
                    CobroUSD = "DOQ70HSP_" + i.ToString(),
                    Factura = "DOQ70IDD_" + i.ToString(),
                    Matricula = "MAQ70OACI_" + i.ToString(),
                    
                });
            }

            return resultado;
        }

        public static List<Anexo4> SimularAnexo4()
        {
            List<Anexo4> resultado = new List<Anexo4>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo4
                {
                    CobroCOP = "DOQ70HLP_" + i.ToString(),
                    CobroUSD = "DOQ70HSP_" + i.ToString(),
                    Factura = "DOQ70IDD_" + i.ToString(),
                    
                });
            }

            return resultado;
        }

        public static List<Anexo5> SimularAnexo5()
        {
            List<Anexo5> resultado = new List<Anexo5>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo5
                {
                    Aerolineas = "MAALPH_" + i.ToString(),
                });
            }

            return resultado;
        }

        public static List<Anexo6> SimularAnexo6()
        {
            List<Anexo6> resultado = new List<Anexo6>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo6
                {
                    CodigoVuelo = "MAALPH_" + i,
                    PaganCOP = "100",
                    PaganUSD = "0",
                    Tripulacion = "10",
                    PaganTasa = "" + i,
                    Infantes = "" + i
                });
            }

            for (int i = 21; i < 26; i++)
            {
                resultado.Add(new Anexo6
                {
                    CodigoVuelo = "MAUSDH_" + i,
                    PaganCOP = "0",
                    PaganUSD = "100",
                    Tripulacion = "10",
                    PaganTasa = "" + i,
                    Infantes = "" + i
                });
            }

            return resultado;
        }

        public static List<Anexo7A> SimularAnexo7A()
        {
            List<Anexo7A> resultado = new List<Anexo7A>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo7A
                {
                    DRDL01 = "DRDL01_" + i.ToString(),
                    DRDL02 = "DRDL02_" + i.ToString(),
                    DRKY = "DRKY_" + i.ToString(),
                    DRRT = "DRRT_" + i.ToString(),
                    DRSY = "DRSY_" + i.ToString()
                });
            }

            return resultado;
        }

        public static List<Anexo7B> SimularAnexo7B()
        {
            List<Anexo7B> resultado = new List<Anexo7B>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo7B
                {
                    CobroCOP = "200" + i,
                    CobroUSD = "100"
                });
            }

            return resultado;
        }

        public static List<Anexo8> SimularAnexo8()
        {
            List<Anexo8> resultado = new List<Anexo8>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo8
                {
                    Factura = "DRDL01_" + i.ToString(),
                    
                });
            }

            return resultado;
        }

        public static List<Anexo9> SimularAnexo9()
        {
            List<Anexo9> resultado = new List<Anexo9>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo9
                {
                    FechaSalida = "DRDL01_" + i.ToString(),
                    
                });
            }

            return resultado;
        }

        public static List<Anexo10> SimularAnexo10()
        {
            List<Anexo10> resultado = new List<Anexo10>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo10
                {
                    NIT_CEDULA = "DRDL01_" + i.ToString(),
                    
                });
            }

            return resultado;
        }

        public static List<Anexo11> SimularAnexo11()
        {
            List<Anexo11> resultado = new List<Anexo11>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo11
                {
                    CobroCOP = "DRDL01_" + i.ToString(),
                    
                });
            }

            return resultado;
        }

        public static List<Anexo12> SimularAnexo12()
        {
            List<Anexo12> resultado = new List<Anexo12>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo12
                {
                    Aerolinea = "DRDL01_" + i.ToString(),
                    
                });
            }

            return resultado;
        }

        public static List<Anexo13> SimularAnexo13()
        {
            List<Anexo13> resultado = new List<Anexo13>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo13
                {
                    Aerolinea = "DRDL01_" + i.ToString(),
                    
                });
            }

            return resultado;
        }

        public static List<Anexo14> SimularAnexo14()
        {
            List<Anexo14> resultado = new List<Anexo14>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo14
                {
                    Aerolinea = "DRDL01_" + i,
                    TarifaUSD = "100",
                    TarifaCOP = "0"
                });
            }

            for (int i = 21; i < 26; i++)
            {
                resultado.Add(new Anexo14
                {
                    Aerolinea = "DRDL01_" + i.ToString(),
                    TarifaCOP = "50",
                    TarifaUSD = "0",
                });
            }

            return resultado;
        }

        public static List<Anexo15> SimularAnexo15()
        {
            List<Anexo15> resultado = new List<Anexo15>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo15
                {
                    Cantidad = "DRDL01_" + i.ToString(),
                    
                });
            }

            return resultado;
        }

        public static List<Anexo16> SimularAnexo16()
        {
            List<Anexo16> resultado = new List<Anexo16>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo16
                {
                    Factura = "DRDL01_" + i.ToString(),
                    
                });
            }

            return resultado;
        }

        public static List<Anexo17> SimularAnexo17()
        {
            List<Anexo17> resultado = new List<Anexo17>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo17
                {
                    Factura = "DRDL01_" + i.ToString(),
                    
                });
            }

            return resultado;
        }

        public static List<Anexo19> SimularAnexo19()
        {
            List<Anexo19> resultado = new List<Anexo19>();

            for (int i = 0; i < 20; i++)
            {
                resultado.Add(new Anexo19
                {
                    TipoRadicado = "DRDL01_" + i.ToString(),
                    
                });
            }

            return resultado;
        }
    }
}
