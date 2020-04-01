using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOModels
{
    public class LoggedUser
    {
        public string ID { get; set; }

        public string Name { get; set; }

       
        public DateTime? Blocked { get; set; }
        public string BlockedBy { get; set; }

        //public DBModels.ApplicationUserRole UserRoles { get; set; }
        public virtual IList<string> Roles { get; set; }

    }
}
