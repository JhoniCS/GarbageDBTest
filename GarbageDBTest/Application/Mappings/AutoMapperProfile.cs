using System;
using AutoMapper;
using GarbageDBTest.Domain.Entities;
using GarbageDBTest.Domain.Dtos.Response;
using GarbageDBTest.Domain.Dtos.Request;

namespace GarbageDBTest.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Poi, POIResponse>();
            
            CreateMap<POICreateRequest, Poi>().
            BeforeMap((s, d) => d.Date = DateTime.Now).
            ForMember(x=> x.IsDeleted, opt=>opt.MapFrom(o=>false)).
            ForMember(x=> x.Confirmations, opt=>opt.MapFrom(o=>0)). 
            ForMember(x=> x.Negations, opt=>opt.MapFrom(o=>0)); 

            CreateMap<Event, EventResponse>();

            CreateMap<EventCreateRequest, Event>().
            BeforeMap((s, d) => d.CreateDate = DateTime.Now).
            ForMember(x=> x.IsDeleted, opt=>opt.MapFrom(o=>false));
            
        }

        
    }
}