using System;
using GarbageDBTest.Domain.Interfaces;
using GarbageDBTest.Domain.Dtos.Request;
using GarbageDBTest.Domain.Entities;

namespace GarbageDBTest.Application.Services
{
    public class ApiServices : IApiServices
    {
       public int generateLocalization()
       {
           var rand = new Random();
            var geo = rand.Next(10,101); 
            return geo;
       }

       public bool ValidarNuevoEvento(Event evento)
       {
           if(string.IsNullOrEmpty(evento.Name))
                return false;
           if (evento.Date== null) 
                return false;
            if(evento.Time ==null)
                return false;
            if (evento.Colony==null)
                return false;
            return true;
       }
       public bool ValidarNuevoPOI(POICreateRequest poidto)
        {
            if (string.IsNullOrEmpty(poidto.Reason))
                return false;
            if (string.IsNullOrEmpty(poidto.Colony))
                return false;
            if (string.IsNullOrEmpty(poidto.Photo))
                return false;

            return true;
        }
    }
}