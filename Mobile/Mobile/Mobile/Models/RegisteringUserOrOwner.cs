using Shared.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Models
{
    public class  Local_RegisteringUser: RegisteringUser
    {
        public bool IsOwner { get; set; }
    }
}
