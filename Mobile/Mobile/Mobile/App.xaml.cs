using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mobile.Services;
using Mobile.Views;
using Mobile.Views;

namespace Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

           
            //MainPage = new AppShell();

            MainPage = new Login();
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
