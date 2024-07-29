using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using CCMS.Common.Dto.Request.Phone;
using CCMS.Common.Dto;
using static MudBlazor.CategoryTypes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CCMS.Common.Dto.Request.MenuItem;
using CCMS.Common.Dto.Response.Auth;
using System.IO;

namespace CCMS.FE.UI.Pages.MenuItem
{
    public partial class AddMenuItem
    {
        private Guid? BranchId { get; set; }
        private Guid? RestaurantId { get; set; }

        [Inject] NotficationServices? Notfication { get; set; }
        [Inject] NavigationManager? NavigationManager { get; set; }
        [Inject] ApiClient? ApiClient { get; set; }
        [Inject] AuthenticationService? authService { get; set; }
        [Inject] IDialogService? DialogService { get; set; }
        public IBrowserFile? Browser;
        private IEnumerable<BranchDto> BranchElements = new List<BranchDto>();
        AddOrEditMenuItem Item = new AddOrEditMenuItem();
        GetToken User=new GetToken();
        private async Task LoadItems()
        {
            try
            {
                User = authService.GetUser();
                var req = new Common.Dto.Request.Branch.GetBranches() { RestaurantId= RestaurantId };
                var res = await ApiClient.Branche.GetBranches(req);
                if (res.Success)
                {
                    BranchElements = res.Branches;
                    if (BranchElements.Any())
                    {
                        Item.BranchId = BranchElements.First().Id;
                    }
                }
                else
                    Notfication.ShowMessageError(res.Message);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"Pages/MenuItem/AddMenuItem.LoadItems : : Unhandeled exception : {ex}");
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
            if (User.SystemType == Common.Const.SystemType.Restaurant)
                Item.BranchId = User.BranchId.Value;
            else if(BranchId.HasValue)
                Item.BranchId = BranchId.Value;
            var res = await ApiClient.MenuItem.AddMenuItem(Item);
            if (res != null)
            {
                if(res.Success)
                    Notfication.ShowMessageSuccess(res.Message);
                else
                    Notfication.ShowMessageError(res.Message);
            }
        }
    }
}