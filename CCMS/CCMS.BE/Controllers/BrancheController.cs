using CCMS.BE.Services;
using CCMS.Common.Dto.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CCMS.Common.Const;
using CCMS.Common.Dto.Request;

namespace CCMS.BE.Controllers
{
    [Route(Router.Root)]
    [ApiController]
    public class BrancheController : ControllerBase
    {
        private readonly ReasturantService _reasturantService;

        public BrancheController(ReasturantService reasturantService)
        {
            _reasturantService = reasturantService;
        }
        [HttpGet(Router.Branche.GetBranches + "/{reasturantId}")]
        public async Task<IActionResult> GetBranches(Guid reasturantId)
        {
            var items = await _reasturantService.GetBranches(reasturantId);
            return Ok(items);
        }
        [HttpPost(Router.Branche.AddBranche)]
        public async Task<IActionResult> AddBranche(AddBrancheRequest model)
        {
            var item = await _reasturantService.AddBranche(model);
            return Ok(item);
        }
        [HttpPut(Router.Branche.EditBranche)]
        public async Task<IActionResult> EditBranche(AddBrancheRequest model)
        {
            var item = await _reasturantService.EditBranche(model);
            return Ok(item);
        }
        [HttpDelete(Router.Branche.DeleteBranche + "/{id}")]
        public async Task<IActionResult> DeleteBranche(Guid id)
        {
            var item = await _reasturantService.DeleteBranch(id);
            return Ok(item);
        }
    }
}
