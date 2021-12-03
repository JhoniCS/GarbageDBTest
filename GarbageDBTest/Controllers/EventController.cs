using Microsoft.AspNetCore.Mvc;
using System.Collections;
using GarbageDBTest.Infraestructure.Repositories;
using GarbageDBTest.Domain.Dtos;
using GarbageDBTest.Domain.Entities;
using System.Linq;
using System;
using GarbageDBTest.Domain.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using GarbageDBTest.Application.Services;
using GarbageDBTest.Domain.Dtos.Response;
using AutoMapper;
using FluentValidation;
using GarbageDBTest.Domain.Dtos.Request;
using System.Collections.Generic;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IApiServices _services;
        private readonly IMapper _mapper;
        private readonly IValidator<EventCreateRequest> _createValidator;

        public EventController(IEventRepository repository, IHttpContextAccessor contextAccessor, IApiServices services, IMapper mapper, IValidator<EventCreateRequest> createValidator)
        {
            this._repository = repository;
            this._contextAccessor = contextAccessor;
            this._services = services;
            this._mapper = mapper;
            this._createValidator = createValidator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll() //metodo que obtiene todos los valores de la base pero solo mostrando los que definimos en el dto
        {

            var events = await _repository.GetAll();
            var respuesta = _mapper.Map<IEnumerable<Event>, IEnumerable<EventResponse>>(events);
            return Ok(respuesta);
        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)//obtenemos un resultado por id
        {

            var events = await _repository.GetById(id);
            if (events == null)
                 return NotFound("no existe ese ID");


            var respuesta = _mapper.Map<Event, EventResponse>(events);
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("Filtros")] //podemos aplicar filtros de busqueda ya que usamos el dto de filtrado, podmeos filtrar por nombre, ubicacion o sponsor
        public async Task<IActionResult> GetByFilter([FromBody] EventFilterRequest newfilter)
        {
            var eventos = await _repository.GetByFilter(newfilter);

            var respuesta = _mapper.Map<IEnumerable<Event>,IEnumerable<EventResponse>>(eventos);

            var total = respuesta.Count();

            if(total == 0)
                return Ok("No hay eventos cercanos a tu ubicacion actual");

            return Ok(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> newEvent(EventCreateRequest evento)
        {

            if (evento == null)
                return UnprocessableEntity("No estas enviando nada");

            var validate = await _createValidator.ValidateAsync(evento);

            if (!validate.IsValid)
                return UnprocessableEntity(validate.Errors.Select(x => $"{x.PropertyName}->Error: {x.ErrorMessage}"));

            var eventtosend = _mapper.Map<EventCreateRequest, Event>(evento);

            var id = 0;
            try
            {
                id = await _repository.newEvent(eventtosend);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            if (id <= 0)
                return Conflict("No se realizo el registro");

            var host = _contextAccessor.HttpContext.Request.Host.Value;
            var urlResult = $"https://{host}api/Event/{id}";
            return Created(urlResult, id);

        }
    }
}