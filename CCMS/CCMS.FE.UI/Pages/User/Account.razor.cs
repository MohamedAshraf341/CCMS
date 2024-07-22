using CCMS.Common.Dto;
using CCMS.Common.Dto.Request.User;
using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Pages.User
{
    public partial class Account
    {
        [Inject] NotficationServices Notfication { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] ApiClient ApiClient { get; set; }
        [Inject] AuthenticationService authService { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        public IBrowserFile Browser;
        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            Browser = e.GetMultipleFiles().FirstOrDefault();
            using MemoryStream ms = new MemoryStream();
            await Browser.OpenReadStream(long.MaxValue).CopyToAsync(ms);
            var bytes = ms.ToArray();
            User.Picture = bytes;
        }
        public Color AvatarButtonColor { get; set; } = Color.Error;

       UsersDto User = new UsersDto();

        private async Task LoadItems()
        {
            try
            {
                var currentUser = authService.GetUser();
                if (currentUser != null)
                {
                    var item= await ApiClient.Account.GetUserById(currentUser.Id);
                    if (item != null)
                    {
                        User= item;
                    }
                    
                }

            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"Pages/Branche/Branches.LoadItems : : Unhandeled exception : {ex}");
                await DialogService.ShowMessageBox("Failed", $"Failed to get ?. Please contact hotline.", yesText: "Ok");
            }
        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadItems();
        }
        void DeletePicture()
        {
            Browser = null;
            User.Picture = null;
        }

        private async Task  SaveChangesGeneral()
        {
            var item=new UpdateUser { UserId=User.UserId,Name=User.Name,DateOfBirth=User.DateOfBirth, Picture = User.Picture,phone=User.phone};
            var res =await ApiClient.Account.UpdateUser(item);
            if(res != null)
            {
                if(res.Success)
                    Notfication.ShowMessageSuccess(res.Message);
                else
                    Notfication.ShowMessageError(res.Message);
            }
        }
        private void SaveChangesSecurity()
        {
            
        }
        MudForm form;
        MudTextField<string> pwField1;

        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
        }

        private string PasswordMatch(string arg)
        {
            if (pwField1.Value != arg)
                return "Passwords don't match";
            return null;
        }
    }
}