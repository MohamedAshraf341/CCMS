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
        [Parameter] public Guid? RestaurantId { get; set; }
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
        private async Task LoadItems()
        {
            Loading = true;
            await InvokeAsync(StateHasChanged);
            try
            {
                try
                {
                    var user = authService.GetUser();
                    var req = new Common.Dto.Request.Branch.GetBranches
                    {
                        RestaurantId = RestaurantId,
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
            if(RestaurantId.HasValue)
                NavigationManager.NavigateTo($"/AddBranch?RestaurantId={RestaurantId}");
            else
                NavigationManager.NavigateTo($"/AddBranch");

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
        private bool FilterFunc1(BranchDto element) => FilterFunc(element, searchString1);

        private bool FilterFunc(BranchDto element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Reasturant.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (string.Join(" , ", element.Moderators).Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (GetFormattedAddress(element.Area, element.City, element.Government).Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

    }
}