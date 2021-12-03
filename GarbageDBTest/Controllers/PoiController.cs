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
using AutoMapper;
using System.Collections.Generic;
using GarbageDBTest.Domain.Dtos.Response;
using GarbageDBTest.Domain.Dtos.Request;
using FluentValidation;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PoiController : ControllerBase
    {
        private readonly IPOIRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IApiServices _services;
        private readonly IMapper _mapper;
        private readonly IValidator<POICreateRequest> _createValidator;

        public PoiController(IPOIRepository repository, IHttpContextAccessor contextAccessor, IApiServices services, IMapper mapper, IValidator<POICreateRequest> createValidator)
        {
            this._repository = repository;
            this._contextAccessor = contextAccessor;
            this._services = services;
            this._mapper = mapper;
            this._createValidator = createValidator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll() //metodo que obtiene todos los pois de la base mostando TODOS sus elementos ya no le aplicamos ningun DTO
        {

            var pois = await _repository.GetAll();
            //var respuesta = persons.Select(x=>x);
            var respuesta = _mapper.Map<IEnumerable<Poi>, IEnumerable<POIResponse>>(pois);
            return Ok(respuesta);

        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)//obtenemos un resultado por id
        {

            var pois = await _repository.GetById(id);

            if (pois == null)
                 return NotFound("no existe ese ID");

            var respuesta = _mapper.Map<Poi, POIResponse>(pois);
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("{mostrar}.{saltar}")] //con este metodo solo mostramos cierta cantidad de elementos despues de saltar otra cantidad dada. Puede sernos util al momentos de paginar resultados.
        public async Task<IActionResult> MostrarYSaltar(int mostrar, int saltar)
        {

            var listita = await _repository.MostrarYSaltar(mostrar, saltar);
            return Ok(listita);
        }

        [HttpPost]
        public async Task<IActionResult> newPOI(POICreateRequest poidto)
        {
            if (poidto == null)
                return UnprocessableEntity("No estas enviando nada");

            //var validate = _services.ValidarNuevoPOI(poidto);
            var validate = await _createValidator.ValidateAsync(poidto);

            if (!validate.IsValid)
                return UnprocessableEntity(validate.Errors.Select(x => $"{x.PropertyName}->Error: {x.ErrorMessage}"));


            var poidtotosend = _mapper.Map<POICreateRequest, Poi>(poidto);
            var id = 0;
            try
            {
                id = await _repository.newPOI(poidtotosend);
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


        [HttpPut]
        [Route("confirm/{id:int}")]
        public async Task<IActionResult> confirmPOI(int id)
        {

            if (id <= 0)
                return NotFound();
            var poi = await _repository.GetById(id);
            if (poi == null)
                return NotFound();
            try
            {
                id = await _repository.confirmPOI(poi.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            if (id <= 0)
                return Conflict("Gracias por confirmar");

            var host = _contextAccessor.HttpContext.Request.Host.Value;
            var urlResult = $"https://{host}api/Event/{id}";
            return Ok("Actualizada Correctamente");

        }
        [HttpPut]
        [Route("deny/{id:int}")]
        public async Task<IActionResult> denyPOI(int id)
        {

            if (id <= 0)
                return NotFound();
            var poi = await _repository.GetById(id);
            if (poi == null)
                return NotFound();
            try
            {
                id = await _repository.denyPOI(poi.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            if (id <= 0)
                return Conflict("votorecibido");

            var host = _contextAccessor.HttpContext.Request.Host.Value;
            var urlResult = $"https://{host}api/Event/{id}";
            return Ok("Actualizada Correctamente");

        }
        [HttpGet]
        [Route("worst")]
        public async Task<IActionResult> moreConfirmed()
        {
            var mConfirmed = await _repository.moreConfirmed();
            var respuesta = _mapper.Map<IEnumerable<Poi>, IEnumerable<POIResponse>>(mConfirmed);
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("near")]
        public async Task<IActionResult> searchNear()
        {
            var geo = _services.generateLocalization(); ;
            var near = await _repository.searchNear(geo);
            var respuesta = _mapper.Map<IEnumerable<Poi>, IEnumerable<POIResponse>>(near);
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("Filtros")] //podemos aplicar filtros de busqueda ya que usamos el dto de filtrado, podmeos filtrar por nombre, ubicacion o sponsor
        public async Task<IActionResult> GetByFilter([FromBody] POIFilterRequest newfilter)
        {
            var pois = await _repository.GetByFilter(newfilter);
            var respuesta = _mapper.Map<IEnumerable<Poi>,IEnumerable<POIResponse>>(pois);

            return Ok(respuesta);
        }

        [HttpGet]
        [Route("Contar")] 
        public IActionResult Count()
        {
            
            var pois =  _repository.Count();

            return Ok($"POIs registrados {pois}");
        }


        



    }
}