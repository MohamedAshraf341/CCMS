using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request.Auth;
using CCMS.Common.Enums;
using CCMS.Common.Helpers;
using CCMS.Common.Resources;
using CCMS.FE.UI.Services;
using CCMS.FE.UI.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.ThemeManager;
using Serilog;

namespace CCMS.FE.UI.Shared
{
    public partial class MainLayout : IDisposable, IAsyncDisposable
    {
        [CascadingParameter]
        public Task<bool> CultureInitialized { get; set; }

        private bool isCultureInitialized = false;

        protected override async Task OnParametersSetAsync()
        {
            if (CultureInitialized is not null)
            {
                isCultureInitialized = await CultureInitialized;
            }
        }

        [Inject] IStringLocalizer<SharedResources> sharedResources { get; set; }


        private HubConnection hubConnection;
        [Inject]   public IJSRuntime _jSRuntime { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        [Inject] Services.NotficationServices Notfication { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] AuthenticationService AuthenticationService { get; set; }
        [Inject] ApiClient? ApiClient { get; set; }
        [Inject] IOptions<AppSettings> AppSettings { get; set; } // Inject IOptions<AppSettings>
        private  string backendUrl;
        [Inject] CultureService cultureService { get; set; }
        private IEnumerable<OrderDto> OrderElements = new List<OrderDto>();
        private Common.Dto.Response.Auth.GetToken User = new Common.Dto.Response.Auth.GetToken();
        private int CountOrder;
        Common.Dto.UserSettingDto DarkModeItem {  get; set; }
        Common.Dto.UserSettingDto LangItem { get; set; }

        private bool _isDarkMode {  get; set; }
        private string DarkModeText { get; set; }
        private bool _isRTL { get; set; }


        private ThemeManagerTheme _themeManager = new ThemeManagerTheme();
        private List<BreadcrumbItem> _items = new List<BreadcrumbItem> { new BreadcrumbItem("Home", href: "/") };

        public bool _drawerOpen = true;
        public bool _themeManagerOpen = false;

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        void OpenThemeManager(bool value)
        {
            _themeManagerOpen = value;
        }

        private async Task ToggleDarkMode()
        {
            _isDarkMode = !_isDarkMode;
            if(_isDarkMode)
                DarkModeText = sharedResources[Common.Keys.DashBoard.NavBar.LightMode];
            else
                DarkModeText = sharedResources[Common.Keys.DashBoard.NavBar.DarkMode];

            if (DarkModeItem == null)
            {
                DarkModeItem = new UserSettingDto { Key = Common.Enums.Settings.DarkMode.ToString(), UserId=User.Id,};
                UserSettingHelper.SetValue(DarkModeItem, _isDarkMode);
                var res=await ApiClient.UserSetting.Add(DarkModeItem);
                if(res)
                {
                    await AuthenticationService.DeleteUserMode();
                    await AuthenticationService.SetUserMode(DarkModeItem);
                }

            }
            else
            {
                UserSettingHelper.SetValue(DarkModeItem, _isDarkMode);
                var res = await ApiClient.UserSetting.Edit(DarkModeItem);
                if (res)
                {
                    await AuthenticationService.DeleteUserMode();
                    await AuthenticationService.SetUserMode(DarkModeItem);
                }
            }
        }
        private async Task SetLanguage(LanguageCode languageCode)
        {
            // Determine if the language is RTL or LTR
            _isRTL = languageCode == LanguageCode.Arabic_EG;

            var newLangValue = LanguageCodeExtensions.ToCultureString(languageCode);

            if (LangItem != null && LangItem.Value != newLangValue)
            {
                // Update the existing language setting
                UserSettingHelper.SetValue(LangItem, newLangValue);
                var res = await ApiClient.UserSetting.Edit(LangItem);

                if (res)
                {
                    await AuthenticationService.DeleteUserLang();
                    await AuthenticationService.SetUserLang(LangItem);

                }
            }
            else if (LangItem == null)
            {
                // Create a new language setting
                LangItem = new UserSettingDto
                {
                    Key = Settings.Language.ToString(),
                    UserId = User.Id,
                };

                UserSettingHelper.SetValue(LangItem, newLangValue);
                var res = await ApiClient.UserSetting.Add(LangItem);

                if (res)
                {
                    await AuthenticationService.DeleteUserLang();
                    await AuthenticationService.SetUserLang(LangItem);

                }
            }

            // Reload the page to apply changes
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }


        void UpdateTheme(ThemeManagerTheme value)
        {
            _themeManager = value;
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            User = AuthenticationService.GetUser();

            var darkMode = await ApiClient.UserSetting.GetByUserAndKey(new Common.Dto.Request.UserSetting.GetByUserAndKey { UserId=User.Id,Key= Settings.DarkMode.ToString()});
            var langUser = await ApiClient.UserSetting.GetByUserAndKey(new Common.Dto.Request.UserSetting.GetByUserAndKey { UserId = User.Id, Key = Settings.Language.ToString() });
            DarkModeItem = darkMode;
            LangItem = langUser;
            if (langUser != null && langUser.Value== LanguageCodeExtensions.ToCultureString(LanguageCode.Arabic_EG))
            {
                _isRTL = true;
            }
            else
                _isRTL= false;

            if (darkMode != null )
            {
               
                var valueDarkMode = UserSettingHelper.GetValue<bool>(darkMode);
                if (valueDarkMode)
                {
                    _isDarkMode = true;
                    DarkModeText = sharedResources[Common.Keys.DashBoard.NavBar.LightMode];
                }
                else
                {
                    _isDarkMode = false;
                    DarkModeText = sharedResources[Common.Keys.DashBoard.NavBar.DarkMode];

                }
            }
            else
            {
                _isDarkMode = false;
                DarkModeText = sharedResources[Common.Keys.DashBoard.NavBar.DarkMode];

            }
            if (User != null && User.SystemType == Common.Const.SystemType.Restaurant)
            {
                await LoadItems();
                backendUrl = AppSettings?.Value?.BackendUrl;
                if (string.IsNullOrEmpty(backendUrl))
                {
                    Log.Error("MainLayout.MainLayout BackendUrl not defined in AppSettings.");
                }
                hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{backendUrl}orderHub") // Backend URL
                    .Build();

                hubConnection.On("ReceiveOrderUpdate", async () =>
                {
                    await LoadItems();
                    StateHasChanged();
                });

                try
                {
                    await hubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    Log.Error("Error starting SignalR connection: " + ex.Message);
                    Log.Error("Exception details: " + ex.ToString());
                    
                }
            }

            _themeManager.Theme = new MudBlazorAdminDashboard();
            _themeManager.DrawerClipMode = DrawerClipMode.Always;
            _themeManager.FontFamily = "Montserrat";
            _themeManager.DefaultBorderRadius = 3;
            NavigationManager.LocationChanged += LocationChanged;
        }

        private async Task LoadItems()
        {
            
            if (User != null && User.SystemType == Common.Const.SystemType.Restaurant)
            {
                var reqOrder = new Common.Dto.Request.Order.GetOrders
                {
                    BranchId = User.BranchId,
                    Confirmed = false,
                };
                var resOrder = await ApiClient.Order.GetOrders(reqOrder);
                if (resOrder != null && resOrder.Success)
                {
                    OrderElements = resOrder.Orders;
                    CountOrder = OrderElements.Count();
                }
            }
        }

        private async Task OnClickLogout()
        {
            var user = AuthenticationService.GetUser();
            if (user != null)
            {
                var req = new RefreshToken { Token = user.RefreshToken };
                await ApiClient.Account.LogOut(req);
            }
            NavigationManager.NavigateTo("/logout", true);
        }

        private void LocationChanged(object sender, LocationChangedEventArgs e)
        {
            var location = NavigationManager.ToBaseRelativePath(e.Location);
            var segments = location.Split('/').Where(s => !string.IsNullOrEmpty(s)).ToArray();

            _items.Clear();
            _items.Add(new BreadcrumbItem("Home", href: "/"));

            for (int i = 0; i < segments.Length; i++)
            {
                var text = segments[i];
                text = char.ToUpper(text[0]) + text.Substring(1);
                var href = "/" + string.Join('/', segments.Take(i + 1));

                _items.Add(new BreadcrumbItem(text, href: href));
            }

            StateHasChanged();
        }
        private async Task OnOrderDetails(OrderDto Order)
        {
            var ConfirmOrder = new Common.Dto.Request.Order.ConfirmOrder { ReceivedUser=User.Id,Id=Order.Id };
            var parameters = new DialogParameters { { "Order", Order },{ "ConfirmOrder", ConfirmOrder },{ "ConfirmButton",true} };
            var options = new DialogOptions { CloseButton = true, CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };
            var dialog = DialogService.Show<Components.Order.Details>("Details Order", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var res = await ApiClient.Order.ConfirmOrder(ConfirmOrder);
                if (res.Success)
                {
                    Notfication.ShowMessageSuccess(res.Message);
                   await LoadItems();
                }
                else
                {
                    await Notfication.ShowMessageError(res.Message);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (hubConnection != null)
            {
                await hubConnection.DisposeAsync();
            }
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= LocationChanged;

        }
    }
}
