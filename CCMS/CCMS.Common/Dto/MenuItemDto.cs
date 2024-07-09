using System;

namespace CCMS.Common.Dto
{
    public class MenuItemDto
    {
        public Guid Id { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public byte[]? Picture { get; set; }
        public string Name { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public int CountOrders { get; set; }    
    }
}
