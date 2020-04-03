using Newtonsoft.Json;
using Shared.DBModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;

namespace Mobile.ViewModels
{
    public class RestaurantViewModel : BaseViewModel
    {

        public Restaurant Restaurant { get; set; }
        public Review MyReview { get; set; } =  new Review();

        public bool FrameCreateReview_Isvisible { get; set; }

        public Command SaveReviewCommand { get; set; }
        public RestaurantViewModel(Guid RestaurantId)
        {
            MyReview.VisitDate = DateTime.Now;

            SaveReviewCommand = new Command(async () => await ExecuteSaveReviewCommand());

            Task.Run(async () =>
            {
                var res = await Services.APIComm.CallGetAsync($"Restaurants/ByRestaurantId/{RestaurantId}/IncludeReviews");
                if (res.Success == true)
                {
                    this.Restaurant = Newtonsoft.Json.JsonConvert.DeserializeObject<Restaurant>(res.ContentString_responJsonText);
                    this.OnPropertyChanged(nameof(Restaurant));

                    if (this.Restaurant.Reviews != null && this.Restaurant.Reviews.Count > 0)
                    {
                        this.MyReview = (from p in this.Restaurant.Reviews
                                         where p.CreatedById == GlobalVariables.LoggedUser.ID
                                         select p).FirstOrDefault();
                        if(MyReview == null)
                        {
                            FrameCreateReview_Isvisible = true;
                            this.OnPropertyChanged(nameof(FrameCreateReview_Isvisible));
                        }
                    }
                    else
                    {
                        FrameCreateReview_Isvisible = true;
                        this.OnPropertyChanged(nameof(FrameCreateReview_Isvisible));
                    }
                  
                }
            });
        }

        async Task ExecuteSaveReviewCommand()
        {
            IsBusy = true;

            try
            {

                if (MyReview.VisitDate == DateTime.MinValue || MyReview.Rating == 0 || string.IsNullOrWhiteSpace(MyReview.Comment) == true)
                {
                    await Application.Current.MainPage.DisplayAlert("Validation", "All fields must contain data", "Ok");
                }
                else
                {
                    MyReview.Id = Guid.NewGuid();
                    MyReview.Created = DateTime.Now;
                    MyReview.CreatedById = GlobalVariables.LoggedUser.ID;
                    MyReview.RestaurantId = Restaurant.Id;

                    var res = await Services.APIComm.CallPostAsync(this.MyReview, "Reviews/New", true);
                    if (res.Success == true)
                    {
                        FrameCreateReview_Isvisible = false;
                        this.OnPropertyChanged(nameof(FrameCreateReview_Isvisible));
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", string.Join(Environment.NewLine, res.ContentString_responJsonText), "Ok");
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
