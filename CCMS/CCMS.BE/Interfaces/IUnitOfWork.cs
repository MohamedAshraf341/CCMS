using CCMS.BE.Data.Models;
using System;
using System.Threading.Tasks;

namespace CCMS.BE.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IVerifyCodeRepository VerifyCodes {  get; }
    IOrderRepository Order { get; }
    IBaseRepository<MenuItemOrder> MenuItemOrder {  get; }
    IBaseRepository<Update> Update { get; }
    IBaseRepository<UpdateOrder> UpdateOrder { get; }
    IRestaurantRepository Restaurant { get; }
    IBrancheRepository Branche { get; }
    IBranchPhoneRepository BranchPhone { get; }
    Task<int> CompleteAsync();

}
