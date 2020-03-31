using Mobile.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {

        RegisterViewModel viewModel;

        public Register()
        {
            InitializeComponent();


            viewModel = new RegisterViewModel();
            BindingContext = viewModel;
        }

      

        private async void OnRegisterTapped(object sender, EventArgs e)
        {
           
        }





        private async void OnLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void switchUserOwner_Toggled(object sender, ToggledEventArgs e)
        {
            if(this.switchUserOwner.IsToggled == false)
            {
                this.lblRegularUser.TextColor = Color.White;
                this.lblRestaurantOwner.TextColor = Color.Gray;
            }
            else
            {
                this.lblRegularUser.TextColor = Color.Gray;
                this.lblRestaurantOwner.TextColor = Color.White;
            }
        }
    }
}