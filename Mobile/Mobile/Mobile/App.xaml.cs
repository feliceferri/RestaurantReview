using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mobile.Services;
using Mobile.Views;
using Mobile.Views;
using System.Collections.Generic;

namespace Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();


            GlobalVariables.LoggedUser = new Shared.DTOModels.LoggedUser() { Name = "Franco Ferri", Roles = new List<string>() { "Owner" } };

            MainPage = new AppShell();

            //MainPage = new Login();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
