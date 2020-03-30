﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOModels
{
    public class LoggedUser
    {
        public string ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public Guid? CompanyID { get; set; }
        public Guid? DepartmentId { get; set; }

        public DateTime? Blocked { get; set; }
        public string BlockedBy { get; set; }

        //public DBModels.ApplicationUserRole UserRoles { get; set; }
        public virtual IEnumerable<string> Roles { get; set; }

    }
}
