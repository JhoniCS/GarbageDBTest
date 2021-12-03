using System;
using System.Collections.Generic;
using System.Linq;
using GarbageDBTest.Domain.Entities;
using GarbageDBTest.Infraestructure.Data;
using GarbageDBTest.Domain.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GarbageDBTest.Domain.Dtos;
using AutoMapper;
using GarbageDBTest.Domain.Dtos.Request;
using System.Linq.Expressions;

namespace GarbageDBTest.Infraestructure.Repositories
{
    public class PoiSqlRepository : IPOIRepository
    {
        private readonly GarbageDBContext _poicontext;
        private readonly IMapper _mapper;
        private readonly IApiServices _services;

        public PoiSqlRepository(GarbageDBContext poicontext, IMapper mapper, IApiServices services)
        {
            _poicontext = poicontext;
            this._mapper = mapper;
            this._services = services;
        }

        public async Task<IEnumerable<Poi>> GetAll()
        {
            // origen, Método, iterador
            var query = _poicontext.Pois.Select(e => e);
            return await query.ToListAsync();
        }

        public async Task<Poi> GetById(int id)
        {
            var query = await _poicontext.Pois.FindAsync(id);

            return query;
        }

        public async Task<IEnumerable<Poi>> MostrarYSaltar(int mostrar, int saltar)
        {
            var show = mostrar;
            var skip = saltar;

            var query = _poicontext.Pois.Select(p => p).Skip(skip).Take(show);

            return await query.ToListAsync();
        }
        public async Task<int> newPOI(Poi poidto)
        {

            poidto.Geoubication = _services.generateLocalization();
            try
            {
                await _poicontext.AddAsync(poidto);
                var rows = await _poicontext.SaveChangesAsync();

                if (rows != 1)
                    throw new Exception("Ocurrio un falla al intentar guardar el evento nuevo");

                return poidto.Id;
            }
            catch (DbUpdateException)
            {
                throw new Exception("Fallo al intentar acceder al repositorio");
            }

        }

        public async Task<int> confirmPOI(int id)
        {
            var poitoupdate = await GetById(id);

            poitoupdate.ModifiedDate = DateTime.Now;
            poitoupdate.Confirmations = poitoupdate.Confirmations + 1;

            try
            {

                var rows = await _poicontext.SaveChangesAsync();

                if (rows != 1)
                    throw new Exception("Ocurrio un falla al intentar guardar el evento nuevo");

                return poitoupdate.Id;
            }
            catch (DbUpdateException)
            {
                throw new Exception("Fallo al intentar acceder al repositorio");
            }
        }

        public async Task<int> denyPOI(int id)
        {
            var poitoupdate = await GetById(id);

            poitoupdate.ModifiedDate = DateTime.Now;
            if(poitoupdate.Negations==null)
                poitoupdate.Negations=0;
            poitoupdate.Negations = poitoupdate.Negations + 1;

            try
            {

                var rows = await _poicontext.SaveChangesAsync();

                if (rows != 1)
                    throw new Exception("Ocurrio un falla al intentar modificar el registro");

                return poitoupdate.Id;
            }
            catch (DbUpdateException)
            {
                throw new Exception("Fallo al intentar acceder al repositorio");
            }
        }

        public async Task<IEnumerable<Poi>> GetByFilter(POIFilterRequest filterpoi)
        {    
            var query = _poicontext.Pois.Select(x => x);

            if (filterpoi.Min!=0)
                query = query.Where(x => x.Confirmations >= filterpoi.Min);

            if (filterpoi.Max!=0)
                query = query.Where(x => x.Confirmations <= filterpoi.Max);
            
            if (!string.IsNullOrEmpty(filterpoi.Colony))
                query = query.Where(x => x.Colony == filterpoi.Colony);

            if (filterpoi.Date!= null)
                query = query.Where(x => x.Date== filterpoi.Date);


            return await query.ToListAsync();
        }


        public async Task<IEnumerable<Poi>> moreConfirmed()
        {
            var query = _poicontext.Pois.OrderByDescending(p => p.Confirmations).Take(20);
            //var response = query.Select(x=>objectToResponse(x));
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Poi>> searchNear(int geo)
        {

            var query = _poicontext.Pois.OrderByDescending(p => p.Geoubication).Where(p => p.Geoubication >= geo - 5 && p.Geoubication <= geo + 5);

            return await query.ToListAsync();
        }

        public bool Exist(Expression<Func<Poi, bool>> expresion)
        {
            return _poicontext.Pois.Any(expresion);
        }

        public int Count()
        {
            var result = _poicontext.Pois.Count();

            return result;
        }
    }
}