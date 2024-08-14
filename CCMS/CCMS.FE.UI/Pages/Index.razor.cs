using Microsoft.AspNetCore.Components;

namespace CCMS.FE.UI.Pages
{
    public partial class Index
    {
        [Inject] Services.NotficationServices Notfication { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var notificationMessage = query["notificationMessage"];

            if (!string.IsNullOrEmpty(notificationMessage))
            {
                Notfication.ShowMessageSuccess(notificationMessage);
            }
            base.OnInitialized();
        }

    }
}