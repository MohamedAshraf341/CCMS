using CCMS.Common.Dto.Request.Auth;
using CCMS.Common.Dto.Response.Auth;
using Microsoft.AspNetCore.Components;
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
        private readonly NavigationManager navigationManager;
        public ApiClient_Account Account { get => Get<ApiClient_Account>().Result; }
        public ApiClient_Admin Admin { get => Get<ApiClient_Admin>().Result; }
        public ApiClient_Restaurant Restaurant { get => Get<ApiClient_Restaurant>().Result; }
        public ApiClient_Branche Branche { get => Get<ApiClient_Branche>().Result; }
        public ApiClient_BranchPhone BranchPhone { get => Get<ApiClient_BranchPhone>().Result; }
        public ApiClient_Order Order { get => Get<ApiClient_Order>().Result; }
        public ApiClient_Client Client { get => Get<ApiClient_Client>().Result; }
        public ApiClient_MenuItem MenuItem { get => Get<ApiClient_MenuItem>().Result; }
        public ApiClient_AppSetting AppSetting { get => Get<ApiClient_AppSetting>().Result; }
        public ApiClient_UserSetting UserSetting { get => Get<ApiClient_UserSetting>().Result; }

        public ApiClient(IOptions<AppSettings> _appSettings, AuthenticationService _authService, NavigationManager _navigationManager)
        {
            backendUrl = _appSettings?.Value?.BackendUrl;
            if (string.IsNullOrEmpty(backendUrl))
                Log.Error("ApiClient.ApiClient BackendUrl not defined in AppSettings.");

            authService = _authService;
            navigationManager = _navigationManager;
        }

        private async Task updateClientAuthHeader()
        {
            if (client == null)
                client = new HttpClient { BaseAddress = new Uri(backendUrl + "api/") };

            var user = authService.GetUser();

            if (user != null)
            {
                if (user.ExpiresOn <= DateTime.Now)
                {
                    if(user.RefreshTokenExpiration <= DateTime.Now)
                    {
                        var req = new RefreshToken { Token = user.RefreshToken };
                        await Account.LogOut(req);
                        navigationManager.NavigateTo("/logout", true);
                    }
                    else
                    {
                        var res = await Account.RefreshToken(new RefreshToken { Token = user.RefreshToken });
                        authService.DeleteUser();
                        authService.SetUser(res);
                        user = res;
                    }
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            }
            else
            {
                client.DefaultRequestHeaders.Authorization = null;
            }
        }

        private async Task<T?> Get<T>() where T : ApiClientBase
        {
            var type = typeof(T);
            if (apis.ContainsKey(type))
                return apis[type] as T;

            if (client == null)
                await updateClientAuthHeader();
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
