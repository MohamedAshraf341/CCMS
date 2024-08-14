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
        [Inject] ApiClient? ApiClient { get; set; }
        private bool _isDarkMode;

        private MudTheme _currentTheme = new MudBlazorAdminDashboard();
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var darkMode = await ApiClient.AppSetting.GetById(Common.Const.AppSetting.DarkMode.Id);

            if (darkMode != null && AppSettingHelper.GetValue<bool>(darkMode))
                _isDarkMode = true;
            else
                _isDarkMode = false;
        }
    }
}