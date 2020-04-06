using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class ApplicationUser_CreateUser: ApplicationUserWithSingleRole
    {
        public string Password { get; set; }
    }
}
