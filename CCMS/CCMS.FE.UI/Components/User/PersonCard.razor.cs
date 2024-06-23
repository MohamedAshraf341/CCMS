using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using CCMS.Common.Dto.Response;
using System;

namespace CCMS.FE.UI.Components.User
{
    public partial class PersonCard
    {
        [Inject] AuthenticationService AuthenticationService { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public string Style { get; set; }
        public TokenResponse? User { get; private set; }
        public string UserPicture { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            User = AuthenticationService.GetUser();
            if(User != null)
            {
                if (User.Picture != null && User.Picture.Length > 0)
                {
                    UserPicture = $"data:image/png;base64,{Convert.ToBase64String(User.Picture)}";
                }
                else
                {
                    UserPicture = "/images/default-user.jpg";
                }
            }
            
            await base.OnInitializedAsync();
        }

    }
}