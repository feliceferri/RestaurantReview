using Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Restaurant : ContentPage
    {
        RestaurantViewModel viewModel;
        public Restaurant()
        {
            InitializeComponent();

            if (GlobalVariables.SelectedRestaurantId == null)
            {
                Task.Run(async () =>
                {
                    await GlobalVariables.NavigateToAsync("Main");
                });
            }
            else
            {
                viewModel = new RestaurantViewModel(GlobalVariables.SelectedRestaurantId);
                BindingContext = viewModel;
            }
        }
    }
}