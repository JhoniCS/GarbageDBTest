using System;
using System.Collections.Generic;
using System.Linq;
using GarbageDBTest.Domain.Entities;
using GarbageDBTest.Infraestructure.Data;
using GarbageDBTest.Domain.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GarbageDBTest.Domain.Dtos.Request;
using AutoMapper;

namespace GarbageDBTest.Infraestructure.Repositories
{
    public class EventSqlRepository : IEventRepository
    {


        private readonly GarbageDBContext _eventcontext;
        private readonly IMapper _mapper;
        private readonly IApiServices _services;

        public EventSqlRepository(GarbageDBContext eventcontext, IMapper mapper, IApiServices services)
        {
            _eventcontext = eventcontext;
            this._mapper = mapper;
            this._services = services;
        }

        public async Task<IQueryable<Event>> GetAll()
        {
            // origen, Método, iterador
            var query = await _eventcontext.Events.AsQueryable<Event>().AsNoTracking().ToListAsync();
            return query.AsQueryable();
        }

        public async Task<Event> GetById(int id)
        {
            var query = await _eventcontext.Events.FindAsync(id);

            
            return query;
        }

        public async Task<IEnumerable<Event>> GetByFilter(EventFilterRequest events)
        {
            var myubication = _services.generateLocalization();

            var query = _eventcontext.Events.Select(x => x).Where(p => p.Geoubication >= myubication - 10 && p.Geoubication <= myubication + 10);

            if (!string.IsNullOrEmpty(events.Colony))
                query = query.Where(x => x.Colony == events.Colony);

            if (!string.IsNullOrEmpty(events.Reason))
                query = query.Where(x => x.Reason == events.Reason);


            return await query.ToListAsync();
        }
        public async Task<int> newEvent(Event evento)
        {
            evento.Geoubication = _services.generateLocalization();
            try
            {
                await _eventcontext.AddAsync(evento);
                var rows = await _eventcontext.SaveChangesAsync();

                if (rows != 1)
                    throw new Exception("Ocurrio un falla al intentar guardar el registro bro");

                return evento.Id;
            }
            catch (DbUpdateException)
            {
                throw new Exception("Fallo al intentar acceder al repositorio");
            }
        }
    }
}