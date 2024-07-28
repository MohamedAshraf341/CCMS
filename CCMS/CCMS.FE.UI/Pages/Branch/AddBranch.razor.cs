using CCMS.Common.Dto;
using CCMS.Common.Dto.Request.Branch;
using CCMS.Common.Dto.Request.Phone;
using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Pages.Branch
{
    public partial class AddBranch
    {
        private Guid? RestaurantId { get; set; }
        [Inject] NotficationServices Notfication { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] ApiClient ApiClient { get; set; }
        [Inject] AuthenticationService authService { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        public IBrowserFile Browser;
        private void AddNewItem(List<AddOrEditPhone> items)
        {
            items.Add(new AddOrEditPhone());
        }

        private void DeleteItem(AddOrEditPhone item, List<AddOrEditPhone> items)
        {
            items.Remove(item);
        }

        AddOrEditBranche Item =new AddOrEditBranche();
        private IEnumerable<RestaurantDto> RestaurantElements = new List<RestaurantDto>();
        private async Task LoadItems()
        {
            try
            {
                Item.Phones = new List<AddOrEditPhone>() { new AddOrEditPhone() };
                var res = await ApiClient.Restaurant.GetRestaurants();
                if (res.Success)
                {
                    RestaurantElements = res.Reasturants;
                    if (RestaurantElements.Any())
                    {
                        Item.RestaurantId = RestaurantElements.First().Id;
                    }
                }
                else
                    Notfication.ShowMessageError(res.Message);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"Pages/Branche/AddBranch.LoadItems : : Unhandeled exception : {ex}");
                await DialogService.ShowMessageBox("Failed", $"Failed to get ?. Please contact hotline.", yesText: "Ok");
            }
        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            if (Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).TryGetValue("RestaurantId", out var restaurantId))
            {
                RestaurantId = Guid.Parse(restaurantId);
            }
            await LoadItems();
        }
        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            Browser = e.GetMultipleFiles().FirstOrDefault();
            using MemoryStream ms = new MemoryStream();
            await Browser.OpenReadStream(long.MaxValue).CopyToAsync(ms);
            var bytes = ms.ToArray();
            Item.Picture = bytes;
        }
        private void DeletePicture()
        {
            Browser = null;
            Item.Picture = null;
        }
        private async Task SaveChanges()
        {

        }
    }
}