using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOModels
{
    public class AddReplyToComment
    {
        public Guid ReviewId { get; set; }
        public string ReviewReply { get; set; }
    }
}
