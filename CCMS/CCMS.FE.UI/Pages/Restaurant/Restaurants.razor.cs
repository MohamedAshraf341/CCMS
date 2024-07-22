using CCMS.Common.Dto;
using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Request.Restaurant;
using CCMS.Common.Dto.Response.Reasturant;
using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Pages.Restaurant
{
    public partial class Restaurants
    {
        [Inject] Services.NotficationServices Notfication { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] ApiClient ApiClient { get; set; }
        [Inject] AuthenticationService authService { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        private bool Loading = false;

        private string searchString1 = "";
        private IEnumerable<RestaurantDto> Elements = new List<RestaurantDto>();
        private async Task LoadItems()
        {
            Loading = true;
            await InvokeAsync(StateHasChanged);
            try
            {
                try
                {
                    var res= await ApiClient.Restaurant.GetRestaurants();
                    if(res.Success)
                        Elements = res.Reasturants;
                    else
                        Notfication.ShowMessageError(res.Message);
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error($"Pages/Restaurant/Restaurants.LoadItems : : Unhandeled exception : {ex}");
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

        private bool FilterFunc1(RestaurantDto element) => FilterFunc(element, searchString1);

        private bool FilterFunc(RestaurantDto element, string searchString)
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
            var item = new AddOrEditRestaurant();
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
        private async Task EditResturant(RestaurantDto model)
        {
            var item =new AddOrEditRestaurant { Id = model.Id ,Name=model.Name};
            var parameters = new DialogParameters { { "Item", item } };
            var options = new DialogOptions { CloseButton = true, CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = DialogService.Show<Components.Restaurant.Add>("Add Reasturant", parameters, options);
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
        private async Task DeleteResturant(RestaurantDto item)
        {
            var result = await DialogService.ShowMessageBox("Delete Restaurant", $"Do you want delete {item.Name}?", yesText: "yes", cancelText: "No");
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
        private void NavigateToBranchPage(RestaurantDto item)
        {
            NavigationManager.NavigateTo($"/Branches?RestaurantId={item.Id}");
        }
    }
}