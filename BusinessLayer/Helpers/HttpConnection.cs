using PayStackDotNetSDK.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BusinessLayer.Helpers
{
    public static class HttpConnection
    {/// <summary>
     /// Initialize a new HttpClient with Security, content and cache configuragions. 
     /// </summary>
     /// <param name="secretKey"></param>
     /// <returns></returns>
        public static HttpClient CreateClient(string secretKey)
        {
            // ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var client = new HttpClient()
            {
                BaseAddress = new Uri(Constants.PaystackBaseURL)
            };


            client.DefaultRequestHeaders.Clear();

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(Constants.ContentTypeHeaderJson));

            client.DefaultRequestHeaders.Add("cache-control", "no-cache");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.AuthorizationHeaderType, secretKey);

            return client;
        }
    }

}
