using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class CultureService
    {

        private readonly AuthenticationService _authenticationService;

        public CultureService(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task SetCultureFromLocalStorageAsync()
        {
            var localLang = await _authenticationService.GetUserLang();
            string culture;

            if (localLang == null || string.IsNullOrEmpty(localLang.Value))
            {
                culture = "en-US"; // Default if no culture is found
            }
            else
            {
                culture = localLang.Value;
            }

            var cultureInfo = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        }

    }

}
