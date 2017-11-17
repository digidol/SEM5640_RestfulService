using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetRestfulWebClient.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace DotNetRestfulWebClient.Services
{
    public class RestfulClientService : IRestfulClientService
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
        /// Initialise the HttpClient. Done once and then used by 
        /// the entire controller.
        /// </summary>
        static RestfulClientService()
        {
            client.BaseAddress = new Uri("http://localhost:50727");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>Returns the new ID created for the employee. If there is an error, 
        /// an Exception will be thrown.</returns>
        public async Task<Uri> CreateEmployee(ClientEmployee employee)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/employees/", employee);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If there is an error, an exception will be thrown.</returns>
        public async Task<int> DeleteEmployee(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync("/api/employees/{id}");
            response.EnsureSuccessStatusCode();
            return 1;
        }

        /// <summary>
        /// Retrieves the Employee details for the specified ID.
        /// </summary>
        /// <param name="id">The ID of the Employee.</param>
        /// <returns>The Employee information is returned, if found. If the Employee data
        /// for the specified ID does not exist, null is returned.</returns>
        public async Task<ClientEmployee> Employee(int id)
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

        /// <summary>
        /// Retrieves the list of Employees.
        /// </summary>
        /// <returns>A list of Employees if the call was successful. Otherwise,
        /// null is returned.</returns>
        public async Task<List<ClientEmployee>> Employees()
        {
            HttpResponseMessage response = await client.GetAsync("/api/employees/");

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<ClientEmployee>>(data);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Updates the Employee with the specified ID. 
        /// </summary>
        /// <param name="id">The ID to update.</param>
        /// <param name="employee">The Employee details to use for the update.</param>
        /// <returns>The ID of the Employee is returned if the update was successful. Otherwise, 
        /// an Exception is thrown.</returns>
        public async Task<Uri> UpdateEmployee(int id, ClientEmployee employee)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync("/api/employees/{id}", employee);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location; 
        }
    }
}
