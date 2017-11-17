using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetRestful.Models
{
    public class WebApiAddress
    {
        public int Number { get; set; }

        public String StreetName { get; set; }

        public String City { get; set; }

        public String PostCode { get; set; }
    }
}
