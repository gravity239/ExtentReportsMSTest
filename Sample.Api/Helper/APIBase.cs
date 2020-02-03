using Microsoft.IdentityModel.Clients.ActiveDirectory;
using RestSharp;
using Sample.API.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Api.Helper
{
    public class APIBase
    {
        const string armUri = "https://management.azure.com/";
        const string aadInstance = "https://login.microsoftonline.com/{0}/oauth2/token";
        const string tenant = "domain.onmicrosoft.com";

        protected string SendHttpCall(string accessToken, string baseUrl, string operationPath, Method method, Dictionary<string, string> headerParameters = null, Dictionary<string, string> queryParameters = null, object postBody = null)
        {
            var client = new RestClient(baseUrl);
            string uri = string.Format("{0}/{1}", baseUrl, operationPath);
            var request = new RestRequest(new Uri(uri), method);

            //Header
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + accessToken);

            if(headerParameters != null && headerParameters.Count > 0)
            {
                foreach(var item in headerParameters)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }

            //Parameter
            request.AddParameter("cache-control", "no-cache");
            if (queryParameters != null && queryParameters.Count > 0)
            {
                foreach (var item in queryParameters)
                {
                    request.AddQueryParameter(item.Key, item.Value);
                }
            }

            //Body
            if (postBody != null)
            {
                request.JsonSerializer = NewtonsoftJsonSerializer.Default;
                if(postBody is string)
                {
                    request.AddParameter("application/json", postBody, ParameterType.RequestBody);
                }
                else
                    request.AddJsonBody(postBody);
                //request.AddJsonBody(Newtonsoft.Json.JsonConvert.SerializeObject(postBody));
            }
            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(response.ErrorMessage);
            }

            return response.Content;

        }

        public static async Task<string> GetAccessTokenAsync(string clientId, string appKey, string resourceId)
        {

            string authority = string.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

            var authContext = new AuthenticationContext(authority);

            ClientCredential clientCredential = new ClientCredential(clientId, appKey);

            AuthenticationResult authenticationResult = null;
            authenticationResult = await authContext.AcquireTokenAsync(resourceId, clientCredential);

            return authenticationResult.AccessToken;

        }


    }
}
