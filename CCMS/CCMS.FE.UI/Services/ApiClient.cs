using CCMS.Common.Dto.Request.Auth;
using CCMS.Common.Dto.Response.Auth;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class ApiClient : IDisposable
    {
        private bool disposedValue;
        private readonly string backendUrl;
        private HttpClient client;
        private ApiHttpClient apiHttpClient;
        readonly Dictionary<Type, ApiClientBase> apis = new();
        private readonly AuthenticationService authService;
        public ApiClient_Account Account { get => Get<ApiClient_Account>(); }
        public ApiClient_Admin Admin { get => Get<ApiClient_Admin>(); }
        public ApiClient_Restaurant Restaurant { get => Get<ApiClient_Restaurant>(); }
        public ApiClient_Branche Branche { get => Get<ApiClient_Branche>(); }
        public ApiClient_BranchPhone BranchPhone { get => Get<ApiClient_BranchPhone>(); }


        public ApiClient(IOptions<AppSettings> _appSettings, AuthenticationService _authService)
        {
            backendUrl = _appSettings?.Value?.BackendUrl;
            if (string.IsNullOrEmpty(backendUrl))
                Log.Error("ApiClient.ApiClient BackendUrl not defined in AppSettings.");

            authService = _authService;

        }
        void updateClientAuthHeader()
        {
            if (client == null)
                client = new HttpClient { BaseAddress = new Uri(backendUrl) };

            var user = authService.GetUser();

            if (user != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            else
                client.DefaultRequestHeaders.Authorization = null;
        }
        private T Get<T>() where T : ApiClientBase
        {
            var type = typeof(T);
            if (apis.ContainsKey(type))
                return apis[type] as T;

            if (client == null)
                updateClientAuthHeader();
            if (apiHttpClient == null)
                apiHttpClient = new ApiHttpClient(client);

            var context = Activator.CreateInstance(type, apiHttpClient) as T;

            apis.Add(type, context);

            return context;
        }
        public async Task<GetToken> CanLogin(Login model)
        {
            var loginResponse = await Account.Login(model);
            return loginResponse;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (client != null)
                        client.Dispose();
                    client = null;
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
