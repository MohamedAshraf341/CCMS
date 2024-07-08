using CCMS.BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CCMS.Common.Const;
using CCMS.Common.Dto.Request.Branch;

namespace CCMS.BE.Controllers
{
    [Route(Router.Root)]
    [ApiController]
    public class BrancheController : ControllerBase
    {
        private readonly BranchService _branchService;

        public BrancheController(BranchService branchService)
        {
            _branchService = branchService;
        }
        [HttpPost(Router.Branche.GetBranches )]
        public async Task<IActionResult> GetBranches(Common.Dto.Request.Branch.GetBranches model)
        {
            var items = await _branchService.GetBranches(model);
            return Ok(items);
        }
        [HttpPost(Router.Branche.AddBranche)]
        public async Task<IActionResult> AddBranche(AddOrEditBranche model)
        {
            var item = await _branchService.AddBranche(model);
            return Ok(item);
        }
        [HttpPut(Router.Branche.EditBranche)]
        public async Task<IActionResult> EditBranche(AddOrEditBranche model)
        {
            var item = await _branchService.EditBranche(model);
            return Ok(item);
        }
        [HttpDelete(Router.Branche.DeleteBranche + "/{id}")]
        public async Task<IActionResult> DeleteBranche(Guid id)
        {
            var item = await _branchService.DeleteBranch(id);
            return Ok(item);
        }
    }
}
