using System.Collections.Generic;

namespace CCMS.Common.Dto.Response.Branch
{
    public class GetBranches:BaseResponse
    {
        public List<BranchDto>? Branches { get; set; }

    }
}
