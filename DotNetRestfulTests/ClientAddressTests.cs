using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetRestfulWebClient.Models;

namespace DotNetRestfulTests
{
    [TestClass]
    public class ClientAddressTests
    {
        [TestMethod]
        public void ShouldCreateEmptyClientAddress()
        {
            ClientAddress address = new ClientAddress();
            Assert.AreEqual(0, address.Number);
            Assert.IsNull(address.StreetName);
            Assert.IsNull(address.City);
            Assert.IsNull(address.PostCode);
        }
    }
}
