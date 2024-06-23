using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Response;
using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Pages
{
    public partial class Restaurants
    {
        [Inject] Services.NotficationServices Notfication { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] ApiClient ApiClient { get; set; }
        [Inject] AuthenticationService authService { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        private bool Loading = false;
        private bool dense = true;
        private bool hover = false;
        private bool striped = true;
        private bool bordered = false;
        private string searchString1 = "";
        private IEnumerable<GetReasturantResponse> Elements = new List<GetReasturantResponse>();
        private async Task LoadItems()
        {
            Loading = true;
            await InvokeAsync(StateHasChanged);
            try
            {
                try
                {
                    Elements = await ApiClient.Restaurant.GetRestaurants();
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error($"Pages/RestaurantPage.LoadItems : : Unhandeled exception : {ex}");
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
            await LoadItems();
        }
        private MarkupString GetHighlightedText(string text)
        {
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

        private bool FilterFunc1(GetReasturantResponse element) => FilterFunc(element, searchString1);

        private bool FilterFunc(GetReasturantResponse element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.BranchCount.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        private async Task AddResturant()
        {
            var item = new AddReasturantRequest();
            var parameters = new DialogParameters { { "Item", item } };
            var options = new DialogOptions { CloseButton = true, CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = DialogService.Show<Components.Restaurant.Add>("Add Reasturant", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var res = await ApiClient.Restaurant.AddRestaurant(item);
                if (res.Success)
                {
                    Notfication.ShowMessageSuccess(res.Message);
                    _ = LoadItems();
                }
                else
                {
                    Notfication.ShowMessageError(res.Message);
                }
            }
        }
        private async Task EditResturant(GetReasturantResponse item)
        {
            var parameters = new DialogParameters { { "Item", item } };
            var options = new DialogOptions { CloseButton = true, CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = DialogService.Show<Components.Restaurant.Edit>("Add Reasturant", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var res = await ApiClient.Restaurant.EditRestaurant(item);
                if (res.Success)
                {
                    Notfication.ShowMessageSuccess(res.Message);
                    _ = LoadItems();
                }
                else
                {
                    Notfication.ShowMessageError(res.Message);
                }
            }
        }
        private async Task DeleteResturant(GetReasturantResponse item)
        {
            var result = await DialogService.ShowMessageBox("Delete User", $"Do you want delete {item.Name}?", yesText: "yes", cancelText: "No");
            if (result == true)
            {
                var res = await ApiClient.Restaurant.DeleteRestaurant(item.Id);
                if (res.Success)
                {
                    Notfication.ShowMessageSuccess(res.Message);
                    _ = LoadItems();
                }
                else
                {
                    Notfication.ShowMessageError(res.Message);
                }
            }
        }
    }
}