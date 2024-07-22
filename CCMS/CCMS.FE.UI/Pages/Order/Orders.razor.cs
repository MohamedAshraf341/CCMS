using CCMS.Common.Dto;
using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Globalization;
using static MudBlazor.FilterOperator;

namespace CCMS.FE.UI.Pages.Order
{
    public partial class Orders
    {
        [Inject] NotficationServices Notfication { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] ApiClient ApiClient { get; set; }
        [Inject] AuthenticationService authService { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        private Guid? BranchId { get; set; }
        private bool Loading = false;
        private string searchString1 = "";
        private IEnumerable<OrderDto> Elements = new List<OrderDto>();
        private Common.Dto.Response.Auth.GetToken User=new Common.Dto.Response.Auth.GetToken();
        private async Task LoadItems()
        {
            Loading = true;
            await InvokeAsync(StateHasChanged);
            try
            {
                try
                {
                    User = authService.GetUser();

                    var req = new Common.Dto.Request.Order.GetOrders
                    {
                        BranchId = User.SystemType == Common.Const.SystemType.Restaurant ? User.BranchId : BranchId,
                       
                        
                    };
                    if(authService.UserHasOneOfRole(Common.Const.Roles.User.Name))
                    {
                        req.CreatedBy = User.SystemType == Common.Const.SystemType.System ? User.Id : null;
                        req.ReceivedBy = User.SystemType == Common.Const.SystemType.Restaurant ? User.Id : null;
                    }
                    var res = await ApiClient.Order.GetOrders(req);
                    if (res.Success)
                        Elements = res.Orders;
                    else
                        Notfication.ShowMessageError(res.Message);
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error($"Pages/Order/Orders.LoadItems : : Unhandeled exception : {ex}");
                    await DialogService.ShowMessageBox("Failed", $"Failed to get ?. Please contact hotline.", yesText: "Ok");
                }
            }
            finally
            {
                Loading = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            if (Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).TryGetValue("BranchId", out var branchId))
            {
                BranchId = Guid.Parse(branchId);
            }
            await LoadItems();
        }
        private MarkupString GetHighlightedText(string text)
        {
            // Check if text or searchString1 is null
            if (text == null)
            {
                return new MarkupString(string.Empty);
            }
            if (string.IsNullOrEmpty(searchString1))
                return new MarkupString(text);

            var index = text.IndexOf(searchString1, StringComparison.OrdinalIgnoreCase);
            if (index == -1)
                return new MarkupString(text);

            var highlighted = text.Substring(0, index) +
                              $"<span style='background-color: yellow'>{text.Substring(index, searchString1.Length)}</span>" +
                              text.Substring(index + searchString1.Length);

            return new MarkupString(highlighted);
        }
        private bool FilterFunc1(OrderDto element) => FilterFunc(element, searchString1);

        private bool FilterFunc(OrderDto element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.CustomerName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.CustomerPhone.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.CustomerAddress.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if(User.SystemType == Common.Const.SystemType.System && authService.UserHasOneOfRole(Common.Const.Roles.Admin.Name, Common.Const.Roles.SuperAdmin.Name))
            {
                if (element.CreatedName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (User.SystemType==Common.Const.SystemType.System )
            {               
                if (element.Restaurant.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (User.SystemType == Common.Const.SystemType.System || authService.UserHasOneOfRole(Common.Const.Roles.Admin.Name))
            {
                if (element.ReceivedName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (FormatTime(element.CreationDate).Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
        private string FormatTime(System.DateTime dateTime)
        {


            return dateTime.ToString("MM/dd/yyyy h:mm tt");
        }
    }
}