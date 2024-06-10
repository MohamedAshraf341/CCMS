using CCMS.BE.Data.Models;
using System;
using System.Threading.Tasks;

namespace CCMS.BE.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IVerifyCodeRepository VerifyCodes {  get; }
    IOrderRepository OrderRepository { get; }
    IBaseRepository<MenuItemOrder> MenuItemOrder {  get; }
    IBaseRepository<Update> Update { get; }
    IBaseRepository<UpdateOrder> UpdateOrder { get; }

    Task<int> CompleteAsync();

}
