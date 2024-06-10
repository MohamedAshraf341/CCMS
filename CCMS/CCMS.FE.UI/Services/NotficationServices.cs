using MudBlazor;

namespace CCMS.FE.UI.Services
{
    public class NotficationServices
    {
        private readonly ISnackbar _Snackbar;
        public NotficationServices(ISnackbar snackbar)
        {
            _Snackbar = snackbar;
        }
        public void ShowMessageError(string Msssage)
        {
            _Snackbar.Clear();
            _Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
            _Snackbar.Add(Msssage, Severity.Error);
        }
        public void ShowMessageSuccess(string Msssage)
        {
            _Snackbar.Clear();
            _Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
            _Snackbar.Add(Msssage, Severity.Success);
        }
    }

}
