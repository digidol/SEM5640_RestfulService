using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetRestful.Models;

namespace DotNetRestful.Controllers
{
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        /// <summary>
        /// A local list of employees that is used for each request. A real
        /// application would use a data storage mechanism, e.g. the Entity 
        /// Framework, to store the information. This is sufficient for this
        /// example.
        /// </summary>
        private static List<WebApiEmployee> employees = new List<WebApiEmployee>();

        private static int MaximumId = 1001;

        /// <summary>
        /// Initialises the class.
        /// </summary>
        public EmployeesController()
        {
            WebApiEmployee employee = new WebApiEmployee()
            {
                Name = "Augusta Ada King-Noel",
                Number = 1001, 
                Address = new WebApiAddress()
                {
                    Number = 101, 
                    StreetName = "Countess Road",
                    City = "London", 
                    PostCode = "W1 4CL"
                }
            };
            AddEmployee(employee);

            employee = new WebApiEmployee()
            {
                Name = "Alan Turing",
                Number = 1002,
                Address = new WebApiAddress()
                {
                    Number = 10,
                    StreetName = "Imitation Street",
                    City = "Manchester",
                    PostCode = "M1 4IG"
                }
            };
            AddEmployee(employee);

            employee = new WebApiEmployee()
            {
                Name = "Grace Hopper",
                Number = 1003,
                Address = new WebApiAddress()
                {
                    Number = 111,
                    StreetName = "Debugging Street",
                    City = "New York",
                    PostCode = "90510"
                }
            };
            AddEmployee(employee);
        }

        /// <summary>
        /// Adds an employee if there isn't an employee with the same 
        /// number.
        /// </summary>
        /// <param name="employee">The employee to be added.</param>
        /// <returns></returns>
        private Boolean AddEmployee(WebApiEmployee employee)
        {
            Boolean add = true;
            foreach(WebApiEmployee existingEmployee in employees)
            {
                if(existingEmployee.Number == employee.Number)
                {
                    add = false;
                    break;
                }
            }

            if(add)
            {
                employees.Add(employee);
                if(employee.Number > MaximumId)
                {
                    MaximumId = employee.Number;
                }
            }

            return add;
        }

        // GET api/employees
        [HttpGet]
        public IEnumerable<WebApiEmployee> Get()
        {
            return employees;
        }

        // GET api/employees/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            WebApiEmployee found = null;

            foreach(WebApiEmployee employee in employees)
            {
                if(employee.Number == id)
                {
                    found = employee;
                    break;
                }
            }

            if(found == null)
            {
                return BadRequest();
            }

            return Ok(found);
        }

        // POST api/employees
        [HttpPost]
        public IActionResult Post([FromBody]WebApiEmployee value)
        {
            value.Number = ++MaximumId;
            
            employees.Add(value);

            return Created($"/api/employees/{value.Number}", value.Number);
        }

        

        // PUT api/employees/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]WebApiEmployee value)
        {
            if(id != value.Number)
            {
                return BadRequest();
            }

            WebApiEmployee existingEmployee = employees.Find(x => x.Number == id);
            if(existingEmployee == null)
            {
                employees.Add(value);
                // Return code 201 to indicate that the resource has been created
                return Created($"/api/employees/{value.Number}", value.Number);
            }
            else
            {
                existingEmployee.Name = value.Name;
                existingEmployee.Address = value.Address;
                // The information was updated, so we return a 204 No Content status
                return NoContent();
            }
        }

        // DELETE api/employees/5
        [HttpDelete("{id}")]
        public Boolean Delete(int id)
        {
            return false;
        }
    }
}
