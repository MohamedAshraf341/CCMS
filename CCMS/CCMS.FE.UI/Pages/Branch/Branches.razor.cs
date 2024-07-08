using CCMS.Common.Dto;
using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Pages.Branch
{
    public partial class Branches
    {
        [Parameter] public string Model { get; set; }
        [Inject] NotficationServices Notfication { get; set; }
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
        private IEnumerable<BranchDto> Elements = new List<BranchDto>();
        private RestaurantDto Restaurant=new RestaurantDto();
        private async Task LoadItems()
        {
            Loading = true;
            await InvokeAsync(StateHasChanged);
            try
            {
                try
                {
                    Restaurant = System.Text.Json.JsonSerializer.Deserialize<RestaurantDto>(Model);
                    var user = authService.GetUser();
                    var req = new Common.Dto.Request.Branch.GetBranches
                    {
                        RestaurantId = Restaurant.Id,
                        UserId = user.SystemType == Common.Const.SystemType.Restaurant ? Guid.Parse(user.Id) : null,
                    };
                    var res = await ApiClient.Branche.GetBranches(req);
                    if (res.Success)
                        Elements = res.Branches;
                    else
                        Notfication.ShowMessageError(res.Message);
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error($"Pages/Branche/Branches.LoadItems : : Unhandeled exception : {ex}");
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
        private void NavigateToAddBranch()
        {
            
            NavigationManager.NavigateTo($"/AddBranch?Branch={Restaurant.Id}");
        }
    }
}