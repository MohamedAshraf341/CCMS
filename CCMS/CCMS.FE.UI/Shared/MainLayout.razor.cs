using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request.Auth;
using CCMS.Common.Helpers;
using CCMS.FE.UI.Services;
using CCMS.FE.UI.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudBlazor.ThemeManager;
using Serilog;

namespace CCMS.FE.UI.Shared
{
    public partial class MainLayout : IDisposable, IAsyncDisposable
    {
        private HubConnection hubConnection;
        [Inject] IDialogService DialogService { get; set; }
        [Inject] Services.NotficationServices Notfication { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] AuthenticationService AuthenticationService { get; set; }
        [Inject] ApiClient? ApiClient { get; set; }
        private IEnumerable<OrderDto> OrderElements = new List<OrderDto>();
        private Common.Dto.Response.Auth.GetToken User = new Common.Dto.Response.Auth.GetToken();
        private int CountOrder;
        Common.Dto.AppSettingDto DarkModeItem {  get; set; } 
        private bool _isDarkMode {  get; set; }

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
            AppSettingHelper.SetValue(DarkModeItem, _isDarkMode);
            await ApiClient.AppSetting.Edit(DarkModeItem);
        }

        void UpdateTheme(ThemeManagerTheme value)
        {
            _themeManager = value;
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var darkMode = await ApiClient.AppSetting.GetById(Common.Const.AppSetting.DarkMode.Id);
            if (darkMode != null )
            {
                DarkModeItem=darkMode;
                if(AppSettingHelper.GetValue<bool>(darkMode))
                    _isDarkMode = true;
                else
                    _isDarkMode = false;
            }
            else
                _isDarkMode = false;
            User = AuthenticationService.GetUser();
            if (User != null && User.SystemType == Common.Const.SystemType.Restaurant)
            {
                await LoadItems();
                hubConnection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:5000/orderHub") // Backend URL
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
                    Notfication.ShowMessageError(res.Message);
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
