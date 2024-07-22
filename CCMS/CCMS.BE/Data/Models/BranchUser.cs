using System;

namespace CCMS.BE.Data.Models
{
    public class BranchUser
    {
        public Guid Id { get; set; }
        public Guid BranchId { get; set; }

        public Branch Branch { get; set; }
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
