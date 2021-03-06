﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shared.DBModels
{
    public class Restaurant : BaseModel
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [MaxLength(20)]
        [Display(Name = "Opening Hours")]
        public string OpeningHours { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }

        [NotMapped]
        public Double Rating { get; set; }
        

        public virtual ICollection<Review> Reviews {get;set;}
        
        [Display(Name = "Picture")]
        public string ImageNameWithExtension { get; set; }
        public string ImageFullURL
        {
            get
            {
                if (ImageNameWithExtension != null)
                {
                    //return "https://localhost:44357/" + "Images/Restaurants/" + ImageNameWithExtension;
                    return "https://restaurants2020.azurewebsites.net/" + "Images/Restaurants/" + ImageNameWithExtension;
                
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
