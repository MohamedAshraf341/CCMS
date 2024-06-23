using CCMS.Common.Dto.Request;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CCMS.FE.UI.Components.User
{
    public partial class AddNewUser
    {
        [CascadingParameter] MudDialogInstance MudDialog {  get; set; }
        [Parameter] public AddUserRequest Item { get; set; }
        void Submit()
        {
            MudDialog.Close(DialogResult.Ok(Item));
        }
        void Cancel() => MudDialog.Cancel();
    }
}