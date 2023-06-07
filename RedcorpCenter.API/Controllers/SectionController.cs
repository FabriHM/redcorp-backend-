﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RedcorpCenter.API.Request;
using RedcorpCenter.API.Response;
using RedcorpCenter.Domain;
using RedcorpCenter.Infraestructure;
using RedcorpCenter.Infraestructure.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RedcorpCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private ISectionInfraestructure _sectionInfraestructure;
        private ISectionDomain _sectionDomain;
        private IMapper _mapper;

        public SectionController(ISectionInfraestructure sectionInfraestructure, ISectionDomain sectionDomain, IMapper mapper)
        {
            _sectionInfraestructure = sectionInfraestructure;
            _sectionDomain = sectionDomain;
            _mapper = mapper;
        }



        // GET: api/<SectionController>
        [HttpGet]
        public async Task<List<SectionResponse>> GetAsync()
        {
            var sections = await _sectionInfraestructure.GetAllAsync();
            return _mapper.Map<List<Section>, List<SectionResponse>>(sections);
            
        }

        // GET api/<SectionController>/5
        [HttpGet("{id}",Name = "GetSection")]
        public SectionResponse Get(int id)
        {
            Section section = _sectionInfraestructure.GetById(id);

            /*SectionResponse sectionResponse = new SectionResponse()
            {
                Id = section.Id,
                Section_Name = section.Section_Name,
                Description = section.Description,
                
            };*/
            var sectionResponse = _mapper.Map<Section,SectionResponse>(section);

            return sectionResponse;
        }

        // POST api/<SectionController>
        [HttpPost]
        public void Post([FromBody] SectionResquest sectionRequest)
        {
            if (ModelState.IsValid)
            {
                /*Section section = new Section()
                {
                    Section_Name = sectionRequest.Section_Name,
                    Description = sectionRequest.Description,
                    IsActive = true // Establece el valor por defecto para IsActive
                };*/
                var section = _mapper.Map<SectionResquest,Section>(sectionRequest);

                _sectionDomain.Save(section);
            }
            else
            {
                StatusCode(400);
            }

        }

        // PUT api/<SectionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] SectionResquest sectionRequest)
        {
            _sectionDomain.update(id, sectionRequest.Section_Name, sectionRequest.Description);
        }

        // DELETE api/<SectionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _sectionDomain.delete(id);
        }
    }
}
