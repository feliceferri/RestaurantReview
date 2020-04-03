using Newtonsoft.Json;
using Shared.DBModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    public class RestaurantViewModel : BaseViewModel
    {

        public Restaurant Restaurant { get; set; }
        public Review MyReview { get; set; }

        public RestaurantViewModel(Guid RestaurantId)
        {
            Task.Run(async () =>
            {
                var res = await Services.APIComm.CallGetAsync($"Restaurants/ByRestaurantId/{RestaurantId}/IncludeReviews");
                if (res.Success == true)
                {
                    Restaurant = Newtonsoft.Json.JsonConvert.DeserializeObject<Restaurant>(res.ContentString_responJsonText);
                    this.OnPropertyChanged(nameof(Restaurant));
                }
            });
        }
    }
}
