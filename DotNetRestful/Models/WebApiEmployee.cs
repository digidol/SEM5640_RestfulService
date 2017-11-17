using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetRestful.Models
{
    /// <summary>
    /// A class that holds some data about an Employee.
    /// </summary>
    public class WebApiEmployee
    {        
        /// <summary>
        /// 
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WebApiAddress Address { get; set; }

    }
}
