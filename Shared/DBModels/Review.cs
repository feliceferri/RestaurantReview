﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shared.DBModels
{
    public class Review: BaseModel
    {
        public int Rating { get; set; }
        public DateTime VisitDate { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name="Owner Reply")]
        public string ReplyByTheOwner { get; set; }

        public Guid RestaurantId {get;set;}

        public Restaurant Restaurant {get;set;}

        [NotMapped]
        public string TypeOfReview { get; set; }
    }
}
