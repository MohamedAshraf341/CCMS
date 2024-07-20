using CCMS.Common.Dto;
using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Pages.Client
{
    public partial class Clients
    {
        [Inject] NotficationServices Notfication { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] ApiClient ApiClient { get; set; }
        [Inject] AuthenticationService authService { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        private Guid? BranchId {  get; set; }
        private bool Loading = false;
        private string searchString1 = "";
        private IEnumerable<CustomerDto> Elements = new List<CustomerDto>();
        private async Task LoadItems()
        {
            Loading = true;
            await InvokeAsync(StateHasChanged);
            try
            {
                try
                {
                    var user = authService.GetUser();

                    var req = new Common.Dto.Request.Client.GetClients
                    {
                        BranchId = user.SystemType == Common.Const.SystemType.Restaurant ? user.BranchId: BranchId,
                    };
                    var res = await ApiClient.Client.GetClients(req);
                    if (res.Success)
                        Elements = res.Customers;
                    else
                        Notfication.ShowMessageError(res.Message);
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error($"Pages/Client/Clients.LoadItems : : Unhandeled exception : {ex}");
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
        private bool FilterFunc1(CustomerDto element) => FilterFunc(element, searchString1);

        private bool FilterFunc(CustomerDto element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Phone.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.CountOrder.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
    }
}