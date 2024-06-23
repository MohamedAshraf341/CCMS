using System;
namespace CCMS.Common.Dto.Response
{
    public class GetReasturantResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int BranchCount { get; set; }

    }
}
