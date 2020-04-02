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
    public partial class Main : ContentPage
    {
        MainViewModel viewModel;
        public Main()
        {
            InitializeComponent();

            viewModel = new MainViewModel();
            BindingContext = viewModel;
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            GlobalVariables.SelectedRestaurantId = (e.CurrentSelection[0] as Shared.DBModels.Restaurant).Id;
            await Xamarin.Forms.Shell.Current.GoToAsync("Restaurant");
        }
    }
}