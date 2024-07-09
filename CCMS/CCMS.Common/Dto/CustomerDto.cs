using System;


namespace CCMS.Common.Dto
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int CountOrder { get; set; } = 0;
    }
}
