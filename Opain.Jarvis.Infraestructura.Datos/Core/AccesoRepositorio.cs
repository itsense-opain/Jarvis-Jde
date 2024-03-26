using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;
using Opain.Jarvis.Infraestructura.Datos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Opain.Jarvis.Infraestructura.Datos.Core
{
    public class AccesoRepositorio : IAccesoRepositorio
    {
        private readonly ContextoOpain _contexto;

        public AccesoRepositorio(ContextoOpain contexto)
        {
            _contexto = contexto;
        }

        public async Task InsertarAsync(Acceso acceso)
        {
            await _contexto.AddAsync(acceso);
            await _contexto.SaveChangesAsync();
        }

        public async Task<IList<Acceso>> ObtenerTodosAsync(DateTime inicio, DateTime fin, string aerolinea)
        {
            IList<Acceso> accesosYAerolineas =new List<Acceso>();

            if (aerolinea == "0")
            {
                 accesosYAerolineas = await (from ac in _contexto.Accesos
                                                join ae in _contexto.Aerolineas on ac.IdAeroLineas equals ae.Id
                                                where ac.Fecha <= fin && ac.Fecha >= inicio 
                                                select new Acceso
                                                {
                                                    Fecha = ac.Fecha,
                                                    Grupo = ac.Grupo,
                                                    Hora = ac.Hora,
                                                    Id = ac.Id,
                                                    IdAeroLineas = ae.Id,
                                                    NombreUsuario = ac.NombreUsuario,
                                                    Rol = ac.Rol
                                                }).ToListAsync();
            }
            else 
            { 
             accesosYAerolineas = await (from ac in _contexto.Accesos
                                            join ae in _contexto.Aerolineas on ac.IdAeroLineas equals ae.Id
                                            where ac.Fecha <= fin && ac.Fecha >= inicio && ae.Codigo == aerolinea
                                            select new Acceso
                                            {
                                                Fecha=ac.Fecha,
                                                Grupo=ac.Grupo,
                                                Hora=ac.Hora,
                                                Id = ac.Id,
                                                IdAeroLineas = ae.Id,
                                                NombreUsuario=ac.NombreUsuario,
                                                Rol=ac.Rol
                                            }).ToListAsync();
            }

            //IList<Acceso> listadoAcceso;

            //foreach (var item in accesosYAerolineas)
            //{
            //    listadoAcceso.Add(new Acceso(item.Fecha,item.Grupo,item.Hora,item.Id,item.IdAeroLineas,item.NombreUsuario,item.Rol)));
            //}

            //IQueryable<Acceso> accesosYAerolineasr = (IQueryable<Acceso>)accesosYAerolineas;

            //return await _contexto.Accesos.Where(x => x.Fecha <= fin && x.Fecha >= inicio ).ToListAsync();
            return  accesosYAerolineas;
            
        }
    }
}
