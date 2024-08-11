using CCMS.FE.UI.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.FE.UI.Components.Branch
{
    public partial class PhoneNumbers
    {
        [Inject] ApiClient ApiClient { get; set; }

        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public Guid BranchId { get; set; }

        private IEnumerable<Common.Dto.PhoneNumberDto>? Items =new List<Common.Dto.PhoneNumberDto>();
        bool Loading { get; set; } = false;

        void Cancel() => MudDialog.Cancel();
        protected async override Task OnInitializedAsync()
        {
            await LoadItems();
        }
        private async Task LoadItems()
        {
            Loading = true;
            try
            {
                var items = await ApiClient.BranchPhone.GetBranchPhones(BranchId);
                Items = items;
            }
            finally
            {
                Loading = false;
            }
            StateHasChanged();
        }
        private void AddNewItem()
        {
            var newItem = new Common.Dto.PhoneNumberDto { IsEditing = true };
            Items = Items.Append(newItem);
            StateHasChanged();
        }

        private void EditItem(Common.Dto.PhoneNumberDto item)
        {
            item.IsEditing = true;
            StateHasChanged();
        }

        private async Task SaveItem(Common.Dto.PhoneNumberDto item)
        {
            // Call API to save the item (either add or update)
            item.IsEditing = false;
            var addPhone=new Common.Dto.Request.Phone.AddOrEditPhone {Id=item.Id, BranchId=BranchId,PhoneNumber=item.PhoneNumber };
            if (item.Id == Guid.Empty)
            {
                // New item, add it to the database
                var response = await ApiClient.BranchPhone.AddBranchPhone(addPhone);
                // Update the item's Id with the Id from the newly created item in the database
                item.Id = response.PhoneNumber.Id;
            }
            else
            {
                // Existing item, update it in the database
                addPhone.Id = item.Id;
                await ApiClient.BranchPhone.EditBranchPhone(addPhone);
            }
            StateHasChanged();
        }

        private void CancelEdit(Common.Dto.PhoneNumberDto item)
        {
            if (item.Id == Guid.Empty)
            {
                // Remove the item if it's a new, unsaved item
                Items = Items.Where(i => i != item);
            }
            else
            {
                item.IsEditing = false;
            }
            StateHasChanged();
        }

        private async Task DeleteItem(Common.Dto.PhoneNumberDto item)
        {
            // Call API to delete the item
            await ApiClient.BranchPhone.DeleteBranchPhone(item.Id);
            Items = Items.Where(i => i != item);
            StateHasChanged();
        }




    }
}