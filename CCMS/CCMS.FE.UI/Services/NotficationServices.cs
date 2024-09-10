using CCMS.Common.Enums;
using CCMS.Common.Helpers;
using MudBlazor;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Services
{
    public class NotficationServices
    {
        private readonly ISnackbar _Snackbar;
        private readonly AuthenticationService _authenticationService;
        public NotficationServices(ISnackbar snackbar, AuthenticationService authenticationService)
        {
            _Snackbar = snackbar;
            _authenticationService = authenticationService;
        }
        public async Task ShowMessageError(string? Msssage)
        {
            _Snackbar.Clear();
            var lang =await _authenticationService.GetUserLang();
            if(lang!= null&&lang.Value == LanguageCodeExtensions.ToCultureString(LanguageCode.Arabic_EG))
                _Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopLeft;
            else
                _Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;

            _Snackbar.Add(Msssage, Severity.Error);
        }
        public async void ShowMessageSuccess(string? Msssage)
        {
            _Snackbar.Clear();
            var lang = await _authenticationService.GetUserLang();
            if (lang != null && lang.Value == LanguageCodeExtensions.ToCultureString(LanguageCode.Arabic_EG))
                _Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopLeft;
            else
                _Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
            _Snackbar.Add(Msssage, Severity.Success);
        }
    }

}
