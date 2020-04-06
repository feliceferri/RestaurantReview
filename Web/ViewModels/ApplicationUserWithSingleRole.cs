using Shared.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class ApplicationUserWithSingleRole 
    {
        public ApplicationUser User { get; set; }
        public string RoleName { get; set; }
    }
}
