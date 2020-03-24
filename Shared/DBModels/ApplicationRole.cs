using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DBModels
{
    //https://entityframeworkcore.com/knowledge-base/51004516/-net-core-2-1-identity-get-all-users-with-their-associated-roles
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
