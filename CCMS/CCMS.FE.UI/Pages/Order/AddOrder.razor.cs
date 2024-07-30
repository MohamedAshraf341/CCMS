using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Response.Auth;

namespace CCMS.FE.UI.Pages.Order
{
    public partial class AddOrder
    {
        private Guid? BranchId { get; set; }
        private Guid? RestaurantId { get; set; }
        [Inject] NotficationServices? Notfication { get; set; }
        [Inject] NavigationManager? NavigationManager { get; set; }
        [Inject] ApiClient? ApiClient { get; set; }
        [Inject] AuthenticationService? authService { get; set; }
        [Inject] IDialogService? DialogService { get; set; }
        private IEnumerable<MenuItemDto> MenuItemsElements = new List<MenuItemDto>();
        Common.Dto.Request.Order.AddOrder Item = new Common.Dto.Request.Order.AddOrder();
        GetToken User = new GetToken();
        private Dictionary<Guid, int> selectedItems = new Dictionary<Guid, int>();

        private async Task LoadItems()
        {
            try
            {
                User = authService.GetUser();
                var req = new Common.Dto.Request.MenuItem.GetMenuItems
                {
                    BranchId = User.SystemType == Common.Const.SystemType.Restaurant ? User.BranchId : BranchId,
                };
                var res = await ApiClient.MenuItem.GetMenuItems(req);
                if (res.Success)
                    MenuItemsElements = res.MenuItems;
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"Pages/Order/AddOrder.LoadItems : : Unhandeled exception : {ex}");
                await DialogService.ShowMessageBox("Failed", $"Failed to get ?. Please contact hotline.", yesText: "Ok");
            }
        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            if (Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).TryGetValue("BranchId", out var branchIdId))
            {
                BranchId = Guid.Parse(branchIdId);
            }
            if (Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).TryGetValue("RestaurantId", out var restaurantId))
            {
                RestaurantId = Guid.Parse(restaurantId);
            }
            await LoadItems();
        }
        private async Task SaveChanges()
        {
            if (User.SystemType == Common.Const.SystemType.Restaurant)
                Item.BranchId = User.BranchId.Value;
            else if (BranchId.HasValue)
                Item.BranchId = BranchId.Value;
        }
        private string ConvertToBase64(byte[]? imageBytes)
        {
            return imageBytes != null ? $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}" : string.Empty;
        }
        private void OnAddItem(MenuItemDto item)
        {
            if (!selectedItems.ContainsKey(item.Id))
            {
                selectedItems[item.Id] = 1; // Default quantity is 1
            }
        }

        private void OnCancelItem(MenuItemDto item)
        {
            if (selectedItems.ContainsKey(item.Id))
            {
                selectedItems.Remove(item.Id); // Remove the item from selectedItems
                item.Number = 0; // Reset the item's number to 0
            }
        }
    }
}