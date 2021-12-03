using System;
using System.Threading.Tasks;
using FluentValidation;
using GarbageDBTest.Domain.Dtos;
using GarbageDBTest.Domain.Dtos.Request;
using GarbageDBTest.Domain.Interfaces;

namespace GarbageDBTest.Infraestructure.Validators
{
    public class POICreateRequestValidator : AbstractValidator<POICreateRequest>
    {
        private readonly IPOIRepository _repository;

        public POICreateRequestValidator(IPOIRepository repository)
        {
            _repository = repository;
            RuleFor(x => x.Colony).NotNull().NotEmpty().WithMessage("La colonia es obligatoria");
            RuleFor(x => x.Description).NotNull().NotEmpty().Length(30, 200);
            RuleFor(x => x.Photo).NotEmpty().Must(NotExitsPhoto).WithMessage("No puedes subir la misma foto dos veces");
            RuleFor(x => x.Reason).NotNull().NotEmpty();



        }
        bool NotExitsPhoto(string photo)
        {
            return !_repository.Exist(p => p.Photo == photo);

  
        }
    }
}