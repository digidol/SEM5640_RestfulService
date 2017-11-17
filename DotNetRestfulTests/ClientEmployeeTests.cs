using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetRestfulWebClient.Models;

namespace DotNetRestfulTests
{
    [TestClass]
    public class ClientEmployeeTests
    {
        [TestMethod]
        public void ShouldCreateEmptyEmployee()
        {
            ClientEmployee employee = new ClientEmployee();
            Assert.AreEqual(0, employee.Number);
            Assert.IsNull(employee.Name);
            Assert.IsNull(employee.Address);
            Assert.AreEqual("No Address Specified", employee.AddressSummary);
        }
    }
}
