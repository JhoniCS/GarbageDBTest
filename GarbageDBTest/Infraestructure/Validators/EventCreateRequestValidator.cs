using System;
using System.Threading.Tasks;
using FluentValidation;
using GarbageDBTest.Domain.Dtos;
using GarbageDBTest.Domain.Dtos.Request;
using GarbageDBTest.Domain.Interfaces;

namespace GarbageDBTest.Infraestructure.Validators
{
    public class EventCreateRequestValidator : AbstractValidator<EventCreateRequest>
    {
        public EventCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Informacon oblatoria");
            RuleFor(x => x.Date).NotNull().NotEmpty().WithMessage("Informcion obligatria");
            RuleFor(x => x.Time).NotNull().NotEmpty().WithMessage("Inforacion obligaoria");
            RuleFor(x => x.Colony).NotNull().NotEmpty().WithMessage("Inoracion obligatoria");
            RuleFor(x => x.RequiredPersons).NotNull().NotEmpty().WithMessage("Inormacion obligatoria");
            RuleFor(x => x.Features).NotNull().NotEmpty().WithMessage("foracion obligatoria");
            RuleFor(x => x.Reason).NotNull().NotEmpty().WithMessage("Inoracion obligatoria");
        }
    }
}