using CCMS.BE.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CCMS.BE.Interfaces;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<IEnumerable<Customer>> GetAllWithInclude(Common.Dto.Request.Client.GetClients req);
    Task<Customer> GetByPhoneNumber(string phoneNumber,Guid branchId);
}
