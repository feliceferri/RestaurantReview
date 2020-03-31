using Newtonsoft.Json;
using Shared.DTOModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    internal class LoginViewModel : BaseViewModel
    {
        
        public LoginUser User { get; set; }  = new LoginUser();

        public Command LoginCommand{ get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await ExecuteLoginCommand());
        }

        public string Version { get { return "v" + Xamarin.Essentials.AppInfo.VersionString; } }
        async Task ExecuteLoginCommand()
        {
            IsBusy = true;

            try
            {
                    var res = await Services.APIComm.CallPostAsync(User, "Users/Login", true);
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
