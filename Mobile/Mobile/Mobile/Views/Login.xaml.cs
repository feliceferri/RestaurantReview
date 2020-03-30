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
    public partial class Login : ContentPage
    {
        LoginViewModel viewModel;
        public Login()
        {
            InitializeComponent();

            
            viewModel = new LoginViewModel();
            BindingContext = viewModel;
        }
               

        private async void OnRegisterNewTapped(object sender, EventArgs e)
        {
          
        }
    }
}