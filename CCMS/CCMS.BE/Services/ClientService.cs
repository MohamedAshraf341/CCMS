using CCMS.BE.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Services
{
    public class ClientService
    {
        private readonly IUnitOfWork _uow;

        public ClientService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<Common.Dto.Response.Client.GetClients> GetClients(Common.Dto.Request.Client.GetClients req)
        {
            try
            {
                var items = await _uow.Customer.GetAllWithInclude(req);
                var customers=items.Select(c => new Common.Dto.CustomerDto
                {
                    Address = c.Address,
                    Id = c.Id,
                    Name = c.Name,
                    Phone = c.Phone,
                    CountOrder= c.Orders.Count,
                }).ToList();
                var res= new Common.Dto.Response.Client.GetClients { Success= true ,Message="List of Customer",Customers=customers};
                return res;
            }
            catch(Exception ex) 
            {
                return new Common.Dto.Response.Client.GetClients { Success=false,Message=ex.Message };
            }
        }
    }
}
