using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GarbageDBTest.Domain.Entities;
using GarbageDBTest.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using GarbageDBTest.Infraestructure.Repositories;
using GarbageDBTest.Domain.Dtos.Request;
using System.Linq.Expressions;


namespace GarbageDBTest.Domain.Interfaces
{
    public interface IPOIRepository
    {
        Task<IEnumerable<Poi>> GetAll();
        Task<Poi> GetById(int id);
        Task<IEnumerable<Poi>> MostrarYSaltar(int mostrar, int saltar);
        Task<int> newPOI(Poi poidto);
        Task<int> confirmPOI(int id);
        Task<int> denyPOI(int id);
        Task<IEnumerable<Poi>> moreConfirmed();
        Task<IEnumerable<Poi>> searchNear(int geo);
        bool Exist(Expression<Func<Poi, bool>> expresion);
        Task<IEnumerable<Poi>> GetByFilter(POIFilterRequest filterpoi);
        int Count();
    }
}