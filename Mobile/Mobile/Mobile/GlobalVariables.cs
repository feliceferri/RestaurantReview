using Shared.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

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
        internal static string BaseURLWS = "http://10.0.2.2:5000/";





        public static LoggedUser LoggedUser = null;
    }
}
