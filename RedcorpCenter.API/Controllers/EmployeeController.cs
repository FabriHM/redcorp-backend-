﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RedcorpCenter.API.Request;
using RedcorpCenter.API.Response;
using RedcorpCenter.Domain;
using RedcorpCenter.Infraestructure.Models;
using RedcorpCenter.Infraestructure;
using Newtonsoft.Json;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Configuration;
using RedcorpCenter.API.Filter;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RedcorpCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeInfraestructure _employeeInfraestructure;
        private IEmployeeDomain _employeeDomain;
        private IMapper _mapper;
        public IConfiguration _configuration;
        public EmployeeController(IEmployeeInfraestructure employeeInfraestructure, IEmployeeDomain employeedomain, IMapper mapper, IConfiguration configuration)
        {
            _employeeInfraestructure = employeeInfraestructure;
            _employeeDomain = employeedomain;
            _mapper = mapper;
            _configuration = configuration;

        }


        [Authorize("user,admin")]   
        // GET: api/Tutorial
        [HttpGet]
        public async Task<List<EmployeeResponse>> GetAsync()
        {

            var employees = await _employeeInfraestructure.GetAllAsync();

            return _mapper.Map<List<Employee>, List<EmployeeResponse>>(employees);
        }
        [Authorize("user,admin")]
        // GET: api/Tutorial/5
        [HttpGet("{id}", Name = "Get")]
        public EmployeeResponse Get(int id)
        {
            Employee employee = _employeeInfraestructure.GetById(id);

            

            var employeeResponse = _mapper.Map<Employee, EmployeeResponse>(employee);

            return employeeResponse;

        }
        [Authorize("user,admin")]
        // POST: api/Tutorial
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] EmployeeRequest employeeRequest)
        {
            if (ModelState.IsValid)
            {
                

                var employee = _mapper.Map<EmployeeRequest, Employee>(employeeRequest);

                var result = await _employeeDomain.SaveAsync(employee);

                return result ? StatusCode(201) : StatusCode(500);
            }
            else
            {
                return StatusCode(400);
            }
        }
        [Authorize("user,admin")]
        // PUT: api/Tutorial/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] EmployeeRequest employeeRequest)
        {
            if(ModelState.IsValid)
            {
                _employeeDomain.update(id, employeeRequest.Name, employeeRequest.last_name, employeeRequest.email, employeeRequest.area, employeeRequest.cargo);

            }
            else
            {
                StatusCode(400);
            }

        }
        [Authorize("user,admin")]
        // DELETE: api/Tutorial/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _employeeDomain.delete(id);
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<dynamic> Login([FromBody] EmployeeRequestLogin employeeRequestLogin)
        {
            try
            {
                var user = _mapper.Map<EmployeeRequestLogin, Employee>(employeeRequestLogin);

                var jwt = await _employeeDomain.Login(user);
                var user_founded = await _employeeDomain.GetByEmail(employeeRequestLogin.email);

                return new
                {
                    token = Ok(jwt),
                    user_id = user_founded.Id
                };
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status400BadRequest, "Error al procesar");
            }
        }
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost]
        [Route("Signup")]
        public async Task<IActionResult> Signup([FromBody] EmployeeRequest employeesignup)
        {
            var employee = _mapper.Map<EmployeeRequest,Employee>(employeesignup);
            var id = await _employeeDomain.Signup(employee);

            if (id > 0)
                return Ok(id.ToString());
            else
                return BadRequest();
        }


    }
}
