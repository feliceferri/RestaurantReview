using Newtonsoft.Json;
using Shared.DBModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Mobile.Services.APIComm;

namespace Mobile.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public Command AddRestaurantCommand { get; set; }
        public Command SaveNewRestaurantCommand { get; set; }

        public Shared.DTOModels.NewRestaurant Restaurant { get; set; } = new Shared.DTOModels.NewRestaurant();

        public ObservableCollection<Restaurant> ListRestaurants { get; set; }


        int? _RatingFilter;
        public int? RatingFilter
        {
            get { return _RatingFilter; }
            set
            {
                _RatingFilter = value;
                this.OnPropertyChanged(nameof(RatingFilter));

                Task.Run(async () =>
                {
                    await LoadData();
                });
            }
        }

        bool _BtnAddRestaurantIsVisible = false;
        public bool BtnAddRestaurantIsVisible { 
            get { return _BtnAddRestaurantIsVisible; }
            set { _BtnAddRestaurantIsVisible = value;
                this.OnPropertyChanged(nameof(BtnAddRestaurantIsVisible));
            } }

        bool _ShowFilterByRating = false;
        public bool ShowFilterByRating
        {
            get { return _ShowFilterByRating; }
            set
            {
                _ShowFilterByRating = value;
                this.OnPropertyChanged(nameof(ShowFilterByRating));
            }
        }

        bool _IsFilteringByRating = false;
        public  bool IsFilteringByRating
        {
            get { return _IsFilteringByRating; }
            set
            {
                _IsFilteringByRating = value;
                this.OnPropertyChanged(nameof(IsFilteringByRating));


                    Task.Run(async () =>
                    {
                        await LoadData();
                    });

            }
        }
                

        public  MainViewModel()
        {
            AddRestaurantCommand = new Command(async () => await ExecuteAddRestaurantCommand());
            SaveNewRestaurantCommand = new Command(async () => await ExecuteSaveNewRestaurantCommand());

            if(GlobalVariables.LoggedUser.Roles.Contains("Owner") || GlobalVariables.LoggedUser.Roles.Contains("Admin"))
            {
                this.BtnAddRestaurantIsVisible = true;
            }
            else
            {
                this.ShowFilterByRating = true;
                
            }

            Task.Run(async () =>
            {
                await LoadData();
            });
            
        }

        private async Task LoadData()
        {
            CallAsync_Results res;

            if (IsFilteringByRating == true)
            {
                res = await Services.APIComm.CallGetAsync(@"Restaurants/ByUserId/" + GlobalVariables.LoggedUser.ID + $"/FilterByRating/{RatingFilter.GetValueOrDefault()}");
            }
            else
            {
                res = await Services.APIComm.CallGetAsync(@"Restaurants/ByUserId/" + GlobalVariables.LoggedUser.ID);
            }

            if (res.Success == true)
            {
                ListRestaurants = new ObservableCollection<Restaurant>(Newtonsoft.Json.JsonConvert.DeserializeObject<List<Restaurant>>(res.ContentString_responJsonText));
                this.OnPropertyChanged(nameof(ListRestaurants));
            }
        }

        public string Title
        {
            get { return GlobalVariables.LoggedUser.Roles.Contains("Owner") ? "My Restaurants" : "Restaurants"; }
        }

        public bool ShowEditingRestaurant { get; set; }

        async Task ExecuteAddRestaurantCommand()
        {
            
            this.ShowEditingRestaurant = !this.ShowEditingRestaurant;
            this.OnPropertyChanged(nameof(ShowEditingRestaurant));
        }

        async Task ExecuteSaveNewRestaurantCommand()
        {
            IsBusy = true;
            try
            {
                Restaurant.Id = Guid.NewGuid(); 
                Restaurant.CreatedById = GlobalVariables.LoggedUser.ID;
                               

                var res = await Services.APIComm.CallPostAsync(Restaurant, "Restaurants/New", true);
                if (res.Success == true)
                {
                    Restaurant  = new Shared.DTOModels.NewRestaurant();
                    await ExecuteAddRestaurantCommand(); //Closes the New Restaurant StackPanel
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", string.Join(Environment.NewLine, res.ContentString_responJsonText), "Ok");
                }
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
    }
