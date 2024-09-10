using CCMS.Common.Enums;
using CCMS.Common.Helpers;
using CCMS.FE.UI.Services;
using CCMS.FE.UI.Theme;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Shared
{
    public partial class LoginLayout
    {
        [Inject] AuthenticationService AuthenticationService { get; set; }

        private MudTheme _currentTheme = new MudBlazorAdminDashboard();
        private bool _isDarkMode { get; set; }
        private bool _isRTL { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var darkMode =await AuthenticationService.GetUserMode();
            var langUser =await AuthenticationService.GetUserLang();
            if (langUser != null && langUser.Value == LanguageCodeExtensions.ToCultureString(LanguageCode.Arabic_EG))
                _isRTL = true;
            else
                _isRTL = false;

            if (darkMode != null && UserSettingHelper.GetValue<bool>(darkMode))
                _isDarkMode = true;
            else
                _isDarkMode = false;

        }
    }
}