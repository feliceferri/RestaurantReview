using Mobile.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.DTOModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    internal class RegisterViewModel : BaseViewModel
    {

        public Local_RegisteringUser User { get; set; } = new Local_RegisteringUser();

        public Command RegisterCommand { get; set; }

        public RegisterViewModel()
        {
            RegisterCommand = new Command(async () => await ExecuteRegisterCommand());
        }

        async Task ExecuteRegisterCommand()
        {
            IsBusy = true;

            try
            {

                if (string.IsNullOrEmpty(User.Password) || string.IsNullOrEmpty(User.Email))
                {
                    await Application.Current.MainPage.DisplayAlert("Validation", "All fields must contain data", "Ok");
                }
                else
                {
                    if(User.IsOwner == true)
                    {
                        User.Roles = new List<string>() { "Owner" };
                    }
                    else
                    {
                        User.Roles = new List<string>() { "Regular" };
                    }
                    var res = await Services.APIComm.CallPostAsync(User, "Users/Register", true);
                    if (res.Success == true)
                    {
                        GlobalVariables.LoggedUser = JsonConvert.DeserializeObject<LoggedUser>(res.ContentString_responJsonText);

                        Application.Current.MainPage = new AppShell();
                    }
                    else
                    {
                        //string error = result.Content.ReadAsStringAsync().Result;
                        //JObject jObject = JObject.Parse(error);
                        //await Application.Current.MainPage.DisplayAlert("Error", string.Join(Environment.NewLine, jObject[""]), "Ok");
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
