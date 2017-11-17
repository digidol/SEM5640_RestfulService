using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetRestfulWebClient.Models
{
    public class ClientAddress
    {
        public int Number { get; set; }

        public String StreetName { get; set; }

        public String City { get; set; }

        public String PostCode { get; set; }

        override
        public String ToString()
        {
            return Number + " " + StreetName + ", " + City + ", " + PostCode;
        }
    }
}
