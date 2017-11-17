using System;
using System.Collections.Generic;
using System.Text;
using DotNetRestfulWebClient.Models;
using DotNetRestfulWebClient.Services;
using System.Threading.Tasks;

namespace DotNetRestfulTests
{
    public class RestfulClientServiceErrorStub : IRestfulClientService
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

        public Task<List<ClientEmployee>> Employees()
        {
            Task<List<ClientEmployee>> task = new Task<List<ClientEmployee>>(() => null);
            task.Start();
            return task;
        }

        public Task<Uri> UpdateEmployee(int id, ClientEmployee employee)
        {
            throw new NotImplementedException();
        }
    }
}
