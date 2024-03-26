using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opain.Jarvis.Motor.Performance
{
    public static class Contadores
    {
        static string CategoriaNombre = SX.Core.MotorBase.Configuracion.Administrador.IdentificacionServicio.NombreUnicoVisible;
        static string CategoriaDescripcion = string.Empty;

        public static void Instalar()
        {
            if (PerformanceCounterCategory.Exists(CategoriaNombre))
                PerformanceCounterCategory.Delete(CategoriaNombre);

            //-- Coleccionar los contadores que se desean asociar al agrupador
            CounterCreationDataCollection ColeccionDeContadores = new CounterCreationDataCollection();

            //-- Agregar a la colección los contador de performance que se necesita
            Array Contadores = Enum.GetValues(typeof(Tipo));
            foreach (Tipo valor in Contadores)
            {
                CounterCreationData Contador = new CounterCreationData();
                Contador.CounterName = valor.ToString();
                Contador.CounterType = PerformanceCounterType.NumberOfItems64;
                ColeccionDeContadores.Add(Contador);
            }

            //-- Finalmente crear la categoria junto a sus contadores asociados
            PerformanceCounterCategory.Create(CategoriaNombre, CategoriaDescripcion, PerformanceCounterCategoryType.SingleInstance, ColeccionDeContadores);
        }

        public static void DesInstalar()
        {
            if (PerformanceCounterCategory.Exists(CategoriaNombre))
                PerformanceCounterCategory.Delete(CategoriaNombre);
        }

        public static void Aumentar(Tipo tipoDeContador)
        {
            PerformanceCounter PerformanceCounter = new PerformanceCounter(CategoriaNombre, tipoDeContador.ToString(), false);
            PerformanceCounter.Increment();
        }

        public static void Decrementar(Tipo tipoDeContador)
        {
            PerformanceCounter PerformanceCounter = new PerformanceCounter(CategoriaNombre, tipoDeContador.ToString(), false);
            PerformanceCounter.Decrement();
        }

    }
}
