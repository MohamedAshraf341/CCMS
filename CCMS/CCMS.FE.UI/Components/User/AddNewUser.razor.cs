using CCMS.Common.Dto.Request.User;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CCMS.FE.UI.Components.User
{
    public partial class AddNewUser
    {
        [CascadingParameter] MudDialogInstance MudDialog {  get; set; }
        [Parameter] public AddUser Item { get; set; }
        void Submit()
        {
            MudDialog.Close(DialogResult.Ok(Item));
        }
        void Cancel() => MudDialog.Cancel();
    }
}