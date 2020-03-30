using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Shared.DTOModels
{
    public class RegisteringUser: DTOBaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<string> Roles { get; set; }
        public Guid? CompanyID { get; set; }
        public Guid? DepartmentID { get; set; }
    }
}
