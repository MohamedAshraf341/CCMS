using CCMS.BE.Services;
using CCMS.Common.Const;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Request.Phone;
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
        private readonly PhoneService _phoneService;

        public BranchPhoneController(PhoneService PhoneService)
        {
            _phoneService = PhoneService;
        }
        [HttpGet(Router.BranchPhone.GetBranchPhones+"/{branchId}")]
        public async Task<IActionResult> GetBranchPhones(Guid branchId)
        {
            var items = await _phoneService.GetPhonesByBranch(branchId);
            return Ok(items);
        }
        [HttpPost(Router.BranchPhone.AddBranchPhone)]
        public async Task<IActionResult> AddBranchPhone(AddOrEditPhone model)
        {
            var item = await _phoneService.AddPhone(model);
            return Ok(item);
        }
        [HttpPut(Router.BranchPhone.EditBranchPhone)]
        public async Task<IActionResult> EditBranchPhone(AddOrEditPhone model)
        {
            var item = await _phoneService.EditPhone(model);
            return Ok(item);
        }
        [HttpDelete(Router.BranchPhone.DeleteBranchPhone + "/{id}")]
        public async Task<IActionResult> DeleteBranchPhone(Guid id)
        {
            var item = await _phoneService.DeletePhone(id);
            return Ok(item);
        }
    }
}
