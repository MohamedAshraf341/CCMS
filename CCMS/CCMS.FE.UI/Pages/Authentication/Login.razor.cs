using CCMS.FE.UI.Extensions;
using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Serilog;
using System;
using System.Threading.Tasks;
using CCMS.Common.Dto.Request;
using CCMS.Common.Models;

namespace CCMS.FE.UI.Pages.Authentication
{
    public partial class Login
    {
        private string AlertMessage { get; set; }
        private bool ShowAlert { get; set; } = false;
        private bool loading;
        [Inject] Services.NotficationServices Notfication { get; set; }

        [Inject] ApiClient ApiClient { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] IDialogService DialogService { get; set; }

        LoginRequest Item = new LoginRequest();


        private async Task LoginAsync()
        {
            try
            {
                var res = await ApiClient.CanLogin(Item);

                if (res.Success)
                {
                    Guid key = Guid.NewGuid();
                    BlazorCookieLoginMiddleware.Logins[key] = res;
                    var returnUrl = NavigationManager.QueryString("returnUrl") ?? "/";
                    var notificationMessage = Uri.EscapeDataString(res.Message);
                    NavigationManager.NavigateTo($"/login?key={key}&returnUrl={returnUrl}&notificationMessage={notificationMessage}", true);
                }
                else
                {
                    AlertMessage = res.Message;
                    ShowAlert = true;
                }
            }
            catch (Exception ex)
            {
                Notfication.ShowMessageError(ex.Message);

                Log.Error($"Login.HandleValidSubmit :: Unhandled Exception : {ex}");
            }

            loading = false;
            StateHasChanged();
        }
    }
}