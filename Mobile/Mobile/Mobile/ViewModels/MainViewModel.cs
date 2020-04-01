using Shared.DBModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public Command AddRestaurantCommand { get; set; }
        public Command SaveNewRestaurantCommand { get; set; }

        public Restaurant Restaurant { get; set; } = new Restaurant();

        public MainViewModel()
        {
            AddRestaurantCommand = new Command(async () => await ExecuteAddRestaurantCommand());
            SaveNewRestaurantCommand = new Command(async () => await ExecuteSaveNewRestaurantCommand());
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
        }
    }
    }
