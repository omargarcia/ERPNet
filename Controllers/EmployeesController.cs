﻿
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ERPNet.Models;
using Microsoft.AspNetCore.Cors;
using ERPNet.Data.Repositories;

namespace ERPNet.Controllers
{
    [EnableCors ( "AllowSpecificOrigin" )]
    [Route("api/[controller]")]
    //[Authorize ( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Admin )]
    [ApiController]
    public class EmployeesController : GenericController<Employee, EmployeeRepository>
    {

        private readonly EmployeeRepository _repository;
       
        public EmployeesController ( EmployeeRepository repository ) : base(repository)
        {
            _repository = repository;
        }

        // GET: api/Employees
       [HttpGet( "byperson" )]
        public async Task<IEnumerable<Employee>> GetAllEmployees ( )
        {
          
            return await _repository.GetAll();
        }

        //GET: api/Employees/5
        [HttpGet ( "person/{id}" )]
        public async Task<ActionResult<Employee>> GetByPerson ( int id )
        {
            var employeebyPerson = await _repository.GetEmployee ( id );

            if(employeebyPerson == null)
            {
                return NotFound ();
            }
            return employeebyPerson;
        }

        //// POST: api/Employees
        [HttpPost("employee")]
        public async Task<ActionResult<Employee>> PostEmployee ( Employee employee )
        {
            var newEmployee = await _repository.AddEmployee ( employee );

            return CreatedAtAction ( "GetEmployee", new { id = employee.Id }, newEmployee );
        }

        // PUT: api/Employees/5
       [HttpPut ( "edit{id}" )]
        public async Task<ActionResult<Employee>> EditEmployee ( Employee employee )
        {
            var employeeEdited = await _repository.GetEmployee ( employee.Id );

            employeeEdited.Id = employee.Id;
            employeeEdited.Name = employee.Name;
            employeeEdited.LastName = employee.LastName;
            employeeEdited.PositionJob = employee.PositionJob;
            employeeEdited.Salary = employee.Salary;
            employeeEdited.UserName = employee.UserName;
            employeeEdited.Password = employee.Password;

            return await _repository.Update ( employeeEdited );
        } 

        // DELETE: api/Employees/5
        [HttpDelete ( "{id}" )]
        public async Task<ActionResult<Employee>> DeleteEmployee ( int id )
        {
            var employee = await _repository.Delete ( id );
            if(employee == null)
            {
                return NotFound ();
            }

            return employee;
        }

    }
}
