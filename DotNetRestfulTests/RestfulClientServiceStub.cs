using System;
using System.Collections.Generic;
using System.Text;
using DotNetRestfulWebClient.Models;
using DotNetRestfulWebClient.Services;
using System.Threading.Tasks;

namespace DotNetRestfulTests
{
    public class RestfulClientServiceStub : IRestfulClientService
    {
        public Task<Uri> CreateEmployee(ClientEmployee employee)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ClientEmployee> Employee(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a list of two employees. This needs to create a task that 
        /// will return the list of employees. The task needs to be started 
        /// otherwise the result is not passed to any calling method.
        /// </summary>
        /// <returns>A list of ClientEmployee objects, with .</returns>
        public Task<List<ClientEmployee>> Employees()
        {
            List<ClientEmployee> employees = new List<ClientEmployee>();
            ClientEmployee employee = new ClientEmployee()
            {
                Name = "Augusta Ada King-Noel",
                Number = 1001,
                Address = new ClientAddress()
                {
                    Number = 101,
                    StreetName = "Countess Road",
                    City = "London",
                    PostCode = "W1 4CL"
                }
            };
            employees.Add(employee);

            employee = new ClientEmployee()
            {
                Name = "Alan Turing",
                Number = 1002,
                Address = new ClientAddress()
                {
                    Number = 10,
                    StreetName = "Imitation Street",
                    City = "Manchester",
                    PostCode = "M1 4IG"
                }
            };
            employees.Add(employee);

            Task<List<ClientEmployee>> task = new Task<List<ClientEmployee>>(() => employees);
            task.Start();
            return task;
        }

        public Task<Uri> UpdateEmployee(int id, ClientEmployee employee)
        {
            throw new NotImplementedException();
        }
    }
}
