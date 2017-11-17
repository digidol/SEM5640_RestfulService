using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetRestfulWebClient.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using DotNetRestfulWebClient.Models;
using Microsoft.Extensions.Logging;

namespace DotNetRestfulTests
{
    [TestClass]
    public class EmployeeControllerTests
    {
        private ILoggerFactory loggerFactory = null;

        private ILogger<EmployeeController> logger = null;

        private EmployeeController controller = null;

        [TestInitialize]
        public void setup()
        {
            loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddDebug();

            logger = loggerFactory.CreateLogger<EmployeeController>();
            controller = new EmployeeController(logger);
        }

        [TestMethod]
        public async Task ShouldShowListOfEmployeesForIndex()
        {
            var result = await controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
            Assert.AreEqual(true, result.ViewData["success"]);
            List<ClientEmployee> employees = result.Model as List<ClientEmployee>;
            Assert.IsNotNull(employees);
            Assert.AreEqual(3, employees.Count);
        }

        [TestMethod]
        public async Task ShowShowDetailsForOneEmployee()
        {
            var result = await controller.Details(1001) as ViewResult;
            Assert.AreEqual("Details", result.ViewName);

            ClientEmployee employee = result.Model as ClientEmployee;
            Assert.IsNotNull(employee);
            Assert.AreEqual("Augusta Ada King-Noel", employee.Name);
            Assert.AreEqual(1001, employee.Number);
            Assert.AreEqual(101, employee.Address.Number);
            Assert.AreEqual("Countess Road", employee.Address.StreetName);
            Assert.AreEqual("London", employee.Address.City);
            Assert.AreEqual("W1 4CL", employee.Address.PostCode);

        }
    }
}
