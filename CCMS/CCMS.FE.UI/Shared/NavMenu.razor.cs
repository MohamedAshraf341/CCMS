using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Shared
{
    public partial class NavMenu
    {
        [Inject] AuthenticationService authService { get; set; }
        private Common.Dto.Response.Auth.GetToken User = new Common.Dto.Response.Auth.GetToken();
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadItems();
        }
        private async Task LoadItems()
        {
            var user = authService.GetUser();
            if (user != null)
            {
                User = user;
            }
        }

     }
}