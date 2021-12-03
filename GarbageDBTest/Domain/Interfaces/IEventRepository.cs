using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GarbageDBTest.Domain.Entities;
using GarbageDBTest.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using GarbageDBTest.Infraestructure.Repositories;
using System.Linq;
using GarbageDBTest.Domain.Dtos.Request;

namespace GarbageDBTest.Domain.Interfaces
{
    public interface IEventRepository
    {
        Task<IQueryable<Event>> GetAll();
        Task<Event> GetById(int id);
        Task<IEnumerable<Event>> GetByFilter(EventFilterRequest events);
        Task<int> newEvent(Event evento);
    }
}