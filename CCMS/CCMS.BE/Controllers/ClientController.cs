using CCMS.BE.Services;
using CCMS.Common.Const;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CCMS.BE.Controllers
{
    [Route(Router.Root)]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _ClientService;

        public ClientController(ClientService ClientService)
        {
            _ClientService = ClientService;
        }
        [HttpPost(Router.Client.GetClients)]
        public async Task<IActionResult> GetClientes(Common.Dto.Request.Client.GetClients model)
        {
            var items = await _ClientService.GetClients(model);
            return Ok(items);
        }
    }
}
