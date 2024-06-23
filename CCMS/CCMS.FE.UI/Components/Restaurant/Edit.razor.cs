using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Response;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CCMS.FE.UI.Components.Restaurant
{
    public partial class Edit
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public GetReasturantResponse Item { get; set; }
        void Submit()
        {
            MudDialog.Close(DialogResult.Ok(Item));
        }
        void Cancel() => MudDialog.Cancel();
    }
}