using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;

namespace CCMS.FE.UI.Shared
{
    public partial class NavMenu
    {
        [Inject] AuthenticationService authService { get; set; }

    }
}