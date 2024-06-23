using CCMS.BE.Services;
using CCMS.Common.Const;
using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CCMS.BE.Controllers
{
    [Route(Router.Root)]
    [ApiController]
    public class BranchPhoneController : ControllerBase
    {
        private readonly ReasturantService _reasturantService;

        public BranchPhoneController(ReasturantService reasturantService)
        {
            _reasturantService = reasturantService;
        }
        [HttpGet(Router.BranchPhone.GetBranchPhones+"/{branchId}")]
        public async Task<IActionResult> GetBranchPhones(Guid branchId)
        {
            var items = await _reasturantService.GetPhonesByBranch(branchId);
            return Ok(items);
        }
        [HttpPost(Router.BranchPhone.AddBranchPhone)]
        public async Task<IActionResult> AddBranchPhone(PhoneNumbersDto model)
        {
            var item = await _reasturantService.AddPhone(model);
            return Ok(item);
        }
        [HttpPut(Router.BranchPhone.EditBranchPhone)]
        public async Task<IActionResult> EditBranchPhone(PhoneNumbersDto model)
        {
            var item = await _reasturantService.EditPhone(model);
            return Ok(item);
        }
        [HttpDelete(Router.BranchPhone.DeleteBranchPhone + "/{id}")]
        public async Task<IActionResult> DeleteBranchPhone(Guid id)
        {
            var item = await _reasturantService.DeletePhone(id);
            return Ok(item);
        }
    }
}
