using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opain.Jarvis.Servicios.ConexionJDE.Repositorio
{
    public class Simulacion
    {
        public bool TraerAerolineas { get; set; }
        public bool SimularAnexo1 { get; set; }
        public bool SimularAnexo2 { get; set; }
        public bool SimularAnexo3 { get; set; }
        public bool SimularAnexo4 { get; set; }
        public bool SimularAnexo5 { get; set; }
        public bool SimularAnexo6 { get; set; }
        public bool SimularAnexo7 { get; set; }
        public bool SimularAnexo8 { get; set; }
        public bool SimularAnexo9 { get; set; }
        public bool SimularAnexo10 { get; set; }
        public bool SimularAnexo11 { get; set; }
        public bool SimularAnexo12 { get; set; }
        public bool SimularAnexo13 { get; set; }
        public bool SimularAnexo14 { get; set; }
        public bool SimularAnexo15 { get; set; }
        public bool SimularAnexo16 { get; set; }
        public bool SimularAnexo17 { get; set; }
        public bool SimularAnexo19 { get; set; }

        public Simulacion(IConfiguration configuration)
        {
            this.TraerAerolineas = configuration.GetSection("Simulador:TraerAerolineas").Value == "true";
            this.SimularAnexo1 = configuration.GetSection("Simulador:PuenteAbordaje").Value == "true";
            this.SimularAnexo2 = configuration.GetSection("Simulador:ParqueoAeronaves").Value == "true";
            this.SimularAnexo3 = configuration.GetSection("Simulador:TasasAeroportuariasCOP").Value == "true";
            this.SimularAnexo4 = configuration.GetSection("Simulador:TasasAeroportuariasUSD").Value == "true";
            this.SimularAnexo5 = configuration.GetSection("Simulador:Mostradores").Value == "true";
            this.SimularAnexo6 = configuration.GetSection("Simulador:DetalleVuelosInterventoria").Value == "true";
            this.SimularAnexo7 = configuration.GetSection("Simulador:ResumenDetalleTasasAeroportuariasFacturadas").Value == "true";
            this.SimularAnexo8 = configuration.GetSection("Simulador:UsoCarroBombero").Value == "true";
            this.SimularAnexo9 = configuration.GetSection("Simulador:InfrasasInfantes").Value == "true";
            this.SimularAnexo10 = configuration.GetSection("Simulador:ResumenTasasAeroportuariasFacturadas").Value == "true";
            this.SimularAnexo11 = configuration.GetSection("Simulador:TasasAeroportuariasFacturadasPorAerolineas").Value == "true";
            this.SimularAnexo12 = configuration.GetSection("Simulador:SoportesExencionesTasasAeroportuarias").Value == "true";
            this.SimularAnexo13 = configuration.GetSection("Simulador:FacturacionParqueosAmplicacionLA33").Value == "true";
            this.SimularAnexo14 = configuration.GetSection("Simulador:FacturacionParqueosAmplicacionLA33100").Value == "true";
            this.SimularAnexo15 = configuration.GetSection("Simulador:FacturacionAmplicacionLA33PuentesAbordaje").Value == "true";
            this.SimularAnexo16 = configuration.GetSection("Simulador:GPU").Value == "true";
            this.SimularAnexo17 = configuration.GetSection("Simulador:GPUAerocivil").Value == "true";
            this.SimularAnexo19 = configuration.GetSection("Simulador:InformeCarnetizacion").Value == "true";
        }
    }
}