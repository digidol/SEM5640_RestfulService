using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DotNetRestfulWebClient.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace DotNetRestfulWebClient.Controllers
{
    public class EmployeeController : Controller
    {
        /// <summary>
        /// Client that manages network accesses. An extension, 
        /// provided by the extra dependency on 
        /// Microsoft.AspNet.WebApi.Client (added using NuGet) 
        /// enables operations to make it easier to send and 
        /// receive JSON data. This is kept as a static object
        /// following guidance from Microsoft. If many instances 
        /// are created, the server would quickly use up all
        /// of the available network sockets.
        /// </summary>
        private static HttpClient client = new HttpClient();

        /// <summary>
        /// Logging service provided to the application. Supplied
        /// through dependency injection in the constructor.
        /// </summary>
        private ILogger<EmployeeController> logger;

        /// <summary>
        /// Initialise the HttpClient. Done once and then used by 
        /// the entire controller.
        /// </summary>
        static EmployeeController()
        {
            client.BaseAddress = new Uri("http://localhost:50727");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Creates a new isntance of the controller, and expects to be 
        /// provided with a logger for the class, using dependency injection.
        /// </summary>
        /// <param name="logger"></param>
        public EmployeeController(ILogger<EmployeeController> logger)
        {
            this.logger = logger;
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
            HttpResponseMessage response = await client.GetAsync("/api/employees/");

            if(response.IsSuccessStatusCode)
            {
                ViewData["success"] = true;
                ViewData["response"] = response.Content.ReadAsStringAsync().Result;
                var data = response.Content.ReadAsStringAsync().Result;
                List<ClientEmployee> employees = JsonConvert.DeserializeObject<List<ClientEmployee>>(data);
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
            if(id == null)
            {
                return BadRequest();
            }

            ClientEmployee employee = await LoadEmployee(id.Value);

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

                HttpResponseMessage response = await client.PostAsJsonAsync("/api/employees/", employee);
                response.EnsureSuccessStatusCode(); 

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewData["message"] = "There was a problem creating the data: " + ex.Message;
                logger.LogError(ex.Message);
                logger.LogError(ex.StackTrace);
                return View("Create");
            }
        }

        /// <summary>
        /// Private utility method that is used to access details for a single 
        /// employee.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<ClientEmployee> LoadEmployee(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/employees/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ClientEmployee>(data);
            }
            else
            {
                return null;
            }
        }

        // GET: Employee/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ClientEmployee employee = await LoadEmployee(id); 

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

                HttpResponseMessage response = await client.PutAsJsonAsync("/api/employees/{id}", employee);
                response.EnsureSuccessStatusCode();

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
            if(id == null)
            {
                return new BadRequestResult();
            }

            ClientEmployee employee = await LoadEmployee(id.Value);

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
                HttpResponseMessage response = await client.DeleteAsync("/api/employees/{id}");
                response.EnsureSuccessStatusCode();

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