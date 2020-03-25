using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.DBModels
{
    public class Reviews: BaseModel
    {
        public int Rating { get; set; }
        public DateTime VisitDate { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [DataType(DataType.MultilineText)]
        public string ReplyByTheOwner { get; set; }
    }
}
