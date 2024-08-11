using CCMS.Common.Dto.Request.Order;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CCMS.FE.UI.Components.Order
{
    public partial class Details
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public Common.Dto.OrderDto Order { get; set; }
        [Parameter] public Common.Dto.Request.Order.ConfirmOrder ConfirmOrder { get; set; }
        [Parameter] public bool ConfirmButton { get; set; }

        void Confirm()
        {
            ConfirmOrder.Confirm = true;
            ConfirmOrder.Status = Common.Const.Status.Progress;
            MudDialog.Close(DialogResult.Ok(ConfirmOrder));
        }
        void Cancel() => MudDialog.Cancel();
        private string FormatTime(System.DateTime dateTime)
        {


            return dateTime.ToString("MM/dd/yyyy h:mm tt");
        }
        private string GetPrice(List<Common.Dto.MenuItemDto>? items)
        {
            if (items != null && items.Count > 0)
            {
                var price = items.Sum(i => i.TotalPrice);
                return price.ToString("C", new CultureInfo("ar-EG"));
            }
            else
                return string.Empty;
        }
    }
}