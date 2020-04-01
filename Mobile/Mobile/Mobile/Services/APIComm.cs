using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Services
{
    internal class APIComm
    {

        public static readonly HttpClientHandler httpClientHandler = new HttpClientHandler();
        public class CallAsync_Results
        {
            public bool Success { get; set; }
            public System.Net.Http.HttpResponseMessage Response { get; set; }
            public String ContentString_responJsonText { get; set; }
            public System.Net.HttpStatusCode StatusCode { get; set; }
        }

        internal static async Task<CallAsync_Results> CallPostAsync(Object objToConvertToJSON, string RelativePath, Boolean ThrowAnyError = false)
        {
            
            Uri requestUri = new Uri(GlobalVariables.BaseURLWS + @"api/" + RelativePath);
            return await CallPostAsync(Newtonsoft.Json.JsonConvert.SerializeObject(objToConvertToJSON), requestUri, ThrowAnyError = false);
        }
        internal static async Task<CallAsync_Results> CallPostAsync(Object objToConvertToJSON, Uri RequestURI, Boolean ThrowAnyError = false)
        {
            return await CallPostAsync(Newtonsoft.Json.JsonConvert.SerializeObject(objToConvertToJSON), RequestURI, ThrowAnyError = false);
        }
            internal static async Task<CallAsync_Results> CallPostAsync(String JSONData, Uri RequestURI, Boolean ThrowAnyError = false)
        {
           
            CallAsync_Results Res = new CallAsync_Results();


            try
            {
                              
                    System.Net.Http.HttpClient objClint = new System.Net.Http.HttpClient();
                    System.Net.Http.HttpResponseMessage respon = null;
                                   

                    respon = await objClint.PostAsync(RequestURI, new System.Net.Http.StringContent(JSONData, System.Text.Encoding.UTF8, "application/json"));
                    Res.Response = respon;
                    Res.StatusCode = respon.StatusCode;
                    if (respon != null)
                    {
                        string responJsonText = await respon.Content.ReadAsStringAsync();
                        Res.ContentString_responJsonText = responJsonText;
                        if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //Dim serverResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responJsonText)
                            Res.Success = true;
                        }
                        
                    }

                return Res;

            }
            catch (Exception Ex)
            {
               
                    if (ThrowAnyError == true)
                    {
                        throw Ex;
                    }
                
                return Res;
            }



        }


        internal static async Task<CallAsync_Results> CallGetAsync(string RelativePath, Boolean ThrowAnyError = false)
        {

            Uri requestUri = new Uri(GlobalVariables.BaseURLWS + @"api/" + RelativePath);
            return await CallGetAsync(requestUri, ThrowAnyError = false);
        }
        internal static async Task<CallAsync_Results> CallGetAsync(Uri RequestURI, Boolean ThrowAnyError = false)
        {

            CallAsync_Results Res = new CallAsync_Results();


            try
            {

                System.Net.Http.HttpClient objClint = new System.Net.Http.HttpClient();
                System.Net.Http.HttpResponseMessage respon = null;


                respon = await objClint.GetAsync(RequestURI);
                Res.Response = respon;
                Res.StatusCode = respon.StatusCode;
                if (respon != null)
                {
                    string responJsonText = await respon.Content.ReadAsStringAsync();
                    Res.ContentString_responJsonText = responJsonText;
                    if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //Dim serverResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responJsonText)
                        Res.Success = true;
                    }

                }

                return Res;

            }
            catch (Exception Ex)
            {

                if (ThrowAnyError == true)
                {
                    throw Ex;
                }

                return Res;
            }



        }


    }
}
