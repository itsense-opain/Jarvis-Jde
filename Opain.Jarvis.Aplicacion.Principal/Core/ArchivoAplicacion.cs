using AutoMapper;
using Opain.Jarvis.Aplicacion;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Opain.Jarvis.Aplicacion
{
    public class ArchivoAplicacion : IArchivoAplicacion
    {
        private readonly IArchivoRepositorio archivoRepositorio;
        private readonly IMapper mapper;
        public ArchivoAplicacion(IMapper map, IArchivoRepositorio archivo)
        {
            mapper = map;
            archivoRepositorio = archivo;
        }

        public async Task ActualizarAsync(ArchivoOtd archivo)
        {
            var archivoMapeo = mapper.Map<ArchivoOtd, Archivo>(archivo);

            await archivoRepositorio.ActualizarAsync(archivoMapeo);
        }

        public async Task EliminarAsync(int id)
        {
            await archivoRepositorio.EliminarAsync(id);
        }

        public async Task InsertarAsync(ArchivoOtd archivo)
        {
            var archivoMapeo = mapper.Map<ArchivoOtd, Archivo>(archivo);
            await archivoRepositorio.InsertarAsync(archivoMapeo);
        }

        public async Task  InsertarMasivoAsync(IList<ArchivoOtd> archivo)
        {
            IList<Archivo> archivosOdt = new List<Archivo>();

            foreach (var item in archivo)
            {
                var a = mapper.Map<ArchivoOtd, Archivo>(item);
                archivosOdt.Add(a);
            }

             await archivoRepositorio.InsertarMasivoAsync(archivosOdt);
        }

        public async Task<ArchivoOtd> ObtenerAsync(int id)
        {
            var archivo = await archivoRepositorio.ObtenerAsync(id);
            var archivoOtd = mapper.Map<Archivo, ArchivoOtd>(archivo);

            return archivoOtd;
        }

        public async Task<IList<ArchivoOtd>> ObtenerPorOperacionAsync(int idOperacion)
        {
            IList<ArchivoOtd> archivosOtd = new List<ArchivoOtd>();
            IList<Archivo> archivos = await archivoRepositorio.ObtenerPorOperacionAsync(idOperacion);

            foreach (var item in archivos)
            {
                var archivoOtd = mapper.Map<Archivo, ArchivoOtd>(item);
                archivosOtd.Add(archivoOtd);
            }

            return archivosOtd;
        }
    }
}
