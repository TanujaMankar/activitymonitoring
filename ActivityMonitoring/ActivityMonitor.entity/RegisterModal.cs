using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;     
   
namespace ActivityMonitor.entity
{ 
    public class RegisterModal
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string UserType { get; set; }
        public DateTime? DOB { get; set; }

    }
}
