using Shared.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mobile
{
    internal class GlobalVariables
    {

        //James Montemagno
        //https://devblogs.microsoft.com/xamarin/cleartext-http-android-network-security/

        //Cleartext HTTP traffic to 192.168.1.106 not permitted at Java.Interop.JniEnvironment
        //Fix => [assembly: Application(UsesCleartextTraffic = true)] https://forums.xamarin.com/discussion/164771/cleartext-http-traffic-not-permitted

        //iOS simulator   iOS device  Android emulator    Android device
        //  127.0.0.1	    192.168.1.1	    10.0.2.2	        192.168.1.1

        //internal static string BaseURLWS = "https://10.0.2.2:5001/";
        //THIS ONE IN DEV internal static string BaseURLWS = "http://10.0.2.2:5000/";

        internal static string BaseURLWS = "https://restaurants2020.azurewebsites.net/";
        





        public static LoggedUser LoggedUser = null;

        public static Guid SelectedRestaurantId { get; set; }

        public static Xamarin.Forms.TabBar NavigationBar;


        public static async Task NavigateToAsync(string TabBarItemName)
        {
            //NavigationBar.CurrentItem = (from p in NavigationBar.Items
            //                             where p.Title == TabItemName
            //                             select p).FirstOrDefault();

            NavigationBar.CurrentItem = TabBarItemNameToTabBar(TabBarItemName);

            await Xamarin.Forms.Shell.Current.GoToAsync(TabBarItemName);

        }

       
        internal static ShellSection TabBarItemNameToTabBar(string TabBarItemName)
        {
            switch (TabBarItemName)
            {
                case "Main":
                    return NavigationBar.Items[0];
                    break;
                case "Restaurant":
                    return NavigationBar.Items[1];
                    break;
                default:
                    return null;
                    break;
            }

        }
    }
}
