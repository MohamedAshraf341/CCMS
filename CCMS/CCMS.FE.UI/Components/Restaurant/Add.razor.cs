using CCMS.Common.Dto.Request.Restaurant;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CCMS.FE.UI.Components.Restaurant
{
    public partial class Add
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public AddOrEditRestaurant Item { get; set; }
        void Submit()
        {
            MudDialog.Close(DialogResult.Ok(Item));
        }
        void Cancel() => MudDialog.Cancel();
    }
}