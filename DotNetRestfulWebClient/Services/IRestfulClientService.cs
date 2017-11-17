using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetRestfulWebClient.Models;

namespace DotNetRestfulWebClient.Services
{
    public interface IRestfulClientService
    {
        Task<ClientEmployee> Employee(int id);

        Task<List<ClientEmployee>> Employees();

        Task<Uri> CreateEmployee(ClientEmployee employee);

        Task<Uri> UpdateEmployee(int id, ClientEmployee employee);

        Task<int> DeleteEmployee(int id); 
    }
}
