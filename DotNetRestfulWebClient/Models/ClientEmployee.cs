using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetRestfulWebClient.Models
{
    public class ClientEmployee
    {
        public int Number { get; set; }

        public String Name { get; set; }

        public ClientAddress Address { get; set; }

        public String AddressSummary
        {
            get
            {
                if (Address != null)
                {
                    return Address.ToString();
                }
                else
                {
                    return "No Address Specified";
                }
            
            }
        }
    }
}
