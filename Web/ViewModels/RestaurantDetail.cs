using Shared.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class RestaurantDetail
    {
        public Restaurant Restaurant { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
