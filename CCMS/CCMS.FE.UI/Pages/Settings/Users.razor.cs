using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Response;
using CCMS.FE.UI.Extensions;
using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MudBlazor.CategoryTypes;

namespace CCMS.FE.UI.Pages.Settings
{
    public partial class Users
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
        private IEnumerable<GetUsersResponse> Elements = new List<GetUsersResponse>();
        private async Task LoadItems()
        {
            Loading = true;
            await InvokeAsync(StateHasChanged);
            try
            {
                try
                {
                    var user = authService.GetUser();
                    var req=new GetUsersRequest { UserId=user.Id };
                    Elements = await ApiClient.Admin.GetUsers(req);
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error($"Pages/Settings/Users.LoadItems : : Unhandeled exception : {ex}");
                    await DialogService.ShowMessageBox("Failed",$"Failed to get ?. Please contact hotline.",yesText: "Ok");
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

        private string CalculateAge(DateTime dateOfBirth)
        {
            DateTime currentDate = DateTime.Today;
            int age = currentDate.Year - dateOfBirth.Year;

            // Check if the birthday for this year has not occurred yet
            if (dateOfBirth > currentDate.AddYears(-age))
            {
                age--;
            }

            return age.ToString();
        }
        private async Task AddUser()
        {
            var item = new AddUserRequest();
            var parameters = new DialogParameters { { "Item", item } };
            var options = new DialogOptions {CloseButton=true,CloseOnEscapeKey=true,MaxWidth=MaxWidth.Small,FullWidth=true };
            var dialog = DialogService.Show<Components.User.AddNewUser>("Add User", parameters, options);
            var result =await dialog.Result;
            if(!result.Cancelled)
            {
                var res = await ApiClient.Admin.AddUser(item);
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
        private async Task DeleteUser(GetUsersResponse item)
        {
            var result = await DialogService.ShowMessageBox("Delete User",$"Do you want delete {item.Email}?",yesText:"yes",cancelText:"No");
            if(result==true)
            {
                var res=await ApiClient.Admin.DeleteUser(item.UserId);
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
        private bool FilterFunc1(GetUsersResponse element) => FilterFunc(element, searchString1);

        private bool FilterFunc(GetUsersResponse element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if(element.DateOfBirth.HasValue)
            {
                if(CalculateAge(element.DateOfBirth.Value).Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if(string.Join(" , ", element.Roles).Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
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
    }
}