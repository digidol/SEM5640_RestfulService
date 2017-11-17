using DotNetRestfulWebClient.Controllers;
using DotNetRestfulWebClient.Models;
using DotNetRestfulWebClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotNetRestfulTests
{
    [TestClass]
    public class EmployeeWithInjectionTest
    {
        private ILoggerFactory loggerFactory = null; 

        private ILogger<EmployeeWithInjectionController> logger = null;
        
        [TestInitialize]
        public void setup()
        {
            loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddDebug();

            logger = loggerFactory.CreateLogger<EmployeeWithInjectionController>();
        }

        [TestMethod]
        public async Task ShouldShowListOfEmployees()
        {
            RestfulClientServiceStub clientService = new RestfulClientServiceStub();
            EmployeeWithInjectionController controller = new EmployeeWithInjectionController(logger, clientService);
            var result = await controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);

            List<ClientEmployee> employees = result.Model as List<ClientEmployee>;
            Assert.AreEqual(2, employees.Count);
        }

        [TestMethod]
        public async Task ShouldHandleErrorWithConnectionForListOfEmployees()
        {
            IRestfulClientService clientService = new RestfulClientServiceErrorStub();
            EmployeeWithInjectionController controller = new EmployeeWithInjectionController(logger, clientService);
            var result = await controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);

            List<ClientEmployee> employees = result.Model as List<ClientEmployee>;
            Assert.AreEqual(0, employees.Count);
            Assert.AreEqual(false, result.ViewData["success"]);
        }

    }
}
