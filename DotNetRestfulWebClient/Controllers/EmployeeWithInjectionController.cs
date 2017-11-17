using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using DotNetRestfulWebClient.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using DotNetRestfulWebClient.Services;

namespace DotNetRestfulWebClient.Controllers
{
    /// <summary>
    /// This is an alternative representation of the EmployeeController. The
    /// difference is that this one can receive a class that provides a service 
    /// to access the Restful Web Service. This is done through dependency 
    /// injection.  This approach allows us to separate the logic of the 
    /// controller from the logic of dealing with the model. This can allow 
    /// us to test the different issues in the controller.
    /// </summary>
    public class EmployeeWithInjectionController : Controller
    {
        /// <summary>
        /// Logging service provided to the application. Supplied
        /// through dependency injection in the constructor.
        /// </summary>
        private ILogger<EmployeeWithInjectionController> logger;

        private IRestfulClientService clientService;

        /// <summary>
        /// Creates a new instance of the controller, and expects to be 
        /// provided with a logger for the class, using dependency injection.
        /// </summary>
        /// <param name="logger"></param>
        public EmployeeWithInjectionController(ILogger<EmployeeWithInjectionController> logger, 
            IRestfulClientService clientService)
        {
            this.logger = logger;
            this.clientService = clientService;
        }

        /// <summary>
        /// Sends a query to the specified service to obtain a list of all of the 
        /// employees in the service. Those employees are then passed to the view 
        /// for display. Note that the method is marked as asynchronous, because of 
        /// the call to GetAsync for the network access. As a result, the return type 
        /// is wrapped in a Task so that the MVC process can capture the result. 
        /// </summary>
        /// <returns></returns>
        // GET: Employee
        public async Task<ActionResult> Index()
        {
            List<ClientEmployee> employees = await clientService.Employees();

            if (employees != null)
            {
                ViewData["success"] = true;
                logger.LogInformation($"Obtained {employees.Count} values.");
                return View("Index", employees);
            }
            else
            {
                ViewData["success"] = false;
                ViewData["response"] = "Unable to access data";
                return View("Index", new List<ClientEmployee>());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">This is the identifier to get the details for a specific employee. If 
        /// this is null, a Bad Request will be returned.</param>
        /// <returns></returns>
        // GET: Employee/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            ClientEmployee employee = await clientService.Employee(id.Value);

            if (employee != null)
            {
                ViewData["success"] = true;
                return View("Details", employee);
            }
            else
            {
                ViewData["success"] = false;
                return View("Details", new ClientEmployee());
            }
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                logger.LogInformation("about to create");
                ClientEmployee employee = new ClientEmployee();
                employee.Name = collection["Name"];
                employee.Address = new ClientAddress();
                employee.Address.Number = Int32.Parse(collection["Address.Number"]);
                employee.Address.StreetName = collection["Address.StreetName"];
                employee.Address.City = collection["Address.City"];
                employee.Address.PostCode = collection["Address.PostCode"];

                Uri response = await clientService.CreateEmployee(employee);
                logger.LogInformation($"Create - Returned URI: {response}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["message"] = "There was a problem creating the data: " + ex.Message;
                logger.LogError(ex.Message);
                logger.LogError(ex.StackTrace);
                return View("Create");
            }
        }

        // GET: Employee/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ClientEmployee employee = await clientService.Employee(id);

            if (employee != null)
            {
                ViewData["success"] = true;
                return View("Edit", employee);
            }
            else
            {
                ViewData["success"] = false;
                return View("Edit", new ClientEmployee());
            }
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                logger.LogInformation("about to update");
                ClientEmployee employee = new ClientEmployee();
                employee.Number = Int32.Parse(collection["Number"]);
                employee.Name = collection["Name"];
                employee.Address = new ClientAddress();
                employee.Address.Number = Int32.Parse(collection["Address.Number"]);
                employee.Address.StreetName = collection["Address.StreetName"];
                employee.Address.City = collection["Address.City"];
                employee.Address.PostCode = collection["Address.PostCode"];

                Uri response = await clientService.UpdateEmployee(id, employee);
                logger.LogInformation($"Edit - Returned URI: {response}");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["message"] = "There was a problem updating the data: " + ex.Message;
                logger.LogError(ex.Message);
                logger.LogError(ex.StackTrace);
                return View("Edit");
            }
        }

        /// <summary>
        /// Retrieves information about a specified employee based on the 
        /// ID for the employee. This is then shown to the user to check 
        /// if this is the set of details to delete.
        /// </summary>
        /// <param name="id">The id for the employee.</param>
        /// <returns></returns>
        // GET: Employee/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            ClientEmployee employee = await clientService.Employee(id.Value);

            if (employee != null)
            {
                ViewData["success"] = true;
                return View("Delete", employee);
            }
            else
            {
                ViewData["success"] = false;
                return View("Delete", new ClientEmployee());
            }
        }

        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                logger.LogInformation("about to delete");
                int response = await clientService.DeleteEmployee(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["message"] = "There was a problem deleting the data: " + ex.Message;
                logger.LogError(ex.Message);
                logger.LogError(ex.StackTrace);
                return View("Delete");
            }
        }

    }
}